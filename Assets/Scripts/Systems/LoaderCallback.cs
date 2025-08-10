/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.IO;
using UnityEngine;

/*
 * Works alongside the Loader class to notify it when the current level has updated the screen
 * */
public class LoaderCallback : MonoBehaviour
{
    [SerializeField] private SnakeBodyPartData _defaultSkinData;
    [SerializeField] private bool _loadAssetBundle = false;
    [SerializeField] private bool _loadLocalAssetBundle = true;
    [SerializeField] private string _remoteAssetBundlePath;
    [SerializeField] private string _assetBundleName = "halloweenbundle";
    [SerializeField] private string _bodyPartName = "SnakeBodyPart_Halloween";

    private AssetBundleLoader _assetBundle;
    private static bool _isAssetBundleLoaded = false;

    private void Awake()
    {
        if (!_isAssetBundleLoaded && _loadAssetBundle)
        {
            LoadAssetBundle();
        }
        else
        {
            GameAssets.Instance.SnakeBodyPartData = _defaultSkinData;
            Loader.LoaderCallback();
        }
    }

    private void LoadAssetBundle()
    {
        _assetBundle = new AssetBundleLoader();
        _assetBundle.OnBundleLoadComplete += OnAssetBundleLoaded;
        if (_loadLocalAssetBundle)
        {
            string folderPath = Path.Combine(Application.dataPath, "../AssetBundles");
            string fullPath = Path.Combine(folderPath, _assetBundleName);

            StartCoroutine(_assetBundle.LoadBundleCoroutine(fullPath, _bodyPartName));
        }
        else
        {
            StartCoroutine(_assetBundle.LoadBundleCoroutine(_remoteAssetBundlePath, _bodyPartName));
        }
        _isAssetBundleLoaded = true;
    }

    private void OnAssetBundleLoaded()
    {
        _assetBundle.OnBundleLoadComplete -= OnAssetBundleLoaded;
        Loader.LoaderCallback();
    }
}
