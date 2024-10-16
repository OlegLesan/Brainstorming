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

    void Start()
    {
        thePath = FindObjectOfType<Path>();
    }
    
    void Update()
    {
        if (!reachedEnd)
        {
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

      
    }

}
