using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Для управления сценами

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource menuMusic;
    public AudioSource levelSelectMusic;
    public AudioSource[] bgm; // Массив с разными треками для BGM

    private AudioSource currentBGM; // Переменная для текущего BGM трека

    private void Awake()
    {
        // Проверка на существование единственного экземпляра AudioManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Не уничтожать объект при загрузке новой сцены
        }
        else
        {
            Destroy(gameObject); // Уничтожаем дубликаты
        }
    }

    private void OnEnable()
    {
        // Подписываемся на событие загрузки сцены
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Отписываемся от события при отключении объекта
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Метод вызывается при каждой загрузке новой сцены
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Останавливаем всю музыку перед включением новой
        StopAllMusic();

        // Проверяем имя загруженной сцены и запускаем соответствующую музыку
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
            PlayRandomBGM(); // Запускаем случайный BGM трек для всех других сцен
        }
    }

    // Остановка всей музыки
    public void StopAllMusic()
    {
        // Останавливаем главную музыку меню и селектора уровней
        if (menuMusic.isPlaying)
        {
            menuMusic.Stop();
        }

        if (levelSelectMusic.isPlaying)
        {
            levelSelectMusic.Stop();
        }

        // Останавливаем все фоновые треки, если они играют
        foreach (AudioSource bgmSource in bgm)
        {
            if (bgmSource.isPlaying)
            {
                bgmSource.Stop();
            }
        }
    }

    // Воспроизведение музыки главного меню
    public void PlayMenuMusic()
    {
        StopAllMusic(); // Убедимся, что все остальное остановлено
        menuMusic.Play(); // Запускаем только музыку меню
    }

    // Воспроизведение музыки селектора уровня
    public void PlayLevelSelectMusic()
    {
        StopAllMusic(); // Убедимся, что все остальное остановлено
        levelSelectMusic.Play(); // Запускаем только музыку селектора уровней
    }

    // Воспроизведение случайного BGM трека
    public void PlayRandomBGM()
    {
        StopAllMusic(); // Остановим все другие треки

        // Выбираем случайный трек из массива bgm
        int randomIndex = Random.Range(0, bgm.Length);
        currentBGM = bgm[randomIndex];
        currentBGM.Play();

        // Запускаем корутину для отслеживания окончания трека и запуска следующего
        StartCoroutine(PlayNextBGMWhenFinished());
    }

    // Корутина для бесконечного воспроизведения случайных треков
    private IEnumerator PlayNextBGMWhenFinished()
    {
        // Ждем, пока текущий трек не закончится
        while (currentBGM.isPlaying)
        {
            yield return null;
        }

        // Когда трек закончился, запускаем следующий случайный трек
        PlayRandomBGM();
    }
}
