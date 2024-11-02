using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource menuMusic;
    public AudioSource levelSelectMusic;
    public AudioSource[] bgm; // Массив с разными треками для BGM

    private AudioSource currentBGM; // Переменная для текущего BGM трека
    private Coroutine bgmCoroutine; // Ссылка на корутину BGM

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

    public AudioSource[] sfx;

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

    // Остановка всей музыки и корутины BGM
    public void StopAllMusic()
    {
        if (menuMusic.isPlaying)
        {
            menuMusic.Stop();
        }

        if (levelSelectMusic.isPlaying)
        {
            levelSelectMusic.Stop();
        }

        foreach (AudioSource bgmSource in bgm)
        {
            if (bgmSource.isPlaying)
            {
                bgmSource.Stop();
            }
        }

        // Останавливаем корутину BGM, если она запущена
        if (bgmCoroutine != null)
        {
            StopCoroutine(bgmCoroutine);
            bgmCoroutine = null;
        }
    }

    public void PlayMenuMusic()
    {
        StopAllMusic();
        menuMusic.Play();
    }

    public void PlayLevelSelectMusic()
    {
        StopAllMusic();
        levelSelectMusic.Play();
    }

    public void PlayRandomBGM()
    {
        StopAllMusic();

        int randomIndex = Random.Range(0, bgm.Length);
        currentBGM = bgm[randomIndex];
        currentBGM.Play();

        bgmCoroutine = StartCoroutine(PlayNextBGMWhenFinished());
    }

    private IEnumerator PlayNextBGMWhenFinished()
    {
        while (currentBGM.isPlaying)
        {
            yield return null;
        }

        PlayRandomBGM();
    }
}
