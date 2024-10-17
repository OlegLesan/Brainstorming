using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControler : MonoBehaviour
{
    public float moveSpeed;
    public Transform target;
    private Path thePath;
    private int currentPoint;
    private bool reachedEnd;

    public float damage = 5;
    private Base theBase;

    void Start()
    {
        thePath = FindObjectOfType<Path>();

        theBase = FindObjectOfType<Base>();

        
    }
    
    void Update()
    {
        if (!reachedEnd)
        {
            transform.LookAt(thePath.points[currentPoint]);
            transform.position = Vector3.MoveTowards(transform.position, thePath.points[currentPoint].position, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, thePath.points[currentPoint].position) < .01f)
            {
                currentPoint = currentPoint + 1;
                if (currentPoint >= thePath.points.Length)
                {
                    reachedEnd = true;
                }
            }
        }
        else
        {
            
                theBase.TakeDamage(damage);
            Destroy(gameObject);
        }    

      
    }

}
