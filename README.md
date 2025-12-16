## Features

- Download APK with real progress bar (`DownloadHandlerFile`)
- Automatic request for **Install unknown apps** permission (Android 8.0+)
- Secure file sharing via `FileProvider` (no `FileUriExposedException`)
- Clean abstraction via `IUpdaterBridge` interface (easy to mock/test)
- Zero external dependencies
- Safe `#if UNITY_EDITOR` guards

## Table of Contents

- [Quick Start](#quick-start)
- [How to Use](#how-to-use)
- [Project Structure](#project-structure)
- [Required AndroidManifest.xml](#required-androidmanifestxml)
- [Testing the Sample](#testing-the-sample)
- [Limitations & Tips](#limitations--tips)
- [License](#license)

## Quick Start

1. Clone or download this repository
2. Open the project in Unity (2018.4 or newer)
3. Switch platform to **Android**
4. In the sample scene, select the GameObject with `NewMonoBehaviourScript`
5. Paste a **direct link** to any `.apk` file into the **Url** field  
   (e.g. GitHub Release asset, your server, Dropbox, Google Drive direct link, etc.)
6. Build & run on a real Android device
7. Press the button → APK downloads → permission screen (if needed) → installer opens

Done! Your app just updated itself.

## How to Use

### 1. Core Components

- `UpdaterBridge.cs` – wrapper around the native Java plugin (`IUpdaterBridge`)
- `NewMonoBehaviourScript.cs` – downloader with progress callbacks
- `APKInstallerPlugin.java` – native Android plugin (already included)

### 2. Example Usage (Recommended)

```csharp
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Updater;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private UpdaterBridge updater;

    public string url = "";
    public Image im;

    public Action<float> OnProgressChanged;
    public Action<string> OnDownloadFailed;
    public Action<string> OnDownloadCompleted;

    public void Load()
    {
        StartCoroutine(DownloadCoroutine(url));
    }

    private void OnLoad(float amount)
    {
        im.fillAmount = amount;
    }

    private IEnumerator DownloadCoroutine(string url) 
    { 
        string filePath = Path.Combine(Application.persistentDataPath, "downloaded_app.apk"); 
        using (UnityWebRequest request = UnityWebRequest.Get(url)) 
        { 
            request.downloadHandler = new DownloadHandlerFile(filePath); 
            request.SendWebRequest(); 
            while (!request.isDone) 
            {
                OnProgressChanged?.Invoke(request.downloadProgress); 
                yield return null; 
            } 

            if (request.result != UnityWebRequest.Result.Success) 
            { 
                Debug.LogError($"Download failed: {request.error}");
                OnDownloadFailed?.Invoke(request.error);
                yield break; 
            }

            OnProgressChanged?.Invoke(1f);

            OnDownloadCompleted?.Invoke(filePath); 

            Debug.Log($"Download finished: {filePath}");
        } 
    }
}
```
3. Request Permission Manually (Optional)
```C#
if (!updater.RequestInstallUnknownSourcesPermission())
{
    // Permission screen was opened — show your own UI:
    // "Please allow installation from this app, then return here"
}
```
```
Project Structure
textAssets/
├── Plugins/Android/
│   ├── APKInstallerPlugin.java
│   ├── AndroidManifest.xml
│   └── res/xml/file_paths.xml
├── Scripts/
│   ├── Updater/
│   │   ├── IUpdaterBridge.cs
│   │   └── UpdaterBridge.cs
│   └── NewMonoBehaviourScript.cs        # Downloader + progress
└── Scenes/
    └── SampleScene.unity                # UI with button & progress bar
```
Required AndroidManifest.xml
Make sure your Assets/Plugins/Android/AndroidManifest.xml contains:
```xml
<uses-permission android:name="android.permission.REQUEST_INSTALL_PACKAGES" />
<?xml version="1.0" encoding="utf-8"?>
<manifest
    xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:tools="http://schemas.android.com/tools">

	<!-- Для Android 10 и ниже -->
	<uses-permission android:name="android.permission.READ_EXTERNAL_STORAGE"
        android:maxSdkVersion="28" />
	<uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE"
        android:maxSdkVersion="28" />

	<!-- Для Android 11+ -->
	<uses-permission android:name="android.permission.MANAGE_EXTERNAL_STORAGE"
        android:minSdkVersion="30"
        tools:ignore="ScopedStorage" />

	<!-- Для установки APK на Android 8.0+ -->
	<uses-permission android:name="android.permission.REQUEST_INSTALL_PACKAGES" />
	
    <application>
        <!--Used when Application Entry is set to GameActivity, otherwise remove this activity block-->
        <activity android:name="com.unity3d.player.UnityPlayerGameActivity"
                  android:theme="@style/BaseUnityGameActivityTheme">
            <intent-filter>
                <action android:name="android.intent.action.MAIN" />
                <category android:name="android.intent.category.LAUNCHER" />
            </intent-filter>
            <meta-data android:name="unityplayer.UnityActivity" android:value="true" />
            <meta-data android:name="android.app.lib_name" android:value="game" />
        </activity>

		<provider
                android:name="androidx.core.content.FileProvider"
                android:authorities="${applicationId}.fileprovider"
                android:exported="false"
                android:grantUriPermissions="true">
			<meta-data
				android:name="android.support.FILE_PROVIDER_PATHS"
				android:resource="@xml/file_paths" />
		</provider>
    </application>
</manifest>
```
file_paths.xml is already included and allows access to persistentDataPath, cache, and external storage.

## Testing the Sample

Host any valid APK file online (direct download link required)
Put the URL into the inspector field
Install the app on a real Android device
Tap the button
On first use (Android 8+): allow "Install unknown apps" for your app
Watch the installer appear

## Limitations & Tips

The user must allow "Install unknown apps" once (Android 8+)
Always use direct APK links (no redirects or HTML pages)
Application.persistentDataPath is recommended for storing the APK
On some devices (Xiaomi, Huawei) additional MIUI/HarmonyOS permissions may be required
Consider adding retry logic after the user returns from Settings

## License
This project is licensed under the MIT License – see the LICENSE file for details.
Feel free to fork, modify, and use in commercial projects.
Made with love for the Unity community by ini-dev2

## Author
ini-dev2 - [GitHub Profile](https://github.com/ini-dev2)
Made with ❤️ for the Unity community

## Readme
ReadMe was created using neural networks, there may be inaccuracies.

December 2025
