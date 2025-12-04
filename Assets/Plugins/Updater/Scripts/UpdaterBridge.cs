using System;
using UnityEngine;

namespace Updater
{
    public sealed class UpdaterBridge : IUpdaterBridge
    {
        private AndroidJavaObject plugin;

        public UpdaterBridge()
        {
            ConnectToPlugin();
        }

        private void ConnectToPlugin()
        {
#if !UNITY_EDITOR && PLATFORM_ANDROID
            if (Application.platform == RuntimePlatform.Android)
            {
                using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                    plugin = new AndroidJavaObject("com.nemajor.unityapkinstaller.APKInstallerPlugin", activity);
                }
            }
#endif
#if UNITY_EDITOR
            Debug.Log("Connect to plugin");
#endif
        }

        public void InstallApk(string filePath)
        {
#if !UNITY_EDITOR && PLATFORM_ANDROID
            if (Application.platform == RuntimePlatform.Android)
            {
                try
                {
                    plugin.Call("installApk", filePath);
                }
                catch (Exception ex)
                {
                    Debug.Log($"Exception when try load apk: {ex.Message}");
                }

            }
            else
            {
                Debug.LogWarning("APK installation works only on Android!");
            }
#endif
#if UNITY_EDITOR
            Debug.Log("Install apk");
#endif
        }

        public bool RequestInstallUnknownSourcesPermission()
        {
#if !UNITY_EDITOR && PLATFORM_ANDROID

            if (Application.platform == RuntimePlatform.Android)
            {
                try
                {
                    return plugin.Call<bool>("checkAndRequestInstallPermission");
                }
                catch (Exception ex)
                {
                    Debug.Log($"Exception when try request permission: {ex.Message}");
                    return false;
                }

            }
            else
            {
                Debug.LogWarning("Request permession works only on Android!");
                return false;
            }
#endif
#if UNITY_EDITOR
            Debug.Log("Request permission");
            return false;
#endif
        }
    }
}