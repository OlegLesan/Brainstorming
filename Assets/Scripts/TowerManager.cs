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
    public Transform indicator;
    public bool isPlacing;

    void Update()
    {
        if(isPlacing)
        {
            indicator.position = GetGridPosition();
        }
    }


    public void StartTowerPlacement(Tower towerToPlace)
    {
        activeTower = towerToPlace;
        Debug.Log("Placing a tower");
        isPlacing = true;
        indicator.gameObject.SetActive(true);
    }

    public Vector3 GetGridPosition()
    {
        Vector3 location = new Vector3(2f, 0f, 2f);

        return location;
    }
}
