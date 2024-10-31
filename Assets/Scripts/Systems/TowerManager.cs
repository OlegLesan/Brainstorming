using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Для работы с EventSystem
using UnityEngine.UI; // Для GraphicRaycaster

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

    public LayerMask whatIsObstacle, groundLayer; // Убрана переменная whatIsPlacement
    public float topSafePercent = 15f;

    public float towerHeight = 0.1f;

    // Для работы с UI
    public GraphicRaycaster uiRaycaster;
    public EventSystem eventSystem;

    void Update()
    {
        if (isPlacing)
        {
            // Проверка, не наведен ли курсор на элемент UI, включая Canvas
            if (IsPointerOverUIElement())
            {
                indicator.gameObject.SetActive(false); // Отключаем индикатор, если курсор над элементом UI
                return;
            }

            Vector3 gridPosition = GetGridPosition();
            indicator.position = gridPosition;

            RaycastHit hit;

            // Не показывать индикатор, если мышь в верхней части экрана
            if (Input.mousePosition.y > Screen.height * (1f - (topSafePercent / 100f)))
            {
                indicator.gameObject.SetActive(false);
                return; // Прерываем выполнение, если мышь в верхней части экрана
            }

            // Проверка на наличие препятствий внизу
            bool obstacleDetected = Physics.Raycast(indicator.position + new Vector3(0f, -2f, 0f), Vector3.up, out hit, 10f, whatIsObstacle);
            if (obstacleDetected)
            {
                indicator.gameObject.SetActive(false);
                return;
            }
            else
            {
                indicator.gameObject.SetActive(true);

                // Проверка, находится ли объект под указателем мыши на слое groundLayer
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f, groundLayer)) // Используем groundLayer
                {
                    // Проверяем, что объект под указателем мыши действительно находится на слое Ground
                    if (((1 << hit.collider.gameObject.layer) & groundLayer) == 0)
                    {
                        Debug.Log("Cannot place tower, the object is not on the Ground layer.");
                        return;
                    }
                }
                else
                {
                    Debug.Log("Not hitting ground layer");
                    return; // Прерываем выполнение, если под указателем мыши нет объекта на слое Ground
                }

              
                

                bool notEnoughMoney = MoneyManager.instance.currentMoney < activeTower.cost;
                UIController.instance.notEnoughMoneyWarning.SetActive(notEnoughMoney);

                if (!notEnoughMoney)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        // Еще раз проверяем, нет ли препятствий
                        obstacleDetected = Physics.Raycast(indicator.position + new Vector3(0f, -2f, 0f), Vector3.up, out hit, 10f, whatIsObstacle);

                        if (!obstacleDetected && MoneyManager.instance.SpendMoney(activeTower.cost))
                        {
                            // Создаем башню
                            GameObject placedTower = Instantiate(activeTower.gameObject, indicator.position, indicator.rotation);
                            SetLayerRecursively(placedTower, LayerMask.NameToLayer("Tower")); // Устанавливаем слой Tower

                            isPlacing = false;
                            UIController.instance.notEnoughMoneyWarning.SetActive(false);
                            Destroy(indicator.gameObject); // Удаляем индикатор
                            Debug.Log("Tower placed successfully!");
                        }
                        else
                        {
                            Debug.Log("Failed to place tower! Either an obstacle is in the way or not enough money!");
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

        // Удаляем предыдущий индикатор, если он был
        if (indicator != null)
        {
            Destroy(indicator.gameObject);
        }

        // Создаем индикатор на основе башни, но не саму башню
        GameObject indicatorInstance = Instantiate(towerToPlace.gameObject);
        indicator = indicatorInstance.transform;

        // Отключаем все ненужные компоненты у индикатора
        indicator.GetComponent<Tower>().enabled = false; // Отключаем логику башни
        indicator.GetComponent<Collider>().enabled = false; // Отключаем коллайдер

        // Включаем модель радиуса действия башни
        indicator.GetComponent<Tower>().rangeModel.SetActive(true);
        indicator.GetComponent<Tower>().rangeModel.transform.localScale = new Vector3(towerToPlace.range, 1f, towerToPlace.range);
    }

    public Vector3 GetGridPosition()
    {
        Vector3 location = Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(location);
        Debug.DrawRay(ray.origin, ray.direction * 200f, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 200f, groundLayer)) // Используем groundLayer
        {
            location = hit.point;
            location.y += towerHeight;
        }
        location.y = 2.6f; // Это значение, возможно, не подходит; попробуйте использовать hit.point.y
        return location;
    }

    // Метод для проверки, не наведен ли указатель на элемент UI, включая объекты Canvas
    private bool IsPointerOverUIElement()
    {
        // Создаем список для хранения результатов Raycast
        List<RaycastResult> results = new List<RaycastResult>();

        // Настраиваем событие для проверки
        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.position = Input.mousePosition;

        // Выполняем Raycast с помощью GraphicRaycaster
        uiRaycaster.Raycast(eventData, results);

        // Если есть объекты UI под курсором, вернем true
        return results.Count > 0;
    }

    // Метод для рекурсивной установки слоя для объекта и всех его дочерних объектов
    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (child == null) continue;
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
