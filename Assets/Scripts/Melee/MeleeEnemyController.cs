using System.Collections;
using UnityEngine;

public class MeleeEnemyController : MonoBehaviour
{
    public float attackRange = 1.5f; // ������ �����
    public float detectionRange = 5f; // ������ ����������� �����
    public float damage = 10f; // ��������� ����
    public float moveSpeed = 2f; // �������� ��������

    private Animator animator;
    private Transform targetUnit; // ���� (����)
    private bool isDead = false; // ���� ������
    private EnemyControler enemyControler; // ������ �� EnemyControler
    private EnemyHealthController healthController; // ������ �� EnemyHealthController

    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyControler = GetComponent<EnemyControler>();
        healthController = GetComponent<EnemyHealthController>();

        if (healthController == null)
        {
            Debug.LogError("EnemyHealthController �� ������ �� ������� " + gameObject.name);
        }
    }

    private void Update()
    {
        if (isDead) return;

        // ���������, ���� ���� �����, ��������� �������� ������
        if (healthController != null && healthController.IsDead())
        {
            TriggerDeathAnimation();
            return;
        }

        // ���������, ������� �� ����
        if (targetUnit != null && !targetUnit.gameObject.activeInHierarchy)
        {
            targetUnit = null; // ���������� ����, ���� ��� ����������
            animator?.SetBool("IsAttacking", false);

            // �������� EnemyController
            if (enemyControler != null && !enemyControler.enabled)
            {
                enemyControler.enabled = true;
            }
        }

        if (targetUnit == null)
        {
            FindUnitInRange();
        }

        if (targetUnit != null)
        {
            EngageUnit();
        }
        else if (enemyControler != null && !enemyControler.enabled)
        {
            enemyControler.enabled = true; // �������� EnemyController, ���� ���� ��������
        }
    }

    private void FindUnitInRange()
    {
        Collider[] unitsInRange = Physics.OverlapSphere(transform.position, detectionRange, LayerMask.GetMask("Unit"));

        if (unitsInRange.Length > 0)
        {
            float closestDistance = Mathf.Infinity;
            Transform closestUnit = null;

            foreach (var unit in unitsInRange)
            {
                float distance = Vector3.Distance(transform.position, unit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestUnit = unit.transform;
                }
            }

            if (closestUnit != null)
            {
                targetUnit = closestUnit;

                // ��������� EnemyController ��� ����������� �����
                if (enemyControler != null && enemyControler.enabled)
                {
                    enemyControler.enabled = false;
                }
            }
        }
        else
        {
            // �������� EnemyController, ���� ����� �� �������
            if (enemyControler != null && !enemyControler.enabled)
            {
                enemyControler.enabled = true;
            }
        }
    }

    private void EngageUnit()
    {
        if (targetUnit == null)
        {
            if (enemyControler != null && !enemyControler.enabled)
            {
                enemyControler.enabled = true; // �������� EnemyController
            }
            return;
        }

        // ���������, ����������� �� ���� ���� "Unit"
        if (!IsTargetInLayer(targetUnit.gameObject, LayerMask.GetMask("Unit")))
        {
            targetUnit = null; // ���������� ����, ���� ��� �� �� ���� "Unit"
            if (enemyControler != null && !enemyControler.enabled)
            {
                enemyControler.enabled = true; // �������� EnemyController
            }
            return;
        }

        Vector3 direction = (targetUnit.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction);

        float distance = Vector3.Distance(transform.position, targetUnit.position);
        if (distance > attackRange)
        {
            // ���������� � �����
            transform.position += direction * moveSpeed * Time.deltaTime;
            animator?.SetBool("IsAttacking", false);
            animator?.SetBool("IsMoving", true);
        }
        else
        {
            // � ������� �����
            animator?.SetBool("IsMoving", false);
            animator?.SetBool("IsAttacking", true);
        }
    }

    private bool IsTargetInLayer(GameObject target, LayerMask layerMask)
    {
        return (layerMask.value & (1 << target.layer)) != 0;
    }

    private void DealDamage()
    {
        if (targetUnit == null) return;

        MeleeUnitController unitController = targetUnit.GetComponent<MeleeUnitController>();
        if (unitController != null)
        {
            unitController.TakeDamage(damage);

            if (unitController.IsDead())
            {
                targetUnit = null;
                animator?.SetBool("IsAttacking", false);

                if (enemyControler != null && !enemyControler.enabled)
                {
                    enemyControler.enabled = true;
                }
            }
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (healthController != null)
        {
            healthController.TakeDamage(damageAmount, "Melee");

            if (healthController.IsDead())
            {
                TriggerDeathAnimation();
            }
        }
        else
        {
            Debug.LogError("EnemyHealthController �� ������ �� ������� " + gameObject.name);
        }
    }

    private void TriggerDeathAnimation()
    {
        if (isDead) return;

        isDead = true;

        Debug.Log("���� �����: " + gameObject.name);

        if (animator != null)
        {
            animator.SetBool("IsAttacking", false);
            animator.SetBool("IsMoving", false);
            animator.SetBool("IsDead", true);
        }
        else
        {
            Debug.LogWarning("�������� ����������� �� ������� " + gameObject.name);
        }

        targetUnit = null;

        if (enemyControler != null && !enemyControler.enabled)
        {
            enemyControler.enabled = true;
        }
    }

    // ���� ����� ���������� ������������ �������
    public void Die()
    {
        Debug.Log("���� ��������� ���������: " + gameObject.name);
        StartCoroutine(ReturnToPoolAfterDelay());
    }

    private IEnumerator ReturnToPoolAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject); // ��� ������� ������ � ���
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
