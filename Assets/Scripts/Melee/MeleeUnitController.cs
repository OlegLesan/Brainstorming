using System.Collections;
using UnityEngine;

public class MeleeUnitController : MonoBehaviour
{
    public float attackRange = 1.5f; // Радиус атаки
    public float attackCooldown = 1f; // Время между атаками
    public float damage = 15f; // Наносимый урон
    public float health = 100f; // Здоровье юнита

    private Vector3 startPosition; // Начальная позиция юнита
    private Animator animator;
    private Transform targetEnemy; // Цель (враг)
    private bool isAttacking = false; // Идёт ли атака
    private bool isEngaged = false; // Юнит занят боем
    private bool isDead = false;

    private EnemyPool unitPool;

    private void Start()
    {
        animator = GetComponent<Animator>();
        unitPool = FindObjectOfType<EnemyPool>();
        startPosition = transform.position; // Сохраняем начальную позицию юнита
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
            // Получаем компонент EnemyHealthController
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
        transform.position = startPosition; // Возвращаем юнита на начальную позицию
        isEngaged = false;
    }

    public bool IsDead()
    {
        return isDead;
    }

    private void OnDrawGizmosSelected()
    {
        // Отображение радиуса атаки в редакторе Unity
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
