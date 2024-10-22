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

    public LayerMask whatIsPlacement;

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
       
        isPlacing = true;
        indicator.gameObject.SetActive(true);
    }

    public Vector3 GetGridPosition()
    {
        Vector3 location = Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(location);
        Debug.DrawRay(ray.origin, ray.direction * 200f, Color.red);

        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 200f, whatIsPlacement))
        {
            location = hit.point;
        }
        location.y = 2.6f;
        return location;
    }
}
