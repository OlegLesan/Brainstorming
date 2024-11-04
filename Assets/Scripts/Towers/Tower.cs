using System.Collections.Generic;
using UnityEngine;

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
    public int upgradeCost = 150; // ��������� ��������� �����
    public int sellValue = 50; // ��������� ������� �����

    public GameObject upgradedTowerPrefab; // ������ ���������� �����
    public GameObject placementPrefab; // ������ ������� � ����������� Placement

    private void Start()
    {
        checkCounter = checkTime;

        if (rangeModel != null)
        {
            float scale = range;
            rangeModel.transform.localScale = new Vector3(scale, rangeModel.transform.localScale.y, scale);
            rangeModel.SetActive(false);
        }

        if (light != null)
        {
            light.SetActive(false);
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
            checkCounter = checkTime;
        }
    }

    public string GetFormattedCost()
    {
        return $"{cost}G";
    }

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

    private void OnMouseDown()
    {
        TowerManager.instance.SelectTower(this);
    }

    // ����� ��� ��������� �����
    public void UpgradeTower()
    {
        if (upgradedTowerPrefab != null && MoneyManager.instance.SpendMoney(upgradeCost))
        {
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
            Destroy(gameObject); // ������� ������� ������
            Instantiate(upgradedTowerPrefab, position, rotation); // ������ ����� ������ �� ��� �� �����
        }
    }

    // ����� ��� ������� �����
    public void SellTower()
    {
        MoneyManager.instance.GiveMoney(sellValue); // ���������� ������
        UIController.instance.HideUpgradePanel(); // ������ ������ ���������

        // ������� ������ ���������� �� ����� ��������� �����
        if (placementPrefab != null)
        {
            Instantiate(placementPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject); // ������� �����
    }
}
