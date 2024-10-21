using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTower : MonoBehaviour
{
    private DefencePeople theTower;

    public GameObject projectile;
    public Transform firePoint;  // Точка выстрела
    public float timeBetweenShots = 1f;
    private float shotCounter;

    private Transform target;
    public Transform launcherModel;  // Модель башни

    void Start()
    {
        theTower = GetComponent<DefencePeople>();
    }

    void Update()
    {
        // Проверяем, что у нас есть цель
        if (target != null)
        {
            // Плавный поворот модели башни к цели
            Vector3 direction = target.position - launcherModel.position;
            launcherModel.rotation = Quaternion.Slerp(
                launcherModel.rotation,
                Quaternion.LookRotation(direction, Vector3.up),
                5f * Time.deltaTime
            );
            launcherModel.rotation = Quaternion.Euler(0f, launcherModel.rotation.eulerAngles.y, 0f);
        }


        shotCounter -= Time.deltaTime;
        
        if (shotCounter <= 0 && target != null && firePoint != null)
        {
            shotCounter = timeBetweenShots;

            if (firePoint != null && target != null)
            {
                firePoint.LookAt(target);  // Поворачиваем точку выстрела к цели
                Instantiate(projectile, firePoint.position, firePoint.rotation);  // Создаем снаряд
            }
        }
        if (theTower.enemiesUpdated)
        {
            // Поиск ближайшего врага в радиусе
            if (theTower.enemiesInRange.Count > 0)
            {
                float minDistance = theTower.range + 1f;
                foreach (EnemyControler enemy in theTower.enemiesInRange)
                {
                    if (enemy != null)
                    {
                        float distance = Vector3.Distance(transform.position, enemy.transform.position);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            target = enemy.transform;  // Устанавливаем цель на ближайшего врага
                        }
                    }
                }
            }
        }
        else
        {
            target = null;  // Если врагов нет, сбрасываем цель
        }
    }
}
