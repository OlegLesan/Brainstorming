using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerManager : MonoBehaviour
{
    public static TowerManager instance;

    private GameObject placementObject;
    public Tower activeTower;
    public ParticleSystem placementFX;
    public float fxOffsetY = 1.5f;

    public GraphicRaycaster uiRaycaster;
    public EventSystem eventSystem;

    public Tower selectedTower; // Добавлено для доступа из UIController

    private LayerMask towerLayerMask; // Добавлено для маски слоя

    private void Awake()
    {
        instance = this;
        towerLayerMask = ~LayerMask.GetMask("IgnoreRaycast"); // Установить маску, исключающую слой IgnoreRaycast
    }

    public void SetPlacementObject(GameObject placementObj)
    {
        placementObject = placementObj;
    }

    public void StartTowerPlacement(Tower towerToPlace)
    {
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
            UIController.instance.notEnoughMoneyWarning.SetActive(true);
            Invoke("HideWarning", 2f);
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

        UIController.instance.ShowUpgradePanel(selectedTower); // Показать панель улучшений
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (selectedTower != null && !IsPointerOverUIObject() && !IsTowerClicked())
            {
                selectedTower.ShowRangeModel(false);
                selectedTower = null;

                UIController.instance.HideUpgradePanel(); // Скрыть панель улучшений при клике на пустое место
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
        RaycastHit[] hits = Physics.RaycastAll(ray);

        foreach (RaycastHit hit in hits)
        {
            // Игнорируем слой "Flashlight"
            if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Ignore" + "Raycast"))
            {
                if (hit.collider.GetComponent<Tower>() != null)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
