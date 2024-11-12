using UnityEngine;

public class BazokaProjectile : MonoBehaviour
{
    public Rigidbody theRB;
    public float moveSpeed;
    public float damageAmount;
    public string damageType; // Переменная для хранения типа урона
    public GameObject impactEffect;

    private ProjectileTower tower;
    private Transform target;
    private bool hasCollided = false;

    public void SetTower(ProjectileTower towerRef)
    {
        tower = towerRef;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void OnEnable()
    {
        hasCollided = false;
    }

    private void Update()
    {
        if (target != null && !hasCollided)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            theRB.velocity = direction * moveSpeed;

            if (Vector3.Distance(transform.position, target.position) < 0.5f)
            {
                ImpactTarget();
            }
        }
        else
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasCollided && other.CompareTag("Enemy"))
        {
            ImpactTarget();
        }
    }

    private void ImpactTarget()
    {
        hasCollided = true;

        // Создаем эффект взрыва
        GameObject impact = Instantiate(impactEffect, transform.position, Quaternion.identity);
        BazokaImpact impactScript = impact.GetComponent<BazokaImpact>();
        if (impactScript != null)
        {
            // Передаем урон и тип урона
            impactScript.SetDamageAmount(damageAmount, damageType);
        }

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        if (tower != null)
        {
            hasCollided = false;
            tower.ReturnProjectileToPool(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        hasCollided = false;
    }
}
