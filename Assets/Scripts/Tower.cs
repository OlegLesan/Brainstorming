using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float range = 3f;
    private Collider[] colliderInRange;
    public LayerMask whatIsEnemy;
    public List<EnemyControler> enemiesInRange = new List<EnemyControler>();
    private float checkCounter;
    public float checkTime = .2f;

    
    public bool enemiesUpdated;
    public GameObject rangeModel;

    public int cost = 100;

    void Start()
    {
        checkCounter = checkTime;
    }


    void Update()
    {
        rangeModel.SetActive(false);
        enemiesUpdated = false;
        checkCounter -= Time.deltaTime;
        if (checkCounter <= 0)
        {
            colliderInRange = Physics.OverlapSphere(transform.position, range, whatIsEnemy);

            enemiesInRange.Clear();
            foreach (Collider col in colliderInRange)
            {
                enemiesInRange.Add(col.GetComponent<EnemyControler>());
            }
            enemiesUpdated = true;
        }
    }
}