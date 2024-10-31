using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerManager : MonoBehaviour
{
    public static TowerManager instance;

    private GameObject placementObject; // Ссылка на объект Placement
    public Tower activeTower;
    public ParticleSystem placementFX; // Particle System для размещения башни
    public float fxOffsetY = 1.5f; // Смещение по высоте для FX

    public GraphicRaycaster uiRaycaster;
    public EventSystem eventSystem;

    private void Awake()
    {
        instance = this;
    }

    public void SetPlacementObject(GameObject placementObj)
    {
        placementObject = placementObj;
    }

    // Метод для начала размещения башни
    public void StartTowerPlacement(Tower towerToPlace)
    {
        activeTower = towerToPlace;

        // Удаляем объект Placement и размещаем башню на его месте
        if (placementObject != null)
        {
            Vector3 placementPosition = placementObject.transform.position;
            Destroy(placementObject);

            // Создаем башню на позиции объекта Placement
            Instantiate(activeTower.gameObject, placementPosition, Quaternion.identity);

            // Запускаем Particle FX на уровне башни
            if (placementFX != null)
            {
                // Создаем FX на позиции установки башни с учетом смещения по Y
                Vector3 fxPosition = placementPosition + new Vector3(0, fxOffsetY, 0);
                Instantiate(placementFX, fxPosition, Quaternion.identity).Play();
            }

            // Скрываем панель с кнопками после размещения башни
            UIController.instance.HideTowerButtons();
        }
    }
}
