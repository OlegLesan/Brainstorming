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
    private int currentPoint = 0; // Инициализация с первой точки
    private bool reachedEnd;

    public float damage = 5;
    private Base theBase;

    private AudioSource audioSource; // Добавляем AudioSource переменную
    private EnemyHealthController healthController; // Ссылка на контроллер здоровья

    void Start()
    {
        // Инициализация AudioSource и запуск звука
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }

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

        // Получаем ссылку на EnemyHealthController
        healthController = GetComponent<EnemyHealthController>();
    }

    void Update()
    {
        // Проверка, чтобы враг двигался только если он жив
        if (LevelManager.instance.levelActive && !reachedEnd && target != null && healthController.totalHealth > 0)
        {
            MoveAlongPath();
        }
    }

    private void MoveAlongPath()
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * speedMod);
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime * speedMod);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            currentPoint++;

            if (currentPoint >= thePath.points.Length)
            {
                reachedEnd = true;
                DealDamageToBase();
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
            target = thePath.points[currentPoint];
        }
        else
        {
            Debug.LogWarning("Все точки пути пройдены.");
        }
    }

    private void DealDamageToBase()
    {
        theBase.TakeDamage(damage);
        Destroy(gameObject);
    }

    public void Setup(Path newPath)
    {
        thePath = newPath;
        currentPoint = 0;
        if (thePath.points.Length > 0)
        {
            target = thePath.points[currentPoint];
        }
    }
}
