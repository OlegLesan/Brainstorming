using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    [System.Serializable]
    public class SpawnPointWithPath
    {
        public Transform spawnPoint; // Точка спавна
        public Path path;            // Путь, связанный с точкой спавна
    }

    [System.Serializable]
    public class EnemyTypeInWave
    {
        public string enemyName;   // Имя врага
        public int enemyCount;     // Количество врагов этого типа
    }

    [System.Serializable]
    public class Wave
    {
        public EnemyTypeInWave[] enemyTypes;  // Массив типов врагов и их количества
        public float spawnInterval;           // Интервал спауна врагов
    }

    public Wave[] waves;
    public SpawnPointWithPath[] spawnPointsWithPaths;
    public Button startWaveButton;
    public TMP_Text waveText;
    public TMP_Text waveCounterText;
    
    
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

        enemyPool = FindObjectOfType<EnemyPool>();
    }

    private void Start()
    {
        foreach (Wave wave in waves)
        {
            foreach (var enemyType in wave.enemyTypes)
            {
                totalEnemiesRemaining += enemyType.enemyCount;
            }
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

        // Немедленно проверяем завершение уровня
        LevelManager.instance.CheckForLevelCompletion();
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
        if (wave == null)
        {
            
            yield break;
        }

        List<EnemyTypeInWave> enemyTypes = new List<EnemyTypeInWave>(wave.enemyTypes);

        while (enemyTypes.Count > 0)
        {
            int randomIndex = Random.Range(0, enemyTypes.Count);
            EnemyTypeInWave selectedEnemyType = enemyTypes[randomIndex];

            GameObject enemy = enemyPool.GetEnemy(selectedEnemyType.enemyName);
            if (enemy != null)
            {
                int randomSpawnIndex = Random.Range(0, spawnPointsWithPaths.Length);
                SpawnPointWithPath selectedSpawnPointWithPath = spawnPointsWithPaths[randomSpawnIndex];

                enemy.transform.position = selectedSpawnPointWithPath.spawnPoint.position;
                enemy.transform.rotation = selectedSpawnPointWithPath.spawnPoint.rotation;

                EnemyControler enemyController = enemy.GetComponent<EnemyControler>();
                if (enemyController != null)
                {
                    enemyController.Setup(selectedSpawnPointWithPath.path);
                }

                yield return new WaitForSeconds(wave.spawnInterval);
            }
            else
            {
                Debug.LogWarning($"Не удалось получить врага из пула: {selectedEnemyType.enemyName}");
            }

            selectedEnemyType.enemyCount--;

            if (selectedEnemyType.enemyCount <= 0)
            {
                enemyTypes.RemoveAt(randomIndex);
            }
        }

        // Завершаем волну немедленно
        
        WaveCompleted();
    }

    private void WaveCompleted()
    {
        waveInProgress = false;
        currentWaveIndex++;

        if (currentWaveIndex >= waves.Length)
        {
            
            LevelManager.instance.CheckForLevelCompletion();
        }
        else
        {
            firstWaveStarted = true;
            startWaveButton.gameObject.SetActive(true);
            UpdateWaveText();
            StartCoroutine(WaveTimer());
        }
    }

    IEnumerator WaveTimer()
    {
        if (currentWaveIndex < waves.Length)
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
}
