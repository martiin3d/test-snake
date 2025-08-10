/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        GameScene,
        Loading,
        MainMenu,
    }

    private static Scene _sceneToLoad;

    public static void Load(Scene scene)
    {
        _sceneToLoad = scene;
        SceneManager.LoadScene(Scene.Loading.ToString());
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(_sceneToLoad.ToString());
    }
}