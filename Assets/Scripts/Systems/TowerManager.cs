using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager instance;

    private void Awake()
    {
        instance = this;
    }

    public Tower activeTower;
    public Transform indicator;
    public bool isPlacing;

    public LayerMask whatIsPlacement, whatIsObstacle;
    public float topSafePercent = 15f;

    public float towerHeight = 0.1f;
    public float towerPlacementRadius = 2.0f; // Задайте радиус, в котором нельзя ставить другие башни

    void Update()
    {
        if (isPlacing)
        {
            indicator.position = GetGridPosition();

            RaycastHit hit;

            if (Input.mousePosition.y > Screen.height * (1f - (topSafePercent / 100f)))
            {
                indicator.gameObject.SetActive(false);
            }

            if (Physics.Raycast(indicator.position + new Vector3(0f, -2f, 0f), Vector3.up, out hit, 10f, whatIsObstacle))
            {
                indicator.gameObject.SetActive(false);
            }
            else
            {
                indicator.gameObject.SetActive(true);

                // Отладочные сообщения для проверки состояния
                Debug.Log($"Current Money: {MoneyManager.instance.currentMoney}, Tower Cost: {activeTower.cost}");

                // Проверяем, достаточно ли денег
                bool notEnoughMoney = MoneyManager.instance.currentMoney < activeTower.cost;
                UIController.instance.notEnoughMoneyWarning.SetActive(notEnoughMoney);

                // Проверяем, можно ли разместить башню
                if (CanPlaceTower(indicator.position) && !notEnoughMoney)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        // Если денег достаточно, пытаемся потратить деньги и установить башню
                        if (MoneyManager.instance.SpendMoney(activeTower.cost))
                        {
                            Instantiate(activeTower, indicator.position, indicator.rotation);
                            isPlacing = false;
                            UIController.instance.notEnoughMoneyWarning.SetActive(false);
                            Debug.Log("Tower placed successfully!");
                        }
                        else
                        {
                            Debug.Log("Failed to spend money!");
                        }
                    }
                }
                else
                {
                    Debug.Log("Cannot place tower or not enough money!");
                }
            }
        }
    }

    public void StartTowerPlacement(Tower towerToPlace)
    {
        activeTower = towerToPlace;
        isPlacing = true;

        Destroy(indicator.gameObject);
        Tower placeTower = Instantiate(activeTower);
        placeTower.enabled = false;
        placeTower.GetComponent<Collider>().enabled = false;

        indicator = placeTower.transform;

        placeTower.rangeModel.SetActive(true);
        placeTower.rangeModel.transform.localScale = new Vector3(placeTower.range, 1f, placeTower.range);
    }

    public Vector3 GetGridPosition()
    {
        Vector3 location = Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(location);
        Debug.DrawRay(ray.origin, ray.direction * 200f, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 200f, whatIsPlacement))
        {
            location = hit.point;
            location.y += towerHeight;
        }
        location.y = 2.6f; // Это значение, возможно, не подходит; попробуйте использовать hit.point.y
        return location;
    }

    // Метод для проверки возможности установки башни
    private bool CanPlaceTower(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, towerPlacementRadius, whatIsPlacement);
        return colliders.Length == 0; // Если в радиусе нет коллайдеров, можно установить
    }
}
