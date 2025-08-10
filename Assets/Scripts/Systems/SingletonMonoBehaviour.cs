using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    private static T _instance;

    public virtual void Awake()
    {
        if (_instance == this)
            return;
        else if (_instance != null)
            Debug.LogError($"Singleton {typeof(T).ToString()} already exists");

        _instance = this as T;

        if (Application.isPlaying)
        {
            DontDestroyOnLoad(this);
        }
    }

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject(typeof(T).ToString());
                _instance = obj.AddComponent<T>();
                obj.name = _instance.GetType().Name;
            }

            return _instance;
        }
    }

    public virtual void OnDestroy()
    {
        _instance = null;
    }
}