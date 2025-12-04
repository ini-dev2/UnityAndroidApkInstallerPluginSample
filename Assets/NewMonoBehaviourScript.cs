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