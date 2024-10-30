using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // ��� ���������� �������

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource menuMusic;
    public AudioSource levelSelectMusic;
    public AudioSource[] bgm; // ������ � ������� ������� ��� BGM

    private AudioSource currentBGM; // ���������� ��� �������� BGM �����

    private void Awake()
    {
        // �������� �� ������������� ������������� ���������� AudioManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �� ���������� ������ ��� �������� ����� �����
        }
        else
        {
            Destroy(gameObject); // ���������� ���������
        }
    }

    private void OnEnable()
    {
        // ������������� �� ������� �������� �����
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // ������������ �� ������� ��� ���������� �������
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // ����� ���������� ��� ������ �������� ����� �����
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ������������� ��� ������ ����� ���������� �����
        StopAllMusic();

        // ��������� ��� ����������� ����� � ��������� ��������������� ������
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
            PlayRandomBGM(); // ��������� ��������� BGM ���� ��� ���� ������ ����
        }
    }

    // ��������� ���� ������
    public void StopAllMusic()
    {
        // ������������� ������� ������ ���� � ��������� �������
        if (menuMusic.isPlaying)
        {
            menuMusic.Stop();
        }

        if (levelSelectMusic.isPlaying)
        {
            levelSelectMusic.Stop();
        }

        // ������������� ��� ������� �����, ���� ��� ������
        foreach (AudioSource bgmSource in bgm)
        {
            if (bgmSource.isPlaying)
            {
                bgmSource.Stop();
            }
        }
    }

    // ��������������� ������ �������� ����
    public void PlayMenuMusic()
    {
        StopAllMusic(); // ��������, ��� ��� ��������� �����������
        menuMusic.Play(); // ��������� ������ ������ ����
    }

    // ��������������� ������ ��������� ������
    public void PlayLevelSelectMusic()
    {
        StopAllMusic(); // ��������, ��� ��� ��������� �����������
        levelSelectMusic.Play(); // ��������� ������ ������ ��������� �������
    }

    // ��������������� ���������� BGM �����
    public void PlayRandomBGM()
    {
        StopAllMusic(); // ��������� ��� ������ �����

        // �������� ��������� ���� �� ������� bgm
        int randomIndex = Random.Range(0, bgm.Length);
        currentBGM = bgm[randomIndex];
        currentBGM.Play();

        // ��������� �������� ��� ������������ ��������� ����� � ������� ����������
        StartCoroutine(PlayNextBGMWhenFinished());
    }

    // �������� ��� ������������ ��������������� ��������� ������
    private IEnumerator PlayNextBGMWhenFinished()
    {
        // ����, ���� ������� ���� �� ����������
        while (currentBGM.isPlaying)
        {
            yield return null;
        }

        // ����� ���� ����������, ��������� ��������� ��������� ����
        PlayRandomBGM();
    }
}
