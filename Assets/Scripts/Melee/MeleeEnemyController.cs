using System.Collections;
using UnityEngine;

public class MeleeEnemyController : MonoBehaviour
{
    public float attackRange = 1.5f; // Радиус атаки
    public float detectionRange = 5f; // Радиус обнаружения юнита
    public float damage = 10f; // Наносимый урон
    public float moveSpeed = 2f; // Скорость движения

    private Animator animator;
    private Transform targetUnit; // Цель (юнит)
    private bool isAttacking = false; // В процессе атаки
    private bool isDead = false;
    private EnemyControler enemyControler; // Ссылка на EnemyControler
    private EnemyHealthController healthController; // Ссылка на EnemyHealthController

    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyControler = GetComponent<EnemyControler>();
        healthController = GetComponent<EnemyHealthController>();

        if (healthController == null)
        {
            Debug.LogError("EnemyHealthController не найден на объекте " + gameObject.name);
        }
    }

    private void Update()
    {
        if (isDead) return;

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
            enemyControler.enabled = true; // Включаем EnemyControler, если цель потеряна
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

                // Выключаем EnemyControler при обнаружении юнита
                if (enemyControler != null && enemyControler.enabled)
                {
                    enemyControler.enabled = false;
                }
            }
        }
    }

    private void EngageUnit()
    {
        if (targetUnit == null)
        {
            if (enemyControler != null && !enemyControler.enabled)
            {
                enemyControler.enabled = true; // Включаем EnemyControler
            }
            return;
        }

        Vector3 direction = (targetUnit.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction);

        float distance = Vector3.Distance(transform.position, targetUnit.position);
        if (distance > attackRange)
        {
            // Сближаемся с целью
            transform.position += direction * moveSpeed * Time.deltaTime;
            animator?.SetBool("IsAttacking", false);
            animator?.SetBool("IsMoving", true);
        }
        else
        {
            // В радиусе атаки
            animator?.SetBool("IsMoving", false);
            animator?.SetBool("IsAttacking", true);
        }
    }

    // Этот метод вызывается из Animation Event
    private void DealDamage()
    {
        if (targetUnit == null) return;

        MeleeUnitController unitController = targetUnit.GetComponent<MeleeUnitController>();
        if (unitController != null)
        {
            unitController.TakeDamage(damage);

            if (unitController.IsDead())
            {
                targetUnit = null; // Уничтожаем цель
                animator?.SetBool("IsAttacking", false);
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
                Die();
            }
        }
    }

    private void Die()
    {
        isDead = true;
        animator?.SetBool("IsAttacking", false);
        animator?.SetBool("IsMoving", false);
        animator?.SetTrigger("Death");
        StartCoroutine(ReturnToPoolAfterDelay());
    }

    private IEnumerator ReturnToPoolAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        if (enemyControler != null)
        {
            enemyControler.enabled = true; // Включаем EnemyControler перед возвратом в пул
        }
        Destroy(gameObject); // Или возвращаем в пул, если используется пул объектов
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
