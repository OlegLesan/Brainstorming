using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    public AudioSource menuMusic; // Музыка для главного меню
    public AudioSource levelSelectMusic; // Музыка для выбора уровня
    public AudioSource[] bgm; // Массив треков для фоновой музыки
    public AudioSource[] sfx; // Звуковые эффекты

    private AudioSource currentBGM; // Текущий трек фоновой музыки
    private Coroutine bgmCoroutine; // Ссылка на корутину фоновой музыки

    [Header("Audio Settings")]
    [Range(0f, 1f)] public float musicVolume = 1f; // Громкость музыки
    [Range(0f, 1f)] public float sfxVolume = 1f; // Громкость звуковых эффектов
    public bool isMusicMuted = false; // Отключена ли музыка
    public bool isSFXMuted = false; // Отключены ли звуковые эффекты

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

    // Остановка всей музыки
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
