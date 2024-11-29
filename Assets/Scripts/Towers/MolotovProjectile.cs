using UnityEngine;

public class MolotovProjectile : MonoBehaviour
{
    public Rigidbody theRB;
    public float moveSpeed;
    public float damagePerSecond; // ���� � �������
    public float fireDuration;    // ������������ ����
    public string damageType;
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
        Debug.Log($"{gameObject.name} ������� ����: {target?.name}");
    }

    private void OnEnable()
    {
        hasCollided = false;
        theRB.velocity = Vector3.zero; // ���������� ��������
    }

    private void Update()
    {
        if (target != null && !hasCollided)
        {
            Debug.Log($"{gameObject.name}: ����� � {target.name}");
            Vector3 direction = (target.position - transform.position).normalized;
            theRB.velocity = direction * moveSpeed;

            if (Vector3.Distance(transform.position, target.position) < 0.5f)
            {
                ImpactTarget();
            }
        }
        else if (target == null)
        {
           
            ReturnToPool();
        }
    }

    private void ImpactTarget()
    {
        hasCollided = true;

        if (impactEffect != null)
        {
            GameObject impact = Instantiate(impactEffect, transform.position, Quaternion.identity);
            MolotovFireEffect fireEffect = impact.GetComponent<MolotovFireEffect>();
            if (fireEffect != null)
            {
                fireEffect.SetDamageAmount(damagePerSecond, damageType);
                fireEffect.SetDuration(fireDuration);
            }
        }

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        if (tower != null)
        {
            hasCollided = false;
            theRB.velocity = Vector3.zero; // ���������� ��������
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
