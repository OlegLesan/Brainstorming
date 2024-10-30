using UnityEngine;

[System.Serializable]
public class Wave
{
    public GameObject[] enemyPrefabs;  // Массив префабов врагов для этой волны
    public int[] enemyCounts;  // Количество каждого типа врагов в этой волне
    public float spawnInterval = 1f;  // Интервал между спавнами врагов
}
