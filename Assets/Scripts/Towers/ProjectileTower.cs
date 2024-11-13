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
    public string projectileTag;

    private Animator animator;
    private static readonly int isShooting = Animator.StringToHash("IsShooting");
    public float animationSpeed = 1f;
    private Vector3 initialLauncherPosition;

    [Tooltip("Прицеливаться в самого дальнего врага вместо ближайшего")]
    public bool targetFarthestEnemy = false; // Новый булево поле для переключения цели

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        theTower = GetComponent<Tower>();
        animator = GetComponent<Animator>();
        animator.speed = animationSpeed;
        initialLauncherPosition = launcherModel.position;
    }

    void Update()
    {
        if (target != null)
        {
            animator.SetBool(isShooting, true);
            Quaternion targetRotation = Quaternion.LookRotation(target.position - launcherModel.position);
            targetRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
            launcherModel.rotation = Quaternion.Slerp(launcherModel.rotation, targetRotation, rotateSpeed * Time.deltaTime);

            firePoint.LookAt(target);
        }
        else
        {
            animator.SetBool(isShooting, false);
        }

        launcherModel.position = initialLauncherPosition;

        if (theTower.enemiesInRange.Count > 0)
        {
            float bestDistance = targetFarthestEnemy ? 0f : theTower.range + 1f; // Начальное значение в зависимости от режима
            Transform bestTarget = null;

            foreach (EnemyControler enemy in theTower.enemiesInRange)
            {
                if (enemy != null)
                {
                    float distance = Vector3.Distance(transform.position, enemy.transform.position);

                    // Условие на выбор цели в зависимости от настройки
                    if ((targetFarthestEnemy && distance > bestDistance) || (!targetFarthestEnemy && distance < bestDistance))
                    {
                        bestDistance = distance;
                        bestTarget = enemy.transform;
                    }
                }
            }
            target = bestTarget;
        }
        else
        {
            target = null;
        }
    }

    public void FireProjectile()
    {
        if (target != null && !string.IsNullOrEmpty(projectileTag))
        {
            audioSource.Play();
            GameObject projectile = ProjectilePoolManager.instance.GetProjectile(projectileTag);

            if (projectile != null)
            {
                projectile.transform.position = firePoint.position;
                projectile.transform.rotation = firePoint.rotation;
                projectile.SetActive(true);

                BazokaProjectile bazokaProjectile = projectile.GetComponent<BazokaProjectile>();
                if (bazokaProjectile != null)
                {
                    bazokaProjectile.SetTarget(target);
                    bazokaProjectile.SetTower(this);
                }
                else
                {
                    Projectile projectileScript = projectile.GetComponent<Projectile>();
                    if (projectileScript != null)
                    {
                        projectileScript.SetTarget(target);
                        projectileScript.projectileTower = this;
                    }
                }

                Instantiate(shotEffect, firePoint.position, firePoint.rotation);
            }
        }
    }

    public void ReturnProjectileToPool(GameObject projectile)
    {
        if (projectileTag != null)
        {
            ProjectilePoolManager.instance.ReturnProjectile(projectileTag, projectile);
        }
    }
}
