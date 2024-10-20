using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyControler : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed = 5f; // скорость поворота
    public Transform target;
    private Path thePath;
    private int currentPoint;
    private bool reachedEnd;

    public float damage = 5;
    private Base theBase;

    void Start()
    {
        if (thePath == null)
        {
            thePath = FindObjectOfType<Path>();
        }
        theBase = FindObjectOfType<Base>();
    }

    void Update()
    {
        if (!reachedEnd)
        {
            // Цель для поворота
            Vector3 directionToTarget = (thePath.points[currentPoint].position - transform.position).normalized;

            // Плавный поворот с использованием Quaternion
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Перемещение к следующей точке
            transform.position = Vector3.MoveTowards(transform.position, thePath.points[currentPoint].position, moveSpeed * Time.deltaTime);

            // Проверка, достигли ли мы текущей точки
            if (Vector3.Distance(transform.position, thePath.points[currentPoint].position) < 0.01f)
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

    public void Setup(Path newPath)
    {
        thePath = newPath;
    }
}
