using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody theRB;
    public float moveSpeed;
    public float damageAmount;
    public GameObject impactEffect;

    private bool hasDamaged;
    public ProjectileTower projectileTower;
    private Transform target; // ссылка на цель врага

    private void OnEnable()
    {
        hasDamaged = false;
    }

    private void Update()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            theRB.velocity = direction * moveSpeed;

            if (Vector3.Distance(transform.position, target.position) < 0.5f)
            {
                DamageTarget();
            }
        }
        else
        {
            ResetProjectile();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !hasDamaged)
        {
            target = other.transform; // Убедимся, что target установлен перед вызовом DamageTarget
            DamageTarget();
        }
    }

    private void DamageTarget()
    {
        if (target == null) return; // Проверка, чтобы избежать ошибки

        EnemyHealthController enemyHealth = target.GetComponent<EnemyHealthController>();
        if (enemyHealth != null && !hasDamaged)
        {
            enemyHealth.TakeDamage(damageAmount);
            hasDamaged = true;

            if (impactEffect != null)
            {
                Instantiate(impactEffect, transform.position, Quaternion.identity);
            }

            ResetProjectile();
        }
    }

    private void ResetProjectile()
    {
        hasDamaged = false;
        target = null;
        projectileTower.ReturnProjectileToPool(gameObject);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
