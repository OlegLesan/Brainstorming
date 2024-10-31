using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // ��� ������ � EventSystem
using UnityEngine.UI; // ��� GraphicRaycaster

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

    public LayerMask whatIsObstacle, groundLayer; // ������ ���������� whatIsPlacement
    public float topSafePercent = 15f;

    public float towerHeight = 0.1f;

    // ��� ������ � UI
    public GraphicRaycaster uiRaycaster;
    public EventSystem eventSystem;

    void Update()
    {
        if (isPlacing)
        {
            // ��������, �� ������� �� ������ �� ������� UI, ������� Canvas
            if (IsPointerOverUIElement())
            {
                indicator.gameObject.SetActive(false); // ��������� ���������, ���� ������ ��� ��������� UI
                return;
            }

            Vector3 gridPosition = GetGridPosition();
            indicator.position = gridPosition;

            RaycastHit hit;

            // �� ���������� ���������, ���� ���� � ������� ����� ������
            if (Input.mousePosition.y > Screen.height * (1f - (topSafePercent / 100f)))
            {
                indicator.gameObject.SetActive(false);
                return; // ��������� ����������, ���� ���� � ������� ����� ������
            }

            // �������� �� ������� ����������� �����
            bool obstacleDetected = Physics.Raycast(indicator.position + new Vector3(0f, -2f, 0f), Vector3.up, out hit, 10f, whatIsObstacle);
            if (obstacleDetected)
            {
                indicator.gameObject.SetActive(false);
                return;
            }
            else
            {
                indicator.gameObject.SetActive(true);

                // ��������, ��������� �� ������ ��� ���������� ���� �� ���� groundLayer
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f, groundLayer)) // ���������� groundLayer
                {
                    // ���������, ��� ������ ��� ���������� ���� ������������� ��������� �� ���� Ground
                    if (((1 << hit.collider.gameObject.layer) & groundLayer) == 0)
                    {
                        Debug.Log("Cannot place tower, the object is not on the Ground layer.");
                        return;
                    }
                }
                else
                {
                    Debug.Log("Not hitting ground layer");
                    return; // ��������� ����������, ���� ��� ���������� ���� ��� ������� �� ���� Ground
                }

              
                

                bool notEnoughMoney = MoneyManager.instance.currentMoney < activeTower.cost;
                UIController.instance.notEnoughMoneyWarning.SetActive(notEnoughMoney);

                if (!notEnoughMoney)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        // ��� ��� ���������, ��� �� �����������
                        obstacleDetected = Physics.Raycast(indicator.position + new Vector3(0f, -2f, 0f), Vector3.up, out hit, 10f, whatIsObstacle);

                        if (!obstacleDetected && MoneyManager.instance.SpendMoney(activeTower.cost))
                        {
                            // ������� �����
                            GameObject placedTower = Instantiate(activeTower.gameObject, indicator.position, indicator.rotation);
                            SetLayerRecursively(placedTower, LayerMask.NameToLayer("Tower")); // ������������� ���� Tower

                            isPlacing = false;
                            UIController.instance.notEnoughMoneyWarning.SetActive(false);
                            Destroy(indicator.gameObject); // ������� ���������
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

        // ������� ���������� ���������, ���� �� ���
        if (indicator != null)
        {
            Destroy(indicator.gameObject);
        }

        // ������� ��������� �� ������ �����, �� �� ���� �����
        GameObject indicatorInstance = Instantiate(towerToPlace.gameObject);
        indicator = indicatorInstance.transform;

        // ��������� ��� �������� ���������� � ����������
        indicator.GetComponent<Tower>().enabled = false; // ��������� ������ �����
        indicator.GetComponent<Collider>().enabled = false; // ��������� ���������

        // �������� ������ ������� �������� �����
        indicator.GetComponent<Tower>().rangeModel.SetActive(true);
        indicator.GetComponent<Tower>().rangeModel.transform.localScale = new Vector3(towerToPlace.range, 1f, towerToPlace.range);
    }

    public Vector3 GetGridPosition()
    {
        Vector3 location = Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(location);
        Debug.DrawRay(ray.origin, ray.direction * 200f, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 200f, groundLayer)) // ���������� groundLayer
        {
            location = hit.point;
            location.y += towerHeight;
        }
        location.y = 2.6f; // ��� ��������, ��������, �� ��������; ���������� ������������ hit.point.y
        return location;
    }

    // ����� ��� ��������, �� ������� �� ��������� �� ������� UI, ������� ������� Canvas
    private bool IsPointerOverUIElement()
    {
        // ������� ������ ��� �������� ����������� Raycast
        List<RaycastResult> results = new List<RaycastResult>();

        // ����������� ������� ��� ��������
        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.position = Input.mousePosition;

        // ��������� Raycast � ������� GraphicRaycaster
        uiRaycaster.Raycast(eventData, results);

        // ���� ���� ������� UI ��� ��������, ������ true
        return results.Count > 0;
    }

    // ����� ��� ����������� ��������� ���� ��� ������� � ���� ��� �������� ��������
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
