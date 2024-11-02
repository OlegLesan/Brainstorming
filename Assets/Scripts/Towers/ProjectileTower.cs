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

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        theTower = GetComponent<Tower>();
        animator = GetComponent<Animator>();
        animator.speed = animationSpeed;
    }

    void Update()
    {
        if (target != null)
        {
            animator.SetBool(isShooting, true);
            launcherModel.rotation = Quaternion.Slerp(launcherModel.rotation, Quaternion.LookRotation(target.position - transform.position), rotateSpeed * Time.deltaTime);
            launcherModel.rotation = Quaternion.Euler(0f, launcherModel.rotation.eulerAngles.y, 0f);
            firePoint.LookAt(target);
        }
        else
        {
            animator.SetBool(isShooting, false);
        }

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
