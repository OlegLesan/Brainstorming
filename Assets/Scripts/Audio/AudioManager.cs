using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

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

    public void PlaySFXFromIndex(int index, AudioSource mainAudioSource)
    {
        if (index >= 0 && index < sfx.Length)
        {
            AudioSource sfxSource = sfx[index];
            if (sfxSource != null && !isSFXMuted)
            {
                sfxSource.volume = sfxVolume * mainAudioSource.volume;
                sfxSource.pitch = mainAudioSource.pitch;
                sfxSource.loop = mainAudioSource.loop;

                sfxSource.Play();
            }
            else
            {
                Debug.LogWarning("AudioSource не найден или отключён для данного SFX.");
            }
        }
        else
        {
            Debug.LogWarning($"Индекс {index} вне диапазона массива sfx.");
        }
    }

    public void ConfigureSFX(int index, AudioSource targetAudioSource)
    {
        if (index >= 0 && index < sfx.Length)
        {
            AudioSource pooledSource = sfx[index];
            if (pooledSource != null)
            {
                // Настраиваем параметры звука
                targetAudioSource.clip = pooledSource.clip;
                targetAudioSource.volume = sfxVolume;
                targetAudioSource.pitch = pooledSource.pitch;
                targetAudioSource.loop = pooledSource.loop;
            }
            else
            {
                Debug.LogWarning("AudioSource из пула отсутствует для данного индекса.");
            }
        }
        else
        {
            Debug.LogWarning($"Индекс {index} вне диапазона массива sfx.");
        }
    }

    public void StopSFX(AudioSource source)
    {
        // Останавливаем звук, связанный с конкретным AudioSource
        if (source != null && source.isPlaying)
        {
            source.Stop();
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

        StartCoroutine(SetupSoundMenuButtonDelayed());
    }

    private IEnumerator SetupSoundMenuButtonDelayed()
    {
        yield return null;
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
            int randomIndex = UnityEngine.Random.Range(0, bgm.Length);
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
        Button[] allButtons = FindObjectsOfType<Button>(true);
        foreach (var button in allButtons)
        {
            if (button.CompareTag("SoundMenuButton"))
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
