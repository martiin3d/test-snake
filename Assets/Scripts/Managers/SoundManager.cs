using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    [SerializeField] public int PoolSize { get; private set; } = 10;

    public Queue<AudioSource> AudioSourcePool { get; private set; }

    private Dictionary<Sound, AudioClip> soundAudioClipDictionary;

    public enum Sound
    {
        SnakeMove,
        SnakeDie,
        SnakeEat,
        ButtonClick,
        ButtonOver,
    }

    public override void Awake()
    {
        base.Awake();
        InitializeSoundDictionary();
        InitializeAudioSourcePool();
    }

    private void InitializeSoundDictionary()
    {
        soundAudioClipDictionary = new Dictionary<Sound, AudioClip>();
        foreach (SoundAudioClip soundAudioClip in GameAssets.Instance.soundAudioClipArray)
        {
            soundAudioClipDictionary[soundAudioClip.sound] = soundAudioClip.audioClip;
        }
    }

    private void InitializeAudioSourcePool()
    {
        AudioSourcePool = new Queue<AudioSource>();
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject soundGameObject = new GameObject("AudioSourcePoolObject");
            soundGameObject.transform.parent = transform;
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            AudioSourcePool.Enqueue(audioSource);
            audioSource.gameObject.SetActive(false);
        }
    }

    public void PlaySound(Sound sound)
    {
        if (soundAudioClipDictionary.TryGetValue(sound, out AudioClip clip))
        {
            AudioSource audioSource = GetAudioSourceFromPool();
            audioSource.clip = clip;
            audioSource.Play();
            StartCoroutine(ReturnAudioSourceToPoolAfterPlay(audioSource, clip.length));
        }
        else
        {
            Debug.LogError($"Sound {sound} not found in dictionary!");
        }
    }

    private IEnumerator ReturnAudioSourceToPoolAfterPlay(AudioSource audioSource, float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        audioSource.Stop();
        audioSource.gameObject.SetActive(false);
        AudioSourcePool.Enqueue(audioSource);
    }

    private AudioSource GetAudioSourceFromPool()
    {
        if (AudioSourcePool.Count > 0)
        {
            AudioSource audioSource = AudioSourcePool.Dequeue();
            audioSource.gameObject.SetActive(true);
            return audioSource;
        }
        else
        {
            // If the pool is empty, create a new one (dynamic growth)
            Debug.LogWarning("AudioSource pool exhausted. Creating a new AudioSource.");
            GameObject newSoundGameObject = new GameObject("AudioSourcePoolObject_Dynamic");
            newSoundGameObject.transform.parent = transform;
            AudioSource newAudioSource = newSoundGameObject.AddComponent<AudioSource>();
            return newAudioSource;
        }
    }
}