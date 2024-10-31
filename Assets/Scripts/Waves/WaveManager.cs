using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Подключаем TextMeshPro

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;  // Singleton для доступа к WaveManager

    public Wave[] waves;  // Массив всех волн
    public Transform spawnPoint;  // Точка спавна врагов
    public Button startWaveButton;  // Кнопка для запуска волн
    public TMP_Text waveText;  // Текст для отображения номера волны
    public float timeBetweenWaves = 2f;  // Время между волнами (задержка до появления кнопки)
    public float waveDuration = 10f;  // Продолжительность волны (в секундах)

    private int currentWaveIndex = 0;  // Индекс текущей волны
    private bool waveInProgress = false;  // Флаг, что волна началась
    private int totalEnemiesRemaining = 0;  // Общее количество оставшихся врагов во всех волнах

    private void Awake()
    {
        // Инициализация Singleton
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
        // Считаем общее количество врагов из всех волн
        foreach (Wave wave in waves)
        {
            foreach (int count in wave.enemyCounts)
            {
                totalEnemiesRemaining += count;
            }
        }

        // Привязываем кнопку к методу StartWave
        startWaveButton.onClick.AddListener(StartWave);
        startWaveButton.gameObject.SetActive(true);  // Кнопка активна в начале игры

        // Отображаем номер первой волны
        UpdateWaveText();
    }

    // Метод для обновления текста волны
    private void UpdateWaveText()
    {
        waveText.text = "Wave: " + (currentWaveIndex + 1);
    }

    // Метод для уменьшения количества оставшихся врагов
    public void DecreaseEnemyCount()
    {
        totalEnemiesRemaining--;  // Уменьшаем общее количество оставшихся врагов

        Debug.Log("Осталось врагов: " + totalEnemiesRemaining);

        if (totalEnemiesRemaining <= 0)
        {
            // Если все враги уничтожены, завершаем уровень
            CheckForLevelCompletion();
        }
    }

    private void CheckForLevelCompletion()
    {
        Debug.Log("Проверка завершения уровня. Осталось врагов: " + totalEnemiesRemaining);

        // Если врагов не осталось, завершаем уровень
        if (totalEnemiesRemaining <= 0)
        {
            Debug.Log("Уровень завершён! Все враги уничтожены.");
            LevelManager.instance.LevelComplete();  // Вызов метода для победы
        }
    }

    // Метод запуска волны
    public void StartWave()
    {
        if (!waveInProgress && currentWaveIndex < waves.Length)
        {
            // Обновляем текст номера волны перед запуском
            UpdateWaveText();

            waveInProgress = true;  // Устанавливаем флаг, что волна началась
            startWaveButton.gameObject.SetActive(false);  // Отключаем кнопку, пока идёт волна

            // Запускаем корутину для отслеживания времени волны
            StartCoroutine(WaveTimer());

            // Запускаем корутину для спавна врагов для текущей волны
            StartCoroutine(SpawnWave(waves[currentWaveIndex]));
        }
    }

    IEnumerator WaveTimer()
    {
        // Ждём завершения времени волны
        yield return new WaitForSeconds(waveDuration);

        // Волна завершена после таймера
        WaveCompleted();
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
    }

    // Завершение волны
    private void WaveCompleted()
    {
        waveInProgress = false;
        currentWaveIndex++;

        Debug.Log("Волна завершена. Текущая волна: " + currentWaveIndex);

        if (currentWaveIndex < waves.Length)
        {
            // Есть ещё волны — активируем кнопку для следующей волны
            startWaveButton.gameObject.SetActive(true);

            // Обновляем текст для следующей волны
            UpdateWaveText();
        }
        else
        {
            Debug.Log("Все волны завершены!");

            // Проверяем завершение уровня
            CheckForLevelCompletion();
        }
    }
}
