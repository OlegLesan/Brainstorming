using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    public Wave[] waves;
    public Transform spawnPoint;
    public Button startWaveButton;
    public TMP_Text waveText; // Основной текст волны
    public TMP_Text waveCounterText; // Новый текст для отображения текущей волны из общего количества
    public float timeBetweenWaves = 2f;
    public float waveDuration = 10f;
    public int rewardForEarlyStart = 50;
    public float fillAmountDecreaseRate = 1f;

    private int currentWaveIndex = 0;
    private bool waveInProgress = false;
    private bool firstWaveStarted = false;
    public int totalEnemiesRemaining = 0;

    private Image buttonImage;

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
    }

    private void Start()
    {
        foreach (Wave wave in waves)
        {
            foreach (int count in wave.enemyCounts)
            {
                totalEnemiesRemaining += count;
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

        // Обновляем новый текст, показывающий текущую волну из общего количества
        waveCounterText.text = $"Wave {currentWaveIndex + 1} / {waves.Length}";
    }

    public void DecreaseEnemyCount()
    {
        totalEnemiesRemaining--;
        Debug.Log("Осталось врагов: " + totalEnemiesRemaining);

        if (totalEnemiesRemaining <= 0)
        {
            CheckForLevelCompletion();
        }
    }

    private void CheckForLevelCompletion()
    {
        if (LevelManager.instance.activeEnemies.Count == 0 && totalEnemiesRemaining <= 0)
        {
            Debug.Log("Уровень завершён! Все враги уничтожены.");
            LevelManager.instance.LevelComplete();
        }
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
        int totalEnemies = 0;
        for (int i = 0; i < wave.enemyCounts.Length; i++)
        {
            totalEnemies += wave.enemyCounts[i];
        }

        for (int i = 0; i < totalEnemies; i++)
        {
            int randomIndex = Random.Range(0, wave.enemyPrefabs.Length);
            Instantiate(wave.enemyPrefabs[randomIndex], spawnPoint.position, spawnPoint.rotation);
            yield return new WaitForSeconds(wave.spawnInterval);
        }

        yield return new WaitForSeconds(waveDuration);
        WaveCompleted();
    }

    private void WaveCompleted()
    {
        waveInProgress = false;
        currentWaveIndex++;

        Debug.Log("Волна завершена. Текущая волна: " + currentWaveIndex);

        if (currentWaveIndex < waves.Length)
        {
            firstWaveStarted = true;
            startWaveButton.gameObject.SetActive(true);
            UpdateWaveText();
            StartCoroutine(WaveTimer());
        }
        else
        {
            Debug.Log("Все волны завершены!");
            CheckForLevelCompletion();
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
