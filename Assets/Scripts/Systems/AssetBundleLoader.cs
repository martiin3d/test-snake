using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public class AssetBundleLoader
{
    public Action OnBundleLoadComplete;

    private bool LoadBundle(AssetBundle bundle, string bodyPartName)
    {
        if (bundle == null)
        {
            Debug.LogError("Can't load asset bundle");
            return false;
        }

        SnakeBodyPartData skinData = bundle.LoadAsset<SnakeBodyPartData>(bodyPartName);
        if (skinData != null)
        {
            GameAssets.Instance.SnakeBodyPartData = skinData;
            Debug.Log("Updated skin: " + skinData.name);
        }

        bundle.Unload(false);

        return true;
    }

    public IEnumerator LoadBundleCoroutine(string url, string bodyPartName)
    {
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error while downloading AssetBundle: " + www.error);
            yield break;
        }

        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);

        LoadBundle(bundle, bodyPartName);
        OnBundleLoadComplete?.Invoke();
    }
}