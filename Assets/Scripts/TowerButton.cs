using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // ���������� ������������ ���� TextMeshPro

public class TowerButton : MonoBehaviour
{
    public Tower towerToPlace;
    public TMP_Text costText; // ������ �� ��������� TextMeshPro ��� ����������� ���������

    private void Start()
    {
        // ���������� ����� � ��������� ����� ��� �������� ������
        costText.text = towerToPlace.GetFormattedCost();
    }

    public void SelectTower()
    {
        TowerManager.instance.StartTowerPlacement(towerToPlace);
    }
}
