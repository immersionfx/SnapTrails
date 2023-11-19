using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class Screenshot : MonoBehaviour
{
    public List<GameObject> UIList;
    public List<GameObject> RandomFrame;
    public GameObject savedAt;

    private string directoryName = "GeoCricketAR";
    private int rndFrame;

    [SerializeField] private AudioSource audioS;
    [SerializeField] private GameObject buttonsView;
    [SerializeField] private RawImage photoImage;
    private string fileName;

    private GameObject newFramePrefab;
    [SerializeField] private GameObject photo;
    private RawImage imgPlaceholder;

    private void Start()
    {
        savedAt.SetActive(false);
    }


    public void clickTakeScreenshot()
    {
        Debug.Log("@@ Take Shot");
        audioS.Play();

        newFramePrefab = Instantiate(photo, null);

        //Take the shot
        //SaveCameraViewToRawImage(Camera.main);
        Transform rawImage = newFramePrefab.transform.Find("PhotoBox/Buttons/Canvas/ImgPlaceholder");
        if (rawImage == null)
        {
            Debug.LogError("No rawImage found");
        }
        else {
            imgPlaceholder = rawImage.GetComponent<RawImage>();
            if (imgPlaceholder == null) Debug.LogError("No imgPlaceholder found");
            SaveCameraViewToDisk(Camera.main);
        }
            
    }


    void SaveCameraViewToRawImage(Camera cam)
    {
        RenderTexture screenTexture = new RenderTexture(Screen.width, Screen.height, 32);
        cam.targetTexture = screenTexture;
        RenderTexture.active = screenTexture;
        cam.Render();
        Texture2D renderedTexture = new Texture2D(Screen.width, Screen.height);
        renderedTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        RenderTexture.active = null;
        byte[] byteArray = renderedTexture.EncodeToPNG();
        Texture2D thisTexture = new Texture2D(Screen.width, Screen.height);
        thisTexture.LoadImage(byteArray);
        photoImage.color = Color.white;
        photoImage.texture = thisTexture;
        cam.targetTexture = null;
    }
    

    private IEnumerator TakeScreenshotAndSave()
    {
        yield return new WaitForEndOfFrame();
        
        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true); //true: if you need mipmaps or not
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();
        
        // Save the screenshot to Gallery/Photos
        string name = string.Format("{0}_{1}.png", Application.productName, System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss"));
	    NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(ss, Application.productName + " Captures", name + ".jpg");
        Debug.Log("@@@ Permission result: " + permission );

        yield return new WaitForSeconds(.5f);
        //ToggleCanvasItems(true);
        //RandomFrame[rndFrame].SetActive(false);

        if (permission != NativeGallery.Permission.Denied) {
            savedAt.SetActive(true);
            yield return new WaitForSeconds(3f);
            savedAt.SetActive(false);
        }
    }


    void SaveCameraViewToDisk(Camera cam)
    {
        int screenWidth = Screen.width;
        int screenHeight = Screen.height - 800;
        Debug.Log("Screenshot dimensions: Width = " + screenWidth + ", Height = " + screenHeight);
        RenderTexture screenTexture = new RenderTexture(screenWidth, screenHeight, 16);
        cam.targetTexture = screenTexture;
        RenderTexture.active = screenTexture;
        cam.Render();
        Texture2D renderedTexture = new Texture2D(screenWidth, screenHeight, TextureFormat.RGB24, true); //true: if you need mipmaps or not
        renderedTexture.ReadPixels(new Rect(0, 0, screenWidth, screenHeight), 0, 0);
        RenderTexture.active = null;
        byte[] byteArray = renderedTexture.EncodeToPNG();
        cam.targetTexture = null;

        // Save the screenshot to Application /screenshots folder
        fileName = "SS" + System.DateTime.Now.ToString("HHmmddyy") + ".png";
        File.WriteAllBytes(Application.dataPath + "/screenshots/" + fileName, byteArray);

        //Now display it in Frame
        Texture2D thisTexture = new Texture2D(screenWidth, screenHeight);
        thisTexture.LoadImage(byteArray);
        imgPlaceholder.color = Color.white;
        imgPlaceholder.texture = thisTexture;
    }


    void ToggleCanvasItems(bool show)
    {
        for (int i=0; i < UIList.Count; i++)
            UIList[i].SetActive(show);
    }

    void showRandomFrame()
    {
        rndFrame = UnityEngine.Random.Range(0, RandomFrame.Count);
        RandomFrame[rndFrame].SetActive(true);
    }
}
