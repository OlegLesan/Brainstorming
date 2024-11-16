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

    public GameObject levelFailScreen;     // ����� ���������
    public GameObject levelCompleteScreen; // ����� ������

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
        // ���������, ��� �������� ���� ������ 0
        if (theBase != null && theBase.currentHealth > 0)
        {
            levelActive = false;
            levelVictory = true;
            ShowEndScreen();
        }
    }

    public void CheckForLevelCompletion()
    {
        // ������� ��������, ����:
        // 1. ��� �������� ������
        // 2. ��� ����� ���������
        // 3. �������� ���� ������ 0
        if (activeEnemies.Count == 0 && WaveManager.instance.totalEnemiesRemaining == 0 &&
            WaveManager.instance.AllWavesCompleted() && theBase.currentHealth > 0)
        {
            LevelComplete();
        }
    }

    private void ShowEndScreen()
    {
        levelFailScreen.SetActive(!levelVictory);       // ���������� ����� ���������, ���� ��� ������
        levelCompleteScreen.SetActive(levelVictory);    // ���������� ����� ������, ���� ������

        Debug.Log(levelVictory ? "������!" : "���������!");
    }
}
