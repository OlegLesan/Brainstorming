using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    private void Awake()
    {
        instance = this;
    }

    public bool levelActive = true;
    private bool levelVictory;
    public Base theBase;

    public List<EnemyHealthController> activeEnemies = new List<EnemyHealthController>();
    public string nextLevel;

    public GameObject levelFailScreen;     // Экран поражения
    public GameObject levelCompleteScreen; // Экран победы

    void Start()
    {
        theBase = FindObjectOfType<Base>();
    }

    public void LevelFailed()
    {
        levelActive = false;
        levelVictory = false;
        ShowEndScreen();
    }

    public void LevelComplete()
    {
        // Проверяем, что здоровье базы больше 0
        if (theBase != null && theBase.currentHealth > 0)
        {
            levelActive = false;
            levelVictory = true;
            ShowEndScreen();
        }
    }

    public void CheckForLevelCompletion()
    {
        // Уровень завершен, если:
        // 1. Нет активных врагов
        // 2. Все волны завершены
        // 3. Здоровье базы больше 0
        if (activeEnemies.Count == 0 && WaveManager.instance.totalEnemiesRemaining == 0 &&
            WaveManager.instance.AllWavesCompleted() && theBase.currentHealth > 0)
        {
            LevelComplete();
        }
    }

    private void ShowEndScreen()
    {
        levelFailScreen.SetActive(!levelVictory);       // Показываем экран поражения, если нет победы
        levelCompleteScreen.SetActive(levelVictory);    // Показываем экран победы, если победа

        Debug.Log(levelVictory ? "Победа!" : "Поражение!");
    }
}
