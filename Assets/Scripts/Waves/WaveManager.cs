using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    [System.Serializable]
    public class Wave
    {
        public string[] enemyNames;  // ����� ������ �� ����
        public int totalEnemyCount;  // ����� ���������� ������ � �����
        public float spawnInterval;  // �������� ������ ������
    }

    public Wave[] waves;
    public Transform spawnPoint;
    public Button startWaveButton;
    public TMP_Text waveText;
    public TMP_Text waveCounterText;
    public float timeBetweenWaves = 2f;
    public float waveDuration = 10f;
    public int rewardForEarlyStart = 50;
    public float fillAmountDecreaseRate = 1f;

    private int currentWaveIndex = 0;
    private bool waveInProgress = false;
    private bool firstWaveStarted = false;
    public int totalEnemiesRemaining = 0;

    private Image buttonImage;
    private EnemyPool enemyPool;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        enemyPool = FindObjectOfType<EnemyPool>();  // �������� ������ �� ��� ������
    }

    private void Start()
    {
        // ������������ ����� ���������� ������ � ������
        foreach (Wave wave in waves)
        {
            totalEnemiesRemaining += wave.totalEnemyCount;
        }

        buttonImage = startWaveButton.GetComponent<Image>();
        startWaveButton.onClick.AddListener(OnWaveButtonPressed);
        startWaveButton.gameObject.SetActive(true);
        UpdateWaveText();
    }

    private void UpdateWaveText()
    {
        waveText.text = "Wave: " + (currentWaveIndex + 1);
        waveCounterText.text = $"Wave {currentWaveIndex + 1} / {waves.Length}";
    }

    public void DecreaseEnemyCount()
    {
        if (totalEnemiesRemaining > 0)
        {
            totalEnemiesRemaining--;
        }

        LevelManager.instance.CheckForLevelCompletion(); // ��������� ���������� ������
    }

    public bool AllWavesCompleted()
    {
        return currentWaveIndex >= waves.Length;
    }

    public void StartWave()
    {
        if (!waveInProgress && currentWaveIndex < waves.Length)
        {
            waveInProgress = true;
            startWaveButton.gameObject.SetActive(false);
            UpdateWaveText();

            StartCoroutine(SpawnWave(waves[currentWaveIndex]));
        }
    }

    private void OnWaveButtonPressed()
    {
        if (firstWaveStarted)
        {
            MoneyManager.instance.GiveMoney(rewardForEarlyStart);
        }

        StartWave();
    }

    IEnumerator SpawnWave(Wave wave)
    {
        // ���������� ������, ������� ����� ���������� � ���� �����
        int enemiesToSpawn = wave.totalEnemyCount;

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            // �������� ��������� ��� ����� �� ������� `enemyNames`
            string enemyName = wave.enemyNames[Random.Range(0, wave.enemyNames.Length)];

            GameObject enemy = enemyPool.GetEnemy(enemyName);
            if (enemy != null)
            {
                enemy.transform.position = spawnPoint.position;
                enemy.transform.rotation = spawnPoint.rotation;

                yield return new WaitForSeconds(wave.spawnInterval);
            }
            else
            {
                Debug.LogWarning($"�� ������� �������� ����� �� ����: {enemyName}");
            }
        }

        yield return new WaitForSeconds(waveDuration);
        WaveCompleted();
    }

    private void WaveCompleted()
    {
        waveInProgress = false;
        currentWaveIndex++;

        if (currentWaveIndex < waves.Length)
        {
            firstWaveStarted = true;
            startWaveButton.gameObject.SetActive(true);
            UpdateWaveText();
            StartCoroutine(WaveTimer());
        }
        else
        {
            LevelManager.instance.CheckForLevelCompletion(); // ��������� ������� ����� ���������� ���� ����
        }
    }

    IEnumerator WaveTimer()
    {
        buttonImage.fillAmount = 1f;

        while (buttonImage.fillAmount > 0)
        {
            buttonImage.fillAmount -= Time.deltaTime / fillAmountDecreaseRate;
            yield return null;
        }

        if (!waveInProgress)
        {
            StartWave();
        }
    }
}
