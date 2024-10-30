using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControler : MonoBehaviour
{
    public float moveSpeed;
    public float speedMod = 1f;
    public float rotationSpeed = 5f;
    public Transform target;
    private Path thePath;
    private int currentPoint = 0;  // Инициализация с первой точки
    private bool reachedEnd;

    public float damage = 5;
    private Base theBase;

    void Start()
    {
        // Находим путь, если он не был установлен
        if (thePath == null)
        {
            thePath = FindObjectOfType<Path>();
        }

        theBase = FindObjectOfType<Base>();

        if (thePath == null || thePath.points.Length == 0)
        {
            Debug.LogError("Путь не найден или не содержит точек!");
            return;
        }

        // Устанавливаем первую точку пути
        target = thePath.points[currentPoint];
    }

    void Update()
    {
        if (LevelManager.instance.levelActive && !reachedEnd && target != null)
        {
            MoveAlongPath();
        }
    }

    private void MoveAlongPath()
    {
        // Направление к текущей цели
        Vector3 directionToTarget = (target.position - transform.position).normalized;

        // Вращение к цели
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * speedMod);

        // Движение к цели
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime * speedMod);

        // Проверка достижения цели
        if (Vector3.Distance(transform.position, target.position) < 0.1f)  // Увеличил допуск для достижения цели
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

    private void SetNewTarget()
    {
        if (currentPoint < thePath.points.Length)
        {
            // Устанавливаем следующую точку пути
            target = thePath.points[currentPoint];
        }
        else
        {
            Debug.LogWarning("Все точки пути пройдены.");
        }
    }

    private void DealDamageToBase()
    {
        theBase.TakeDamage(damage); // Наносим урон базе
        Destroy(gameObject); // Удаляем врага после нанесения урона
    }

    public void Setup(Path newPath)
    {
        thePath = newPath;
        currentPoint = 0;  // Сброс индекса пути
        if (thePath.points.Length > 0)
        {
            target = thePath.points[currentPoint];  // Установка первой точки
        }
    }
}
