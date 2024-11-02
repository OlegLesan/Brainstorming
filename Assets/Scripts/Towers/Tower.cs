using UnityEngine;
using System.Collections.Generic;

public class Tower : MonoBehaviour
{
    public float range = 3f;
    private Collider[] colliderInRange;
    public LayerMask whatIsEnemy;
    public List<EnemyControler> enemiesInRange = new List<EnemyControler>();
    private float checkCounter;
    public float checkTime = .2f;

    public bool enemiesUpdated;
    public GameObject rangeModel, light;
    public int cost = 100;

    private void Start()
    {
        checkCounter = checkTime;

        // Устанавливаем масштаб rangeModel только по осям X и Z в соответствии с range
        if (rangeModel != null)
        {
            float scale = range;
            rangeModel.transform.localScale = new Vector3(scale, rangeModel.transform.localScale.y, scale);
            rangeModel.SetActive(false); // Скрываем по умолчанию
        }

        // Делаем то же самое для light, но без изменения масштаба
        if (light != null)
        {
            light.SetActive(false); // Скрываем по умолчанию
        }
    }

    private void Update()
    {
        enemiesUpdated = false;
        checkCounter -= Time.deltaTime;
        if (checkCounter <= 0)
        {
            colliderInRange = Physics.OverlapSphere(transform.position, range, whatIsEnemy);
            enemiesInRange.Clear();
            foreach (Collider col in colliderInRange)
            {
                EnemyControler enemy = col.GetComponent<EnemyControler>();
                if (enemy != null)
                {
                    enemiesInRange.Add(enemy);
                }
            }
            enemiesUpdated = true;
            checkCounter = checkTime; // Сбрасываем таймер
        }
    }

    public string GetFormattedCost()
    {
        return $"{cost}G";
    }

    // Метод для отображения/скрытия модели радиуса и света
    public void ShowRangeModel(bool show)
    {
        if (rangeModel != null)
        {
            rangeModel.SetActive(show);
        }
        if (light != null)
        {
            light.SetActive(show);
        }
    }

    // Проверка клика на башне
    private void OnMouseDown()
    {
        TowerManager.instance.SelectTower(this);
    }
}
