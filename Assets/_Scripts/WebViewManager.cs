using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WebViewManager : MonoBehaviour
{
    UniWebView webView;
    public TMP_Text ttext;

    void Start()
    {
        // Set the log level to Verbose
        UniWebViewLogger.Instance.LogLevel = UniWebViewLogger.Level.Verbose;
        
        // Create a game object to hold UniWebView and add component.
        var webViewGameObject = new GameObject("UniWebView");
        webView = webViewGameObject.AddComponent<UniWebView>();

        ttext.text = "Loading..";
        webView.Frame = new Rect(0, 0, Screen.width, Screen.height - 100);
        //webView.Load("https://www.protothema.gr/");

        var url = UniWebViewHelper.StreamingAssetURLForPath("PlaceMarker.html");
        webView.Load(url);

        webView.OnPageFinished += (view, statusCode, url) =>
        {
            // Page load finished
            ttext.text = "Page load finished";
            webView.Show();
        };

        webView.OnPageErrorReceived += (view, error, message) =>
        {
            ttext.text = "Error: " + error + "   " + message;
        };

        webView.OnPageFinished += (view, statusCode, url) =>
        {
            ttext.text = "PageFinished: " + statusCode;
        };

        webView.OnShouldClose += (view) =>
        {
            Destroy(webView);
            webView = null;
            return true;
        };
    }

    void OnDestroy()
    {
        Destroy(webView);
        webView = null;
    }
}
