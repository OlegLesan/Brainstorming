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
        Debug.Log($"Checking level completion: WavesCompleted={WaveManager.instance.AllWavesCompleted()}, ActiveEnemies={activeEnemies.Count}, BaseHealth={theBase.currentHealth}");

        // Проверяем условия завершения уровня
        if (WaveManager.instance.AllWavesCompleted() &&
            GameObject.FindGameObjectsWithTag("Enemy").Length == 0 &&
            theBase.currentHealth > 0)
        {
            Debug.Log("Conditions for level completion met.");
            LevelComplete();
        }
        else
        {
            Debug.LogWarning("Conditions not met for level completion.");
        }
    }

    private void ShowEndScreen()
    {
        if (levelVictory)
        {
            Debug.Log("Showing Level Complete screen.");
        }
        else
        {
            Debug.Log("Showing Level Failed screen.");
        }

        levelFailScreen.SetActive(!levelVictory);
        levelCompleteScreen.SetActive(levelVictory);

        Debug.Log(levelVictory ? "Victory!" : "Defeat!");
    }

    public void RemoveEnemyFromActiveList(EnemyHealthController enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
            Debug.Log($"Enemy removed. Remaining enemies: {activeEnemies.Count}");
            CheckForLevelCompletion();
        }
    }
}
