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
    private Base theBase;

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
        // Проверяем, что все враги действительно уничтожены
        if (activeEnemies.Count == 0 && WaveManager.instance.totalEnemiesRemaining <= 0)
        {
            levelActive = false;
            levelVictory = true;
            ShowEndScreen();
        }
    }

    private void ShowEndScreen()
    {
        levelFailScreen.SetActive(!levelVictory);       // Показываем экран поражения, если нет победы
        levelCompleteScreen.SetActive(levelVictory);    // Показываем экран победы, если победа

        Debug.Log(levelVictory ? "Победа!" : "Поражение!");
    }
}
