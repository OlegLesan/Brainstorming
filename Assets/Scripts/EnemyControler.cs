using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControler : MonoBehaviour
{
    public float speedMod = 1f; // Скорость движения
    public float rotationSpeed = 5f;
    public Transform target;
    internal Path thePath;
    private int currentPoint = 0;
    private bool reachedEnd;

    public float damage = 5;
    private Base theBase;
    private AudioSource audioSource;

    private float initialSpeedMod; // Для сохранения изначальной скорости

    void Awake()
    {
        initialSpeedMod = speedMod; // Сохраняем начальное значение скорости
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }

        theBase = FindObjectOfType<Base>();

        if (thePath == null)
        {
            thePath = FindObjectOfType<Path>();
        }

        if (thePath != null && thePath.points.Length > 0)
        {
            SetRandomTargetFromPoint(currentPoint);
        }
        else
        {
            Debug.LogError("Путь не найден или не содержит точек!");
        }
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
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.position = Vector3.MoveTowards(transform.position, target.position, speedMod * Time.deltaTime);

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
                SetRandomTargetFromPoint(currentPoint);
            }
        }
    }

    private void SetRandomTargetFromPoint(int pointIndex)
    {
        if (pointIndex < thePath.points.Length)
        {
            Transform pathPoint = thePath.points[pointIndex];
            if (pathPoint.childCount > 0)
            {
                int randomChildIndex = Random.Range(0, pathPoint.childCount);
                target = pathPoint.GetChild(randomChildIndex);
            }
            else
            {
                target = pathPoint;
            }
        }
    }

    private void DealDamageToBase()
    {
        if (theBase != null)
        {
            theBase.TakeDamage(damage);
        }

        // Удаляем врага из списка активных врагов
        LevelManager.instance.RemoveEnemyFromActiveList(GetComponent<EnemyHealthController>());

        Destroy(gameObject);
    }

    public void Setup(Path newPath)
    {
        thePath = newPath;
        currentPoint = 0;
        reachedEnd = false;

        // Восстанавливаем изначальную скорость
        speedMod = initialSpeedMod;

        if (thePath.points.Length > 0)
        {
            SetRandomTargetFromPoint(currentPoint);
        }
    }

    public void StopMoving()
    {
        speedMod = 0; // Останавливаем движение
    }
}
