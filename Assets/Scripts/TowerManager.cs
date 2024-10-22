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
    




    public void StartTowerPlacement(Tower towerToPlace)
    {
        activeTower = towerToPlace;
        Debug.Log("Placing a tower");
    }
}
