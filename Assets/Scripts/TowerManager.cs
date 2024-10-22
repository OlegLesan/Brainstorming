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

    public LayerMask whatIsPlacement, whatIsObstacle;


    void Update()
    {
        if (isPlacing)
        {
            indicator.position = GetGridPosition();

            RaycastHit hit;
            if (Physics.Raycast(indicator.position + new Vector3(0f, -2f, 0f), Vector3.up, out hit, 10f, whatIsObstacle))
            {
                indicator.gameObject.SetActive(false);
            }
            else
            {

                indicator.gameObject.SetActive(true);

                if (Input.GetMouseButtonDown(0))
                {
                    isPlacing = false;

                    Instantiate(activeTower, indicator.position, activeTower.transform.rotation);
                    indicator.gameObject.SetActive(false);
                }
            }
        }
    }


    public void StartTowerPlacement(Tower towerToPlace)
    {
        activeTower = towerToPlace;
       
        isPlacing = true;

        Destroy(indicator.gameObject);
        Tower placeTower = Instantiate(activeTower);
        placeTower.enabled = false;
        placeTower.GetComponent<Collider>().enabled = false;

        indicator = placeTower.transform;
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
