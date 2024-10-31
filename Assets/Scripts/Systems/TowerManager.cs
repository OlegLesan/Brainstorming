using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class TowerManager : MonoBehaviour
{
    public static TowerManager instance;

    private GameObject placementObject;
    public Tower activeTower;
    public ParticleSystem placementFX;
    public float fxOffsetY = 1.5f;

    public GraphicRaycaster uiRaycaster;
    public EventSystem eventSystem;

    private Tower selectedTower;

    private void Awake()
    {
        instance = this;
    }

    public void SetPlacementObject(GameObject placementObj)
    {
        placementObject = placementObj;
    }

    public void StartTowerPlacement(Tower towerToPlace)
    {
        // Проверка наличия достаточного количества денег
        if (MoneyManager.instance.SpendMoney(towerToPlace.cost))
        {
            activeTower = towerToPlace;

            if (placementObject != null)
            {
                Vector3 placementPosition = placementObject.transform.position;
                Destroy(placementObject);

                Instantiate(activeTower.gameObject, placementPosition, Quaternion.identity);

                if (placementFX != null)
                {
                    Vector3 fxPosition = placementPosition + new Vector3(0, fxOffsetY, 0);
                    Instantiate(placementFX, fxPosition, Quaternion.identity).Play();
                }

                UIController.instance.HideTowerButtons();
            }
        }
        else
        {
            // Если недостаточно денег, показать предупреждение
            UIController.instance.notEnoughMoneyWarning.SetActive(true);
            Invoke("HideWarning", 2f); // Скрыть предупреждение через 2 секунды
        }
    }

    private void HideWarning()
    {
        UIController.instance.notEnoughMoneyWarning.SetActive(false);
    }

    public void SelectTower(Tower tower)
    {
        if (selectedTower != null && selectedTower != tower)
        {
            selectedTower.ShowRangeModel(false);
        }

        selectedTower = tower;
        selectedTower.ShowRangeModel(true);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (selectedTower != null && !IsPointerOverUIObject() && !IsTowerClicked())
            {
                selectedTower.ShowRangeModel(false);
                selectedTower = null;
            }
        }
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventData = new PointerEventData(eventSystem) { position = Input.mousePosition };
        var results = new List<RaycastResult>();
        uiRaycaster.Raycast(eventData, results);
        return results.Count > 0;
    }

    private bool IsTowerClicked()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.collider.GetComponent<Tower>() != null;
        }
        return false;
    }
}
