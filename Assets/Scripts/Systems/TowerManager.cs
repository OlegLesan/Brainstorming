using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerManager : MonoBehaviour
{
    public static TowerManager instance;

    private GameObject placementObject; // ������ �� ������ Placement
    public Tower activeTower;
    public ParticleSystem placementFX; // Particle System ��� ���������� �����
    public float fxOffsetY = 1.5f; // �������� �� ������ ��� FX

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

    // ����� ��� ������ ���������� �����
    public void StartTowerPlacement(Tower towerToPlace)
    {
        activeTower = towerToPlace;

        // ������� ������ Placement � ��������� ����� �� ��� �����
        if (placementObject != null)
        {
            Vector3 placementPosition = placementObject.transform.position;
            Destroy(placementObject);

            // ������� ����� �� ������� ������� Placement
            Instantiate(activeTower.gameObject, placementPosition, Quaternion.identity);

            // ��������� Particle FX �� ������ �����
            if (placementFX != null)
            {
                // ������� FX �� ������� ��������� ����� � ������ �������� �� Y
                Vector3 fxPosition = placementPosition + new Vector3(0, fxOffsetY, 0);
                Instantiate(placementFX, fxPosition, Quaternion.identity).Play();
            }

            // �������� ������ � �������� ����� ���������� �����
            UIController.instance.HideTowerButtons();
        }
    }
}
