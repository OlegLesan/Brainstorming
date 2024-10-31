using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // ���������� TextMeshPro

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;  // Singleton ��� ������� � WaveManager

    public Wave[] waves;  // ������ ���� ����
    public Transform spawnPoint;  // ����� ������ ������
    public Button startWaveButton;  // ������ ��� ������� ����
    public TMP_Text waveText;  // ����� ��� ����������� ������ �����
    public float timeBetweenWaves = 2f;  // ����� ����� ������� (�������� �� ��������� ������)
    public float waveDuration = 10f;  // ����������������� ����� (� ��������)

    private int currentWaveIndex = 0;  // ������ ������� �����
    private bool waveInProgress = false;  // ����, ��� ����� ��������
    private int totalEnemiesRemaining = 0;  // ����� ���������� ���������� ������ �� ���� ������

    private void Awake()
    {
        // ������������� Singleton
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
        // ������� ����� ���������� ������ �� ���� ����
        foreach (Wave wave in waves)
        {
            foreach (int count in wave.enemyCounts)
            {
                totalEnemiesRemaining += count;
            }
        }

        // ����������� ������ � ������ StartWave
        startWaveButton.onClick.AddListener(StartWave);
        startWaveButton.gameObject.SetActive(true);  // ������ ������� � ������ ����

        // ���������� ����� ������ �����
        UpdateWaveText();
    }

    // ����� ��� ���������� ������ �����
    private void UpdateWaveText()
    {
        waveText.text = "Wave: " + (currentWaveIndex + 1);
    }

    // ����� ��� ���������� ���������� ���������� ������
    public void DecreaseEnemyCount()
    {
        totalEnemiesRemaining--;  // ��������� ����� ���������� ���������� ������

        Debug.Log("�������� ������: " + totalEnemiesRemaining);

        if (totalEnemiesRemaining <= 0)
        {
            // ���� ��� ����� ����������, ��������� �������
            CheckForLevelCompletion();
        }
    }

    private void CheckForLevelCompletion()
    {
        Debug.Log("�������� ���������� ������. �������� ������: " + totalEnemiesRemaining);

        // ���� ������ �� ��������, ��������� �������
        if (totalEnemiesRemaining <= 0)
        {
            Debug.Log("������� ��������! ��� ����� ����������.");
            LevelManager.instance.LevelComplete();  // ����� ������ ��� ������
        }
    }

    // ����� ������� �����
    public void StartWave()
    {
        if (!waveInProgress && currentWaveIndex < waves.Length)
        {
            // ��������� ����� ������ ����� ����� ��������
            UpdateWaveText();

            waveInProgress = true;  // ������������� ����, ��� ����� ��������
            startWaveButton.gameObject.SetActive(false);  // ��������� ������, ���� ��� �����

            // ��������� �������� ��� ������������ ������� �����
            StartCoroutine(WaveTimer());

            // ��������� �������� ��� ������ ������ ��� ������� �����
            StartCoroutine(SpawnWave(waves[currentWaveIndex]));
        }
    }

    IEnumerator WaveTimer()
    {
        // ��� ���������� ������� �����
        yield return new WaitForSeconds(waveDuration);

        // ����� ��������� ����� �������
        WaveCompleted();
    }

    IEnumerator SpawnWave(Wave wave)
    {
        int totalEnemies = 0;
        for (int i = 0; i < wave.enemyCounts.Length; i++)
        {
            totalEnemies += wave.enemyCounts[i];  // ������� ������ ���������� ������ � �����
        }

        // ���� ��� ������ ���� ������
        for (int i = 0; i < totalEnemies; i++)
        {
            // �������� �������� ����� �� ������� ��������
            int randomIndex = Random.Range(0, wave.enemyPrefabs.Length);

            // ������� ���������� �����
            Instantiate(wave.enemyPrefabs[randomIndex], spawnPoint.position, spawnPoint.rotation);

            yield return new WaitForSeconds(wave.spawnInterval);  // ��� ����� ������� ���������� �����
        }
    }

    // ���������� �����
    private void WaveCompleted()
    {
        waveInProgress = false;
        currentWaveIndex++;

        Debug.Log("����� ���������. ������� �����: " + currentWaveIndex);

        if (currentWaveIndex < waves.Length)
        {
            // ���� ��� ����� � ���������� ������ ��� ��������� �����
            startWaveButton.gameObject.SetActive(true);

            // ��������� ����� ��� ��������� �����
            UpdateWaveText();
        }
        else
        {
            Debug.Log("��� ����� ���������!");

            // ��������� ���������� ������
            CheckForLevelCompletion();
        }
    }
}
