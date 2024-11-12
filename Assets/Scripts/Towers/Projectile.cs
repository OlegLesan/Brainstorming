using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody theRB;
    public float moveSpeed;
    public float damageAmount;
    public string damageType; // Тип урона для проверки защиты
    public GameObject impactEffect;

    private bool hasDamaged;
    public ProjectileTower projectileTower;
    private Transform target;

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
            target = other.transform;
            DamageTarget();
        }
    }

    private void DamageTarget()
    {
        if (target == null) return;

        EnemyHealthController enemyHealth = target.GetComponent<EnemyHealthController>();
        if (enemyHealth != null && !hasDamaged)
        {
            enemyHealth.TakeDamage(damageAmount, damageType);
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
