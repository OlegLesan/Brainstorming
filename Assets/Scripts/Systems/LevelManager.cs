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

    public GameObject levelFailScreen;
    public GameObject levelCompleteScreen;

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
        if (theBase != null && theBase.currentHealth > 0)
        {
            levelActive = false;
            levelVictory = true;
            Debug.Log("Level completed!");
            ShowEndScreen();
        }
        else
        {
            Debug.LogWarning("Base health is 0, cannot complete level.");
        }
    }

    public void CheckForLevelCompletion()
    {
        // Проверяем завершение уровня.
        bool allEnemiesDefeated = activeEnemies.Count == 0 && WaveManager.instance.totalEnemiesRemaining <= 0;
        bool wavesCompleted = WaveManager.instance.AllWavesCompleted();
        bool baseIsAlive = theBase.currentHealth > 0;

        Debug.Log($"[CheckForLevelCompletion] WavesCompleted={wavesCompleted}, AllEnemiesDefeated={allEnemiesDefeated}, BaseHealth={baseIsAlive}");

        if (allEnemiesDefeated && wavesCompleted && baseIsAlive)
        {
            LevelComplete();
        }
    }

    public void RemoveEnemyFromActiveList(EnemyHealthController enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
            
        }

        // Проверяем завершение уровня
        CheckForLevelCompletion();
    }

    private void ShowEndScreen()
    {
        if (levelVictory)
        {
            
        }
        else
        {
           
        }

        levelFailScreen.SetActive(!levelVictory);
        levelCompleteScreen.SetActive(levelVictory);

        
    }
}
