using System.Collections.Generic;
using UnityEngine;

public class ProjectileTower : MonoBehaviour
{
    private AudioSource audioSource;
    private Tower theTower;
    private Transform target;

    public Transform firePoint;
    public Transform launcherModel;
    public float rotateSpeed;
    public GameObject shotEffect;

    private Animator animator;
    private static readonly int isShooting = Animator.StringToHash("IsShooting");
    public float animationSpeed = 1f;

    public ProjectilePool projectilePool; // ссылка на пул для снарядов

    private Vector3 initialLauncherPosition; // начальная позиция launcherModel

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        theTower = GetComponent<Tower>();
        animator = GetComponent<Animator>();
        animator.speed = animationSpeed;

        initialLauncherPosition = launcherModel.position; // сохраняем начальную позицию
    }

    void Update()
    {
        if (target != null)
        {
            animator.SetBool(isShooting, true);

            // Поворачиваем launcherModel только по оси Y в сторону цели
            Quaternion targetRotation = Quaternion.LookRotation(target.position - launcherModel.position);
            targetRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
            launcherModel.rotation = Quaternion.Slerp(launcherModel.rotation, targetRotation, rotateSpeed * Time.deltaTime);

            firePoint.LookAt(target);
        }
        else
        {
            animator.SetBool(isShooting, false);
        }

        // Обновляем позицию launcherModel, чтобы она оставалась фиксированной
        launcherModel.position = initialLauncherPosition;

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
                        target = enemy.transform;
                    }
                }
            }
        }
        else
        {
            target = null;
        }
    }

    public void FireProjectile()
    {
        if (projectilePool != null)
        {
            audioSource.Play();
            GameObject projectile = projectilePool.GetProjectile();
            projectile.transform.position = firePoint.position;
            projectile.transform.rotation = firePoint.rotation;

            Instantiate(shotEffect, firePoint.position, firePoint.rotation);
        }
    }

    public void ReturnProjectileToPool(GameObject projectile)
    {
        if (projectilePool != null)
        {
            projectilePool.ReturnProjectile(projectile);
        }
    }
}
