using UnityEngine;

[CreateAssetMenu(fileName = "GameAssets", menuName = "Martin Luquet/GameAssets", order = 1)]
public class GameAssets : ScriptableObject
{
    private static GameAssets _instance;
    public static GameAssets Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<GameAssets>("GameAssets");
                if (_instance == null)
                {
                    Debug.LogError("GameAssets ScriptableObject not found in Resources folder");
                }
            }
            return _instance;
        }
    }
    public static void SetInstanceForTesting(GameAssets instance)
    {
        _instance = instance;
    }

    public Snake snakePrefab;
    public SnakeBodyPartData SnakeBodyPartData;
    public SnakeData SnakeData;
    public Sprite foodSprite;
    public SoundAudioClip[] soundAudioClipArray;
}
