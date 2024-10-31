using UnityEngine;
using TMPro;

public class TowerButton : MonoBehaviour
{
    public Tower towerToPlace;
    public TMP_Text costText;

    private void Start()
    {
        costText.text = towerToPlace.GetFormattedCost();
    }

    public void SelectTower()
    {
        // �������� ���������� �����
        TowerManager.instance.StartTowerPlacement(towerToPlace);
    }
}
