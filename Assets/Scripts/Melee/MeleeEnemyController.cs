using System.Collections;
using UnityEngine;

public class MeleeEnemyController : MonoBehaviour
{
    public float moveSpeed = 2f; // Скорость движения
    public float attackRange = 1.5f; // Радиус атаки
    public float detectionRange = 5f; // Радиус обнаружения юнита
    public float attackCooldown = 1f; // Время между атаками
    public float damage = 10f; // Наносимый урон

    private Animator animator; // Для управления анимацией
    private Transform targetUnit; // Цель (юнит)
    private Vector3 savedTargetPosition; // Сохранённая точка пути
    private bool isAttacking = false; // Идёт ли атака
    private bool isEngaged = false; // Враг занят боем
    private bool reachedEnd = false; // Достиг ли конца пути
    private Path thePath; // Путь врага
    private int currentPoint = 0; // Текущая точка пути
    private EnemyPool enemyPool;

    private EnemyHealthController healthController; // Ссылка на EnemyHealthController

    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyPool = FindObjectOfType<EnemyPool>();
        healthController = GetComponent<EnemyHealthController>();

        if (healthController == null)
        {
            Debug.LogError("EnemyHealthController не найден на враге!");
        }

        
    }

    private void Update()
    {
        if (reachedEnd || isAttacking || healthController == null || healthController.IsDead()) return;

        if (!isEngaged && targetUnit == null)
        {
            FindUnitInRange();
        }

        if (targetUnit != null)
        {
            EngageUnit();
        }
        else
        {
            ResumeSavedPath();
        }
    }

    private void FindUnitInRange()
    {
        if (thePath == null || thePath.points == null || thePath.points.Length == 0)
        {
            Debug.LogWarning($"Path отсутствует или пуст у объекта {gameObject.name}!");
            return;
        }

        Collider[] unitsInRange = Physics.OverlapSphere(transform.position, detectionRange, LayerMask.GetMask("Unit"));

        Debug.Log($"Units found in range: {unitsInRange.Length}");

        if (unitsInRange.Length > 0)
        {
            if (!isEngaged)
            {
                if (thePath.points == null || currentPoint >= thePath.points.Length)
                {
                    Debug.LogError("Текущая точка пути недействительна!");
                    return;
                }
                savedTargetPosition = thePath.points[currentPoint].position;
            }

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
                isEngaged = true;
                if (animator != null)
                {
                    animator.SetBool("IsMoving", true);
                }
            }
        }
    }

    private void EngageUnit()
    {
        if (targetUnit == null)
        {
            isEngaged = false;
            return;
        }

        Vector3 direction = (targetUnit.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction);

        float distance = Vector3.Distance(transform.position, targetUnit.position);

        if (distance > attackRange)
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
            animator?.SetBool("IsMoving", true);
        }
        else
        {
            animator?.SetBool("IsMoving", false);
            if (!isAttacking)
            {
                StartCoroutine(AttackUnit());
            }
        }
    }

    private IEnumerator AttackUnit()
    {
        isAttacking = true;
        animator?.SetTrigger("Attack");

        yield return new WaitForSeconds(attackCooldown);

        if (targetUnit != null)
        {
            MeleeUnitController unitController = targetUnit.GetComponent<MeleeUnitController>();
            if (unitController != null)
            {
                unitController.TakeDamage(damage);

                if (unitController.IsDead())
                {
                    targetUnit = null;
                    isEngaged = false;
                }
            }
        }

        isAttacking = false;
    }

    private void ResumeSavedPath()
    {
        if (thePath == null || thePath.points.Length == 0) return;

        Vector3 direction = (savedTargetPosition - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(direction);

        if (Vector3.Distance(transform.position, savedTargetPosition) < 0.1f)
        {
            currentPoint++;
            if (currentPoint >= thePath.points.Length)
            {
                reachedEnd = true;
                DealDamageToBase();
            }
            else
            {
                savedTargetPosition = thePath.points[currentPoint].position;
            }
        }

        animator?.SetBool("IsMoving", true);
    }

    private void DealDamageToBase()
    {
        Base baseComponent = FindObjectOfType<Base>();
        if (baseComponent != null)
        {
            baseComponent.TakeDamage(damage);
        }

        if (enemyPool != null)
        {
            enemyPool.ReturnEnemy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (healthController != null)
        {
            healthController.TakeDamage(damageAmount, "Melee");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void Setup(Path path)
    {
        thePath = path;
        currentPoint = 0;
        reachedEnd = false;
    }
}
