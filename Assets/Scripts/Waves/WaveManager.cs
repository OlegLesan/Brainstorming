using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    public Wave[] waves;  // ������ ���� ����
    public Transform spawnPoint;  // ����� ������ ������
    public Button startWaveButton;  // ������ ��� ������� ����
    public float timeBetweenWaves = 2f;  // ����� ����� ������� (�������� �� ��������� ������)
    public float waveDuration = 10f;  // ����������������� ����� (� ��������)

    private int currentWaveIndex = 0;  // ������ ������� �����
    private bool waveInProgress = false;  // ����, ��� ����� ��������

    private void Start()
    {
        // ����������� ������ � ������ StartWave
        startWaveButton.onClick.AddListener(StartWave);
        startWaveButton.gameObject.SetActive(true);  // ������ ������� � ������ ����
    }

    // ����� ������� �����
    public void StartWave()
    {
        if (!waveInProgress && currentWaveIndex < waves.Length)
        {
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

        // ���� ��� �� ��������� �����, ���������� ������ ��� ��������� �����
        if (currentWaveIndex < waves.Length)
        {
            startWaveButton.gameObject.SetActive(true);  // �������� ������ ��� ������� ��������� �����
        }
        else
        {
            Debug.Log("��� ����� ���������!");
        }
    }
}
