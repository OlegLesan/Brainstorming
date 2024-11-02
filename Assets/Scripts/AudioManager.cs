using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource menuMusic;
    public AudioSource levelSelectMusic;
    public AudioSource[] bgm; // ������ � ������� ������� ��� BGM

    private AudioSource currentBGM; // ���������� ��� �������� BGM �����
    private Coroutine bgmCoroutine; // ������ �� �������� BGM

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

    // ��������� ���� ������ � �������� BGM
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

        // ������������� �������� BGM, ���� ��� ��������
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
