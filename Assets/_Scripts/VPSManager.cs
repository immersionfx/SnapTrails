using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Google.XR.ARCoreExtensions;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;
using TMPro;


public class VPSManager : MonoBehaviour
{
    [SerializeField] private AREarthManager earthManager;
    [SerializeField] private ARAnchorManager aRAnchorManager;

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private GameObject frame;
    [SerializeField] private GameObject buttonsView;

    [SerializeField] private AudioSource audioClick;
    [SerializeField] private GameObject photoPrefab;
    [SerializeField] private GameObject videoPrefab;
    [SerializeField] private Camera cam;
    
    bool userPositionSet = false;

    [System.Serializable]
    public struct Coords
    {
        public string latitude;
        public string longitude;
    }

    // List to store new map Waypoints created with each new photo taken
    public List<Coords> waypointsList = new List<Coords>();

    //User position (updated every 2 secs)
    public string userLat, userLong;

    private byte[] byteArray; //stores the image as PNG

    private GameObject newFramePrefab;
    private RawImage imgPlaceholder;
    private TMP_Text datePlaceholder;

    private GameObject InfoText;
    private bool objectsPlaced = false;

    int screenWidth, screenHeight;
	ARGeospatialAnchor geoAnchor;
		
    
    void Start()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height - 800;

        messagePanel.SetActive(false);

        if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB32)) {
            Debug.LogError("## Render texture format not supported on this phone!");
        }

        InvokeRepeating("getUserPosition", 2f, 2f);
    }


    void Update()
    {
        //Exit to Menu
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("Start");
        }
    }

    //Update user lat/lon every 2 seconds
    void getUserPosition()
    {
        if (earthManager.EarthTrackingState != TrackingState.Tracking) return;
        var geospatialPose = earthManager.CameraGeospatialPose;
        userLat = geospatialPose.Latitude.ToString();
        userLong = geospatialPose.Longitude.ToString();
    }

    // Get Shot ////////////////////////////////////////////////////////////////////////////////////

    //
    // Camera Button 
    //
    public void GetCameraPose(bool isPhoto)
    {
        Debug.Log("@@ Take Shot");
        audioClick.Play();

        if (earthManager.EarthTrackingState == TrackingState.Tracking)
        {
            var geospatialPose = earthManager.CameraGeospatialPose;
            Debug.LogFormat("@@@ [Lat, Lon]: {0}, {1}", geospatialPose.Latitude, geospatialPose.Longitude);

            // Create a new waypoint struct
            Coords newWaypoint = new Coords
            {
                latitude = geospatialPose.Latitude.ToString(),
                longitude = geospatialPose.Longitude.ToString()
            };

            // Add the new waypoint to the list
            waypointsList.Add(newWaypoint); 

            geoAnchor = ARAnchorManagerExtensions.AddAnchor(aRAnchorManager, geospatialPose.Latitude, geospatialPose.Longitude, geospatialPose.Altitude, Quaternion.identity);
			geoAnchor.transform.position = new Vector3(geoAnchor.transform.position.x, transform.position.y, geoAnchor.transform.position.z) + Camera.main.transform.forward * 2f;
            
            StartCoroutine(PlaceContentWhenAnchorCreated(isPhoto));            
        }
        else if (earthManager.EarthTrackingState == TrackingState.Limited)
        {
            Debug.Log("[GetCameraPose] EarthTrackingState = Limited.");
            StartCoroutine(showUnavailabilityPanel("AR Tracking is limited.\n\nPlease connect to the internet,\nfind an open outdoors space\nand try again in a few seconds"));
        }
        else if (earthManager.EarthTrackingState == TrackingState.None)
        {
            Debug.Log("[GetCameraPose] EarthTrackingState = None.");
            StartCoroutine(showUnavailabilityPanel("AR Tracking is non existent.\n\nPlease connect to the internet,\nfind an open outdoors space\nand try again in a few seconds"));
        }
    }


    IEnumerator PlaceContentWhenAnchorCreated(bool isPhoto)
    {
        bool isAnchorReady = false;
		bool noError = true;

        //Create the image as PNG and store it to byteArray
        if (isPhoto) SaveCameraView();

		yield return new WaitForSeconds(.5f);

        if (isPhoto)
            newFramePrefab = Instantiate(photoPrefab, geoAnchor.transform);
        else
            newFramePrefab = Instantiate(videoPrefab, geoAnchor.transform);

        Debug.Log("@@Photo Placed");
		isAnchorReady = true;
		
        yield return new WaitForSeconds(0.5f);

        if (isAnchorReady) {
            Debug.Log("@@ Anchor Instantiated");
            Transform rawImage = newFramePrefab.transform.Find("PhotoBox/Buttons/Canvas/ImgPlaceholder");            
            if (rawImage == null)
            {
                Debug.LogError("## No rawImage found");
            }
            else
            {
                Transform timestamp = newFramePrefab.transform.Find("PhotoBox/Buttons/Canvas/TimestampTxt");
                if (timestamp == null)
                {
                    Debug.LogError("## No timestampTxt found");
                }
                else
                {                    
                    imgPlaceholder = rawImage.GetComponent<RawImage>();
                    datePlaceholder = timestamp.GetComponent<TMP_Text>();
                    if (imgPlaceholder == null) {
                        Debug.LogError("## No imgPlaceholder found");
                    }
                    else if (isPhoto)
                    {
                        updateRawImage();                        
                    }
                }  
            }
        }
    }

    //Create the image as PNG and store it to byteArray
    void SaveCameraView()
    {
        Debug.Log("@@ Screenshot dimensions: Width = " + screenWidth + ", Height = " + screenHeight);
        RenderTexture screenTexture = new RenderTexture(screenWidth, screenHeight, 16);
        cam.targetTexture = screenTexture;
        RenderTexture.active = screenTexture;
        cam.Render();
        Texture2D renderedTexture = new Texture2D(screenWidth, screenHeight, TextureFormat.RGB24, false); //true: if you need mipmaps or not
        renderedTexture.ReadPixels(new Rect(0, 0, screenWidth, screenHeight), 0, 0);
        RenderTexture.active = null;
        byteArray = renderedTexture.EncodeToPNG();

        //Optionally, save image to Application.dataPath + "/screenshots
        //fileName = "SS" + System.DateTime.Now.ToString("HHmmddyy") + ".png";
        //File.WriteAllBytes(Application.dataPath + "/screenshots/" + fileName, byteArray);        
    }

    //Put the image and the Date onto PhotoPrefab
    void updateRawImage()
    {
        //Date
        string dateNow = System.DateTime.Now.ToString("MMMM dd, yyyy");
        Debug.Log("@@ Date is: " + dateNow);
        datePlaceholder.text = dateNow;

        //Image
        cam.targetTexture = null;
        Texture2D thisTexture = new Texture2D(screenWidth, screenHeight);
        thisTexture.LoadImage(byteArray);
        imgPlaceholder.color = Color.white;
        imgPlaceholder.texture = thisTexture;
    }

    IEnumerator showUnavailabilityPanel(string m_Text)
    {
        messagePanel.SetActive(true);
        messageText.text = m_Text;
        frame.SetActive(false);
        buttonsView.SetActive(false);
        yield return new WaitForSeconds(5f);
        messagePanel.SetActive(false);
        messageText.text = "";
        frame.SetActive(true);
        buttonsView.SetActive(true);
    }

}