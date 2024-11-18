using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [System.Serializable]
    public struct EnemyType
    {
        public string name;
        public GameObject enemyPrefab;
    }

    public List<EnemyType> enemyTypes; // Список типов врагов
    public int poolSize = 10;

    private Dictionary<string, Queue<GameObject>> enemyPools;
    private Path globalPath; // Ссылка на объект Path в сцене

    void Awake()
    {
        enemyPools = new Dictionary<string, Queue<GameObject>>();

        // Создаем пул для каждого типа врага
        foreach (var enemyType in enemyTypes)
        {
            Queue<GameObject> poolQueue = new Queue<GameObject>();
            for (int i = 0; i < poolSize; i++)
            {
                GameObject enemy = Instantiate(enemyType.enemyPrefab);
                enemy.SetActive(false);
                poolQueue.Enqueue(enemy);
            }
            enemyPools.Add(enemyType.name, poolQueue);
        }

        globalPath = FindObjectOfType<Path>(); // Ищем объект Path в сцене
        if (globalPath == null)
        {
            Debug.LogError("Path не найден в сцене!");
        }
    }

    public GameObject GetEnemy(string enemyTypeName)
    {
        if (enemyPools.ContainsKey(enemyTypeName) && enemyPools[enemyTypeName].Count > 0)
        {
            GameObject enemy = enemyPools[enemyTypeName].Dequeue();
            var enemyController = enemy.GetComponent<EnemyControler>();

            if (enemyController != null && globalPath != null)
            {
                enemyController.Setup(globalPath); // Устанавливаем путь для врага
            }

            enemy.GetComponent<EnemyHealthController>().ResetEnemy(); // Сбрасываем состояние врага
            enemy.SetActive(true);
            return enemy;
        }
        else
        {
            // Если пул пуст, создаем нового врага
            var enemyType = enemyTypes.Find(type => type.name == enemyTypeName);
            GameObject enemy = Instantiate(enemyType.enemyPrefab);

            var enemyController = enemy.GetComponent<EnemyControler>();
            if (enemyController != null && globalPath != null)
            {
                enemyController.Setup(globalPath); // Устанавливаем путь для нового врага
            }

            return enemy;
        }
    }

    public void ReturnEnemy(GameObject enemy)
    {
        // Находим тип врага по его префабу
        string enemyTypeName = null;
        foreach (var enemyType in enemyTypes)
        {
            if (enemyType.enemyPrefab.name == enemy.name.Replace("(Clone)", "").Trim())
            {
                enemyTypeName = enemyType.name;
                break;
            }
        }

        // Если тип найден, возвращаем врага в пул
        if (enemyTypeName != null && enemyPools.ContainsKey(enemyTypeName))
        {
            enemy.SetActive(false);
            enemyPools[enemyTypeName].Enqueue(enemy);
        }
        else
        {
            Destroy(enemy); // Уничтожаем, если тип врага не найден
        }
    }
}
