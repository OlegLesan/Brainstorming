using System.Collections;
using UnityEngine;

public class MeleeUnitController : MonoBehaviour
{
    public float attackRange = 1.5f; // ������ �����
    public float attackCooldown = 1f; // ����� ����� �������
    public float damage = 15f; // ��������� ����
    public float health = 100f; // �������� �����

    private Vector3 startPosition; // ��������� ������� �����
    private Animator animator;
    private Transform targetEnemy; // ���� (����)
    private bool isAttacking = false; // ��� �� �����
    private bool isEngaged = false; // ���� ����� ����
    private bool isDead = false;

    private EnemyPool unitPool;

    private void Start()
    {
        animator = GetComponent<Animator>();
        unitPool = FindObjectOfType<EnemyPool>();
        startPosition = transform.position; // ��������� ��������� ������� �����
    }

    private void Update()
    {
        if (isDead || isAttacking) return;

        if (!isEngaged && targetEnemy == null)
        {
            FindEnemyInRange();
        }

        if (targetEnemy != null)
        {
            EngageEnemy();
        }
    }

    private void FindEnemyInRange()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, attackRange, LayerMask.GetMask("Enemy"));

        if (enemiesInRange.Length > 0)
        {
            targetEnemy = enemiesInRange[0].transform;
            isEngaged = true;
        }
    }

    private void EngageEnemy()
    {
        if (targetEnemy == null) return;

        Vector3 direction = (targetEnemy.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction);

        float distance = Vector3.Distance(transform.position, targetEnemy.position);
        if (distance > attackRange)
        {
            animator?.SetBool("IsMoving", false);
        }
        else
        {
            StartCoroutine(AttackEnemy());
        }
    }

    private IEnumerator AttackEnemy()
    {
        isAttacking = true;
        animator?.SetTrigger("Attack");

        yield return new WaitForSeconds(attackCooldown);

        if (targetEnemy != null)
        {
            // �������� ��������� EnemyHealthController
            EnemyHealthController enemyHealth = targetEnemy.GetComponent<EnemyHealthController>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage, "Melee");

                if (enemyHealth.IsDead())
                {
                    ResetUnit();
                }
            }
        }

        isAttacking = false;
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        animator?.SetTrigger("Death");
        StartCoroutine(ReturnToPoolAfterDelay());
    }

    private IEnumerator ReturnToPoolAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        unitPool.ReturnEnemy(gameObject);
    }

    private void ResetUnit()
    {
        targetEnemy = null;
        transform.position = startPosition; // ���������� ����� �� ��������� �������
        isEngaged = false;
    }

    public bool IsDead()
    {
        return isDead;
    }

    private void OnDrawGizmosSelected()
    {
        // ����������� ������� ����� � ��������� Unity
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
