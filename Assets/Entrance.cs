using AssetBundles;
using System.Collections;
using UnityEngine;

public class Entrance : MonoBehaviour
{
    private static bool DidEnter = false;

    public IEnumerator Start()
    {
        if (!DidEnter)
        {
            DidEnter = true;
#if UNITY_EDITOR
            AssetBundleManager.BaseDownloadingURL = "file://" + Application.streamingAssetsPath + "/";
#else
            AssetBundleManager.BaseDownloadingURL = Application.streamingAssetsPath + "/";
#endif
            yield return AssetBundleManager.Initialize();
        }
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}