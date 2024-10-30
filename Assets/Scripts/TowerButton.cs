using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Подключите пространство имен TextMeshPro

public class TowerButton : MonoBehaviour
{
    public Tower towerToPlace;
    public TMP_Text costText; // Ссылка на компонент TextMeshPro для отображения стоимости

    private void Start()
    {
        // Установите текст в стоимость башни при создании кнопки
        costText.text = towerToPlace.GetFormattedCost();
    }

    public void SelectTower()
    {
        TowerManager.instance.StartTowerPlacement(towerToPlace);
    }
}
