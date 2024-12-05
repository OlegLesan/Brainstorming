using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;



public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private Dictionary<GameObject, bool> uiElementStates = new Dictionary<GameObject, bool>();
    

    [Header("Audio Sources")]
    public AudioSource menuMusic;
    public AudioSource levelSelectMusic;
    public AudioSource[] bgm;
    public AudioSource[] sfx;

    private AudioSource currentBGM;
    private Coroutine bgmCoroutine;

    [Header("Audio Settings")]
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    public bool isMusicMuted = false;
    public bool isSFXMuted = false;

    [Header("UI References")]
    public Canvas soundMenuCanvas;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

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
        
        SetupSoundMenuButton();

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = musicVolume;
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.value = sfxVolume;
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        }

        ApplyAudioSettings();
    }

   

    public void PlaySFX(int index)
    {
        if (index >= 0 && index < sfx.Length)
        {
            AudioSource audioSource = sfx[index];
            if (!isSFXMuted && audioSource != null)
            {
                audioSource.volume = sfxVolume;
                audioSource.PlayOneShot(audioSource.clip);
            }
        }
        else
        {
            Debug.LogWarning($"Индекс {index} находится вне диапазона массива sfx.");
        }
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

        // Убедитесь, что кнопка настроена после загрузки сцены
        StartCoroutine(SetupSoundMenuButtonDelayed());
    }

    // Добавьте задержку для поиска кнопки
    private IEnumerator SetupSoundMenuButtonDelayed()
    {
        yield return null; // Дождаться завершения загрузки сцены
        SetupSoundMenuButton();
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
            int randomIndex = UnityEngine.Random.Range(0, bgm.Length); // Явное указание UnityEngine.Random
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

    private void SetupSoundMenuButton()
    {
        Button[] allButtons = FindObjectsOfType<Button>(true); // Ищем все кнопки, включая отключённые
        foreach (var button in allButtons)
        {
            if (button.CompareTag("SoundMenuButton")) // Проверяем тэг
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(ShowSoundMenu);
                Debug.Log("SoundMenuButton настроена.");
                return;
            }
        }

        Debug.LogWarning("Кнопка с тэгом SoundMenuButton не найдена (даже если отключена).");
    }


    public void ShowSoundMenu()
    {
        if (soundMenuCanvas != null)
        {
            soundMenuCanvas.gameObject.SetActive(true);
        }

        Canvas[] allCanvases = FindObjectsOfType<Canvas>();
        foreach (var canvas in allCanvases)
        {
            if (canvas.gameObject != soundMenuCanvas.gameObject && !uiElementStates.ContainsKey(canvas.gameObject))
            {
                uiElementStates[canvas.gameObject] = canvas.gameObject.activeSelf;
                canvas.gameObject.SetActive(false);
            }
        }

        Time.timeScale = 0;
    }

    public void HideSoundMenu()
    {
        if (soundMenuCanvas != null)
        {
            soundMenuCanvas.gameObject.SetActive(false);
        }

        foreach (var entry in uiElementStates)
        {
            if (entry.Key != null)
            {
                entry.Key.SetActive(entry.Value);
            }
        }

        uiElementStates.Clear();
        Time.timeScale = 1;
    }
}
