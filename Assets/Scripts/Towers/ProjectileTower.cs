using System.Collections.Generic;
using UnityEngine;

public class ProjectileTower : MonoBehaviour
{
    
    private Tower theTower;
    private Transform target;
    private SoundPlayer soundPlayer;
    public Transform firePoint;
    public Transform launcherModel;
    public float rotateSpeed;
    public string projectileTag;

    private Animator animator;
    private static readonly int isShooting = Animator.StringToHash("IsShooting");
    public float animationSpeed = 1f;
    private Vector3 initialLauncherPosition;

    [Tooltip("Прицеливаться в самого дальнего врага вместо ближайшего")]
    public bool targetFarthestEnemy = false; // Новый булево поле для переключения цели

    void Awake()
    {
        soundPlayer = GetComponent<SoundPlayer>();
    }

    void Start()
    {
        
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
        soundPlayer.PlaySound(0);
        if (target != null && !string.IsNullOrEmpty(projectileTag))
        {
            
            GameObject projectile = ProjectilePoolManager.instance.GetProjectile(projectileTag);

            if (projectile != null)
            {
                projectile.transform.position = firePoint.position;
                projectile.transform.rotation = firePoint.rotation;
                projectile.SetActive(true);

                // Проверяем, какой тип снаряда мы получили
                BazokaProjectile bazokaProjectile = projectile.GetComponent<BazokaProjectile>();
                if (bazokaProjectile != null)
                {
                    bazokaProjectile.SetTarget(target);
                    bazokaProjectile.SetTower(this);
                }
                else
                {
                    MolotovProjectile molotovProjectile = projectile.GetComponent<MolotovProjectile>();
                    if (molotovProjectile != null)
                    {
                        molotovProjectile.SetTarget(target);
                        molotovProjectile.SetTower(this);
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
                }
            }
        }
    }

    public void ReturnProjectileToPool(GameObject projectile)
    {
        if (!string.IsNullOrEmpty(projectileTag))
        {
            ProjectilePoolManager.instance.ReturnProjectile(projectileTag, projectile);
        }
    }
}
