using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControler : MonoBehaviour
{
    public float speedMod = 1f; // Скорость движения, которая может быть изменена для замедления
    public float rotationSpeed = 5f;
    public Transform target;
    internal Path thePath; // Убрали public, так как он будет находиться в Start
    private int currentPoint = 0;
    private bool reachedEnd;

    public float damage = 5;
    private Base theBase;
    private AudioSource audioSource;
    private EnemyHealthController healthController;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // Найдём объект Path в сцене, если он ещё не установлен
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

        healthController = GetComponent<EnemyHealthController>();
        SetRandomTargetFromPoint(currentPoint);
    }

    void Update()
    {
        if (LevelManager.instance.levelActive && !reachedEnd && target != null && healthController.totalHealth > 0)
        {
            MoveAlongPath();
        }
    }

    private void MoveAlongPath()
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Перемещаем врага с учетом скорости, управляемой только speedMod
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
            SetRandomTargetFromPoint(currentPoint);
        }
    }
}
