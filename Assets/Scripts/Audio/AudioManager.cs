using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    public AudioSource menuMusic; // ������ ��� �������� ����
    public AudioSource levelSelectMusic; // ������ ��� ������ ������
    public AudioSource[] bgm; // ������ ������ ��� ������� ������
    public AudioSource[] sfx; // �������� �������

    private AudioSource currentBGM; // ������� ���� ������� ������
    private Coroutine bgmCoroutine; // ������ �� �������� ������� ������

    [Header("Audio Settings")]
    [Range(0f, 1f)] public float musicVolume = 1f; // ��������� ������
    [Range(0f, 1f)] public float sfxVolume = 1f; // ��������� �������� ��������
    public bool isMusicMuted = false; // ��������� �� ������
    public bool isSFXMuted = false; // ��������� �� �������� �������

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ApplyAudioSettings();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StopAllMusic();

        if (scene.name == "MainMenu")
        {
            PlayMenuMusic();
        }
        else if (scene.name == "LevelSelect")
        {
            PlayLevelSelectMusic();
        }
        else
        {
            PlayRandomBGM();
        }
    }

    // ��������� ���� ������
    public void StopAllMusic()
    {
        if (menuMusic != null && menuMusic.isPlaying) menuMusic.Stop();
        if (levelSelectMusic != null && levelSelectMusic.isPlaying) levelSelectMusic.Stop();

        foreach (var bgmSource in bgm)
        {
            if (bgmSource != null && bgmSource.isPlaying) bgmSource.Stop();
        }

        if (bgmCoroutine != null)
        {
            StopCoroutine(bgmCoroutine);
            bgmCoroutine = null;
        }
    }

    public void PlayMenuMusic()
    {
        StopAllMusic();
        if (menuMusic != null)
        {
            menuMusic.volume = isMusicMuted ? 0f : musicVolume;
            menuMusic.Play();
        }
    }

    public void PlayLevelSelectMusic()
    {
        StopAllMusic();
        if (levelSelectMusic != null)
        {
            levelSelectMusic.volume = isMusicMuted ? 0f : musicVolume;
            levelSelectMusic.Play();
        }
    }

    public void PlayRandomBGM()
    {
        StopAllMusic();
        if (bgm.Length > 0)
        {
            int randomIndex = Random.Range(0, bgm.Length);
            currentBGM = bgm[randomIndex];
            currentBGM.volume = isMusicMuted ? 0f : musicVolume;
            currentBGM.Play();
            bgmCoroutine = StartCoroutine(PlayNextBGMWhenFinished());
        }
    }

    private IEnumerator PlayNextBGMWhenFinished()
    {
        while (currentBGM != null && currentBGM.isPlaying)
        {
            yield return null;
        }

        PlayRandomBGM();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        ApplyAudioSettings();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        ApplyAudioSettings();
    }

    public void ToggleMusicMute(bool mute)
    {
        isMusicMuted = mute;
        ApplyAudioSettings();
    }

    public void ToggleSFXMute(bool mute)
    {
        isSFXMuted = mute;
        ApplyAudioSettings();
    }

    private void ApplyAudioSettings()
    {
        if (menuMusic != null)
            menuMusic.volume = isMusicMuted ? 0f : musicVolume;

        if (levelSelectMusic != null)
            levelSelectMusic.volume = isMusicMuted ? 0f : musicVolume;

        foreach (var bgmSource in bgm)
        {
            if (bgmSource != null)
                bgmSource.volume = isMusicMuted ? 0f : musicVolume;
        }

        foreach (var sfxSource in sfx)
        {
            if (sfxSource != null)
                sfxSource.volume = isSFXMuted ? 0f : sfxVolume;
        }
    }
}
