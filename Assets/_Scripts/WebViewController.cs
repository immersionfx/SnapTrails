using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WebViewController : MonoBehaviour
{
    [SerializeField] private UniWebView webView;
    [SerializeField] private VPSManager vpsManager;

    bool webViewShown = false;

    private static int MAPVIEWHEIGHT = 160;
    
    [Header("UI Elements")]
    [SerializeField] private GameObject distancePopup;
    [SerializeField] private TMP_Text distanceText;
    [SerializeField] private GameObject arrowUp, arrowDown;
    [SerializeField] private RectTransform mapView;

    //Origin/Destination Map Coords for testing
    const string originLat = "37.91357453450387";
    const string originLon = "23.75631038934201";
    const string destLat = "37.91145635457938";
    const string destLon = "23.754509238340518";

    private bool isUserAdded = false;
    

    void Start()
    {
        Debug.Log("@@@ Loading..");

        mapView.anchoredPosition = new Vector2(0, MAPVIEWHEIGHT);
        arrowUp.SetActive(true);
        arrowDown.SetActive(false);
        distancePopup.SetActive(false);

#if  !UNITY_EDITOR
        var url = UniWebViewHelper.StreamingAssetURLForPath("directions.html");
        webView.Load(url);

        webView.OnPageFinished += (view, statusCode, url) =>
        {
            // Page load finished
            Debug.Log("@@@ Page load finished");
            webView.Hide();  

            StartCoroutine(addAllWaypoints());          
        };

        webView.OnPageErrorReceived += (view, error, message) =>
        {
            Debug.LogError("Error: " + error + "   " + message);
        };
       
        webView.OnShouldClose += (view) =>
        {
            Destroy(webView);
            webView = null;
            return true;
        };
#endif
    }

    public void showHide()
    {                
        webView.Frame = new Rect(0, Screen.height / 2, Screen.width, Screen.height / 2);

        if (!webViewShown) {           
            webView.Show(true, UniWebViewTransitionEdge.Bottom, 0.35f);
            webViewShown = true;

#if !UNITY_EDITOR
            addUser();
            StartCoroutine(updateUserPosition());
            StartCoroutine(getDistance());
#endif

            StartCoroutine(slideUp()); //mapView.anchoredPosition = new Vector2(0, Screen.height/2 + mapView.sizeDelta.y);            
        }
        else {
            webView.Hide(true, UniWebViewTransitionEdge.Bottom, 0.35f);
            webViewShown = false;
            mapView.anchoredPosition = new Vector2(0, MAPVIEWHEIGHT);
        }

        distancePopup.SetActive(webViewShown);
        arrowUp.SetActive(!webViewShown);
        arrowDown.SetActive(webViewShown);
        if (webViewShown) 
            Debug.Log("@@@ WebView showing");
        else
            Debug.Log("@@@ WebView hidden");
    }

    IEnumerator slideUp()
    {
        float elapsedTime = 0;
        float waitTime = .5f;
        float heightThreshold = mapView.sizeDelta.y * .75f;

        while (elapsedTime < waitTime)
        {
            mapView.anchoredPosition = Vector2.Lerp(mapView.anchoredPosition, new Vector2(0, Screen.height / 2 + heightThreshold), elapsedTime / waitTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Make sure we got there
        mapView.anchoredPosition = new Vector2(0, Screen.height / 2 + heightThreshold);
        yield return null;
    }

    void OnDestroy()
    {
        Destroy(webView);
        webView = null;
    }


    //
    //Add User (just once)
    //
    public void addUser()
    {
        if (isUserAdded) return;
        webView.EvaluateJavaScript("addMarker(" + vpsManager.userLat + ", " + vpsManager.userLong + ")", (payload) =>
        {
            Debug.LogFormat("@@@ added User");
            isUserAdded = true;
        });
    }

    IEnumerator updateUserPosition()
    {
        while (webViewShown)
        {
            webView.EvaluateJavaScript("moveMarker(" + vpsManager.userLat + ", " + vpsManager.userLong + ")", (payload) =>
            {
                //
            });
            yield return new WaitForSeconds(2f);
        }
        yield return null;
    }

    IEnumerator getDistance()
    {
        while (webViewShown)
        {
            webView.EvaluateJavaScript("getSphericalDistance(" + vpsManager.userLat + ", " + vpsManager.userLong + ", " + destLat + ", " + destLon + ")", (payload) =>
            {
                Debug.LogFormat("@@@ Distance: {0} {1}", payload.data, "km");
                distanceText.text = payload.data;
            });
            yield return new WaitForSeconds(2f);
        }
        yield return null;
    }


    //
    //Add Waypoints to the route
    //
    public void addWaypoint(int count, string lat, string lng)
    {
        webView.EvaluateJavaScript("addWaypoint(" + lat + ", " + lng + ")", (payload) =>
        {
            Debug.LogFormat("@@@ added {0} Waypoint", count);
        });
    }


    IEnumerator addAllWaypoints()
    {
        yield return new WaitForSeconds(.5f);
        for (int i = 0; i < vpsManager.waypointsList.Count; i++) {
            addWaypoint(i+1, vpsManager.waypointsList[i].latitude, vpsManager.waypointsList[i].longitude);
            yield return new WaitForSeconds(.5f);
        }

        webView.EvaluateJavaScript("updateMap(" + originLat + ", " + originLon + ")", (payload) =>
        {
            Debug.Log("@@@ UpdatedMap");
        });
        
        yield return null;
    }

    // public void addMarker()
    // {
    //     var lat = "37.912787541753865";
    //     var lng = "23.756196527415323";
    //     webView.EvaluateJavaScript("addMarker(" + lat + ", " + lng + ")", (payload) =>
    //     {
    //         Debug.Log("@@@ addMarker: " + payload.data);
    //     });
    // }

    // public void moveMarker()
    // {
    //     var lat = "37.91287404414073";
    //     var lng = "23.755393819638687";
    //     webView.EvaluateJavaScript("moveMarker(" + lat + ", " + lng + ")", (payload) =>
    //     {
    //         Debug.Log("@@@ moveMarker: " + payload.data);
    //     });
    // }
}
