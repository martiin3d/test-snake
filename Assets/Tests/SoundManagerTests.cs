#if UNITY_EDITOR
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using System.Reflection;

public class MockGameAssets : GameAssets
{
    public static MockGameAssets CreateInstance()
    {
        var instance = ScriptableObject.CreateInstance<MockGameAssets>();
        typeof(GameAssets).GetField("_instance", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, instance);
        return instance;
    }
}

public static class MockAudioClip
{
    public static AudioClip CreateMockAudioClip(string name, float length)
    {
        int samples = (int)(length * 44100);
        return AudioClip.Create(name, samples, 1, 44100, false);
    }
}

public class SoundManagerTests
{
    private SoundManager soundManager;
    private MockGameAssets mockGameAssets;

    [SetUp]
    public void SetUp()
    {
        mockGameAssets = MockGameAssets.CreateInstance();
        mockGameAssets.soundAudioClipArray = new SoundAudioClip[0];

        GameObject soundManagerGO = new GameObject("SoundManager");
        soundManager = soundManagerGO.AddComponent<SoundManager>();

        soundManager.Awake();
    }

    [TearDown]
    public void TearDown()
    {
        if (soundManager != null)
        {
            Object.DestroyImmediate(soundManager.gameObject);
        }
        if (mockGameAssets != null)
        {
            Object.DestroyImmediate(mockGameAssets);
        }
    }

    [Test]
    public void Awake_InitializesAudioSourcePoolToCorrectSize()
    {
        Assert.AreEqual(soundManager.PoolSize, soundManager.AudioSourcePool.Count);
    }

    [UnityTest]
    public IEnumerator PlaySound_ShouldGetAudioSourceFromPool()
    {
        mockGameAssets.soundAudioClipArray = new SoundAudioClip[]
        {
        new SoundAudioClip { sound = SoundManager.Sound.ButtonClick, audioClip = MockAudioClip.CreateMockAudioClip("TestClip", 1.0f) }
        };
        soundManager.Awake();

        int initialPoolCount = soundManager.AudioSourcePool.Count;

        soundManager.PlaySound(SoundManager.Sound.ButtonClick);

        Assert.AreEqual(initialPoolCount - 1, soundManager.AudioSourcePool.Count);

        yield return null;
    }

    [UnityTest]
    public IEnumerator PlaySound_WhenSoundIsNotFound_ShouldLogAnError()
    {
        mockGameAssets.soundAudioClipArray = new SoundAudioClip[0];
        soundManager.Awake();

        LogAssert.Expect(LogType.Error, "Sound ButtonClick not found in dictionary!");

        soundManager.PlaySound(SoundManager.Sound.ButtonClick);

        yield return null;
    }
}
#endif