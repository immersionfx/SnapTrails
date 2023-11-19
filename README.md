# SnapTrails - Picture Your Way

We all take photos on our trips, but what if these photos could help you plan your future adventures? 

## Made With

- Unity 2021.3.22f1 (URP)
- Geospatial Creator
- Google Maps Javascript API
- UniWebView
- Native Gallery

## Description of the app

Introducing SnapTrails. The app that revolutionizes the way you see, share, and savor your travels. 

We crafted SnapTrails with the dynamic duo of Google Geospatial Creator and Google's Javascript Map API to make it like a travel diary, but with a sprinkle of AR magic! With SnapTrails, you can follow in the footsteps of fellow explorers and view the world through their lenses - literally! Here's how:

A user embarks on a city tour and utilizes the smartphone app to capture photos or videos of intriguing landmarks during their journey. These media are then securely stored in a cloud database, complete with their geo-coordinates indicating the exact location where each one was taken.

The app then crafts a tailored one-day travel plan by considering the specific route the user took. It strategically designates stops at locations that align with the landmarks and attractions the user found intriguing and captured in photographs or videos during their journey. This individualized itinerary is referred to as a 'trail' within the app. These trails, essentially a curated sequence of recommended stops based on user experiences, form a collection within the app. 

Subsequent users of the app have the option to browse through the available trails, including the one created by the original user. If they choose to follow a particular trail, the app doesn't just show you the way, but it also brings the photo and video memories to life in an augmented world. In other words, you can view these shots as part of the real environment, step into the past, right where that epic sunset photo was taken, and feel like you're walking in the footsteps of your own memories. The app also provides you with a 2D map that displays directions and designated stopping points that correspond to locations where the original user captured the moment during their itinerary.

And what's a journey without a little competition? Rate and be rated! Each of these photos and videos can be rated by fellow travelers by pressing the Like button in AR space. All individual ratings contribute to the overall rating of the entire trail. This approach simplifies the process of discovering the top-rated or recommended trails from the available options when you plan to revisit the city in the future, so you can experience the very best a city has to offer, according to travelers just like you.

Another button allows you to repost your photos and videos on your social media accounts in real time. It's like keeping a live travel blog and sharing your travel adventures with your friends.

If you're ready to turn your travel shooting memories into an adventure of a lifetime, take a SnapTrail today and explore the world like never before. Embark on an unforgettable journey, one snapshot at a time!

## Get started video

- https://www.youtube.com/watch?v=MDcyG9MAMAo

- https://developers.google.com/ar/geospatialcreator/unity/quickstart

- ARCore Extensions: https://developers.google.com/ar/develop/unity-arf/getting-started-extensions

- com.cesium.unity-1.6.4.tgz: https://github.com/CesiumGS/cesium-unity/releases/

## Install AR Foundation

- AR Foundation 4.2.9 (from Package Manager)

- Google ARCore XR plugin (from Package Manager)

- Edit > Project Settings. In XR Plug-in Management, open the Android tab and enable ARCore.

## Android Setup

- Switch to Android

- Player Settings - Other Settings:

    - Uncheck Auto Graphics API.

    - Graphics API > Disable / Remove Vulkan

    - Android 7.0 'Nougat' (API Level 24) or higher

    - Scripting Backend = IL2CPP

    - ARM64 Only (Enable both 32-bit (ARMv7) and 64-bit (ARM64) to meet Play Store 64-bit requirements)

    - Write Permission: "External (SDCard)"

- Gradle: installed with Unity

## APIs Enabled in Google Cloud Console

- ARCore API
- Map Tiles API

## ARCore Extensions package

Add package from git URL...  https://github.com/google-ar/arcore-unity-extensions.git

Package Manager - ARCore Extensions package - Samples: Import Geospatial (Android only)

Project Settings... > XR Plug-in Management - ARCore Extensions panel. Ensure "Geospatial" is checked (you don't need to enable the Geospatial Creator just yet)

Project Settings... > XR Plug-in Management - ARCore Extensions - Android Authentication Strategy : API Key. Set your ARCore Android API key
AIzaSyDDmFegJAHDL4SbFZZBq_sCTa13WZ3V4UM

Assets/Samples/ARCore Extensions/1.40.0/Geospatial Sample/Configurations/GeospatialConfig.asset: Enable Geospatial

## Install Cesium

Download the most recent version of the precompiled .tgz of Cesium for Unity (com.cesium.unity-1.6.4.tgz) from their GitHub Releases page: https://github.com/CesiumGS/cesium-unity/releases/

Package Manager - "Add package from tarball"

Project Settings... > XR Plug-in Management - ARCore Extensions panel. Enable the Geospatial Creator

## Resources

- Read about the new Geospatial Creator → https://goo.gle/3pdk2fE
- Get started with Geospatial Creator in Unity → https://goo.gle/gc-unity
- Visit our Geospatial Creator website → https://goo.gle/geospatialcreator   

## Write Image to Gallery (Android)

Set Write Permission to External (SDCard) in Player Settings. 
https://github.com/yasirkula/UnityNativeGallery

    // Save the screenshot to Gallery/Photos
    string name = string.Format("{0}_{1}.png", Application.productName, System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss"));
    NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(ss, Application.productName + " Captures", name + ".jpg");
    Debug.Log("@@@ Permission result: " + permission );

On Android, images are saved at DCIM/album/snap-trails Captures


## Application.persistentDataPath

Windows: C:\Users\tsotr\AppData\LocalLow\Manolis Tsotros\...

iOS: /var/mobile/Containers/Data/Application/<guid>/Documents.

Android: /storage/emulated/0/Android/data/<packagename>/files on most devices (some older phones might point to location on SD card if present)

## Troubleshoot

- Google's troubleshooting page:

    https://github.com/google-ar/arcore-unity-extensions/issues
    
- https://developers.google.com/ar/geospatialcreator/unity/quickstart#troubleshooting

- The app displays a black screen:

    Unity's Universal Rendering Pipeline (URP) is not configured for the AR Camera by default. To add the feature, search for "renderer" in your project. For each URP Renderer object, click the "Add Renderer Feature" button in the Inspector and add the AR Background Renderer Feature.

- Why is my anchor moving?  

    You may notice, especially when the app first launches, that the anchor may appear to slowly "drift". This occurs when the VPS session is still determining and refining the precise location of the camera. You may wish to render the object differently (or not at all) until the location accuracy reaches a certain threshold. To do so, you can query the ARCoreEarthManager.CameraGeospatialPose property to determine the accuracy for the current frame update.

- Assembly 'Assets/External/ExternalDependencyManager/Editor/Google.IOSResolver.dll' will not be loaded due to errors

    1. Just delete Assets/ExternalDependencyManager folder (or from package manager if you have added to packages)
    2. Import the latest package(EDM4U) from EXTERNAL/external-dependency-manager-latest.unitypackage.

- Shadow on character is flickering

    Shadows might flicker if they’re far away from the camera. If shadows are closer to the camera than the world space origin, enable camera-relative culling. Unity uses the camera as the relative position for shadow calculations instead of the world space origin, which reduces flickering.

    To enable camera-relative culling, follow these steps:

    1. Go to Project Settings > Graphics > Culling Settings > Camera-Relative Culling.
    2. Enable Shadows.

- Incompatible API Key

    Same API Key must exist in:
        1. Project Settings... > XR Plug-in Management - ARCore Extensions - Android Authentication Strategy : API Key
        2. Hierarchy - AR Geospatial Creator Origin

- Nothing happens when I try to access the Gallery on Android

    Make sure that you've set the Write Permission to "External (SDCard)" in Player Settings - Other Settings.
