using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    public Wave[] waves;  // Массив всех волн
    public Transform spawnPoint;  // Точка спавна врагов
    public Button startWaveButton;  // Кнопка для запуска волн
    public float timeBetweenWaves = 2f;  // Время между волнами (задержка до появления кнопки)

    private int currentWaveIndex = 0;  // Индекс текущей волны
    private bool waveInProgress = false;  // Флаг, что волна началась

    private void Start()
    {
        // Привязываем кнопку к методу StartWave
        startWaveButton.onClick.AddListener(StartWave);
        startWaveButton.gameObject.SetActive(true);  // Кнопка активна в начале игры
    }

    // Метод запуска волны
    public void StartWave()
    {
        if (!waveInProgress && currentWaveIndex < waves.Length)
        {
            waveInProgress = true;  // Устанавливаем флаг, что волна началась
            startWaveButton.gameObject.SetActive(false);  // Отключаем кнопку, пока идёт волна
            StartCoroutine(SpawnWave(waves[currentWaveIndex]));  // Запускаем спавн врагов для текущей волны
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        int totalEnemies = 0;
        for (int i = 0; i < wave.enemyCounts.Length; i++)
        {
            totalEnemies += wave.enemyCounts[i];  // Подсчёт общего количества врагов в волне
        }

        // Цикл для спавна всех врагов
        for (int i = 0; i < totalEnemies; i++)
        {
            // Случайно выбираем врага из массива префабов
            int randomIndex = Random.Range(0, wave.enemyPrefabs.Length);

            // Спавним выбранного врага
            Instantiate(wave.enemyPrefabs[randomIndex], spawnPoint.position, spawnPoint.rotation);

            yield return new WaitForSeconds(wave.spawnInterval);  // Ждём перед спавном следующего врага
        }

        // Завершение волны
        waveInProgress = false;
        currentWaveIndex++;

        // Если это не последняя волна, показываем кнопку для следующей волны
        if (currentWaveIndex < waves.Length)
        {
            yield return new WaitForSeconds(timeBetweenWaves);  // Задержка перед появлением кнопки
            startWaveButton.gameObject.SetActive(true);  // Включаем кнопку для запуска следующей волны
        }
        else
        {
            Debug.Log("Все волны завершены!");
        }
    }
}
