using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControler : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed = 5f;
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

        // Получаем случайную дочернюю точку для первой точки пути
        target = thePath.points[currentPoint];
        SetNewTarget();
    }

    void Update()
    {
        if (LevelManager.instance.levelActive)
        {
            if (!reachedEnd)
            {
                // Направление к текущей цели
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                // Вращение к цели
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                // Движение к цели
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

                // Проверка достижения цели
                if (Vector3.Distance(transform.position, target.position) < 0.01f)
                {
                    currentPoint++;

                    if (currentPoint >= thePath.points.Length)
                    {
                        reachedEnd = true;
                        DealDamageToBase(); // Нанести урон базе при достижении конца пути
                    }
                    else
                    {
                        SetNewTarget();
                    }
                }
            }
        }
    }

    private void SetNewTarget()
    {
        target = thePath.GetRandomChildPoint(thePath.points[currentPoint]);
    }

    private void DealDamageToBase()
    {
        theBase.TakeDamage(damage); // Наносим урон базе
        Destroy(gameObject); // Удаляем врага после нанесения урона
    }

    public void Setup(Path newPath)
    {
        thePath = newPath;
    }
}
