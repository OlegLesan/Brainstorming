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

    public List<EnemyType> enemyTypes; // ������ ����� ������
    public int poolSize = 10;

    private Dictionary<string, Queue<GameObject>> enemyPools;
    private Path globalPath; // ������ �� ������ Path � �����

    void Awake()
    {
        enemyPools = new Dictionary<string, Queue<GameObject>>();

        // ������� ��� ��� ������� ���� �����
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

        globalPath = FindObjectOfType<Path>(); // ���� ������ Path � �����
        if (globalPath == null)
        {
            Debug.LogError("Path �� ������ � �����!");
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
                enemyController.Setup(globalPath); // ������������� ���� ��� �����
            }

            enemy.GetComponent<EnemyHealthController>().ResetEnemy(); // ���������� ��������� �����
            enemy.SetActive(true);
            return enemy;
        }
        else
        {
            // ���� ��� ����, ������� ������ �����
            var enemyType = enemyTypes.Find(type => type.name == enemyTypeName);
            GameObject enemy = Instantiate(enemyType.enemyPrefab);

            var enemyController = enemy.GetComponent<EnemyControler>();
            if (enemyController != null && globalPath != null)
            {
                enemyController.Setup(globalPath); // ������������� ���� ��� ������ �����
            }

            return enemy;
        }
    }

    public void ReturnEnemy(GameObject enemy)
    {
        // ������� ��� ����� �� ��� �������
        string enemyTypeName = null;
        foreach (var enemyType in enemyTypes)
        {
            if (enemyType.enemyPrefab.name == enemy.name.Replace("(Clone)", "").Trim())
            {
                enemyTypeName = enemyType.name;
                break;
            }
        }

        // ���� ��� ������, ���������� ����� � ���
        if (enemyTypeName != null && enemyPools.ContainsKey(enemyTypeName))
        {
            enemy.SetActive(false);
            enemyPools[enemyTypeName].Enqueue(enemy);
        }
        else
        {
            Destroy(enemy); // ����������, ���� ��� ����� �� ������
        }
    }
}
