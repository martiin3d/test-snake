#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.IO;

public class BuildAssetBundles
{
    [MenuItem("Tools/Martin Luquet/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string path = Path.Combine(Application.dataPath, "../AssetBundles");

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        BuildPipeline.BuildAssetBundles(path,
            BuildAssetBundleOptions.None,
#if UNITY_IOS
            BuildTarget.iOS);
#elif UNITY_ANDROID
            BuildTarget.Android);
#else
            BuildTarget.StandaloneWindows);
#endif
        Debug.Log("AssetBundles saved in: " + path);
    }
}
#endif