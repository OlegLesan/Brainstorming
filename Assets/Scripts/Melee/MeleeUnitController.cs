using UnityEngine;
using UnityEngine.UI;

public class MeleeUnitController : MonoBehaviour
{
    public float detectionRange = 5f; // Радиус обнаружения врагов
    public float attackRange = 1.5f; // Радиус атаки
    public float moveSpeed = 3f; // Скорость движения
    public float damage = 15f; // Наносимый урон
    public float health = 100f; // Здоровье юнита
    public Slider healthBar; // Полоска здоровья

    private Animator animator;
    private Transform targetEnemy; // Цель (враг)
    private bool isEngaged = false; // Юнит находится в бою
    private bool isDead = false;

    private Camera mainCamera;
    private MeleePlacementController poolController; // Контроллер пула

    private void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;

        if (healthBar != null)
        {
            healthBar.maxValue = health;
            healthBar.value = health;
            healthBar.gameObject.SetActive(false); // Полоска скрыта, пока не получен урон
        }

        // Поиск контроллера пула
        poolController = FindObjectOfType<MeleePlacementController>();
    }

    private void Update()
    {
        if (isDead) return;

        if (!isEngaged && targetEnemy == null)
        {
            FindEnemyInDetectionRange();
        }

        if (targetEnemy != null)
        {
            MoveTowardsEnemy();
        }

        UpdateHealthBarRotation();
    }

    private void FindEnemyInDetectionRange()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, detectionRange, LayerMask.GetMask("Enemy"));

        if (enemiesInRange.Length > 0)
        {
            targetEnemy = enemiesInRange[0].transform;
            isEngaged = true;
            animator?.SetBool("IsMoving", true); // Начинаем движение
        }
    }

    private void MoveTowardsEnemy()
    {
        if (targetEnemy == null) return;

        // Рассчитываем расстояние до врага
        float distanceToEnemy = Vector3.Distance(transform.position, targetEnemy.position);

        if (distanceToEnemy > attackRange)
        {
            // Движемся к врагу
            Vector3 direction = (targetEnemy.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            // Поворачиваем юнита в сторону врага
            transform.rotation = Quaternion.LookRotation(direction);

            animator?.SetBool("IsMoving", true);
            animator?.SetBool("IsAttacking", false);
        }
        else
        {
            // Останавливаем движение и атакуем
            animator?.SetBool("IsMoving", false);
            animator?.SetBool("IsAttacking", true);
        }
    }

    // Этот метод вызывается из Animation Event
    private void DealDamage()
    {
        if (targetEnemy == null) return;

        EnemyHealthController enemyHealth = targetEnemy.GetComponent<EnemyHealthController>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage, "Melee");

            if (enemyHealth.IsDead())
            {
                targetEnemy = null;
                isEngaged = false;
                animator?.SetBool("IsAttacking", false);
                animator?.SetBool("IsMoving", false);
            }
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return; // Если уже мертв, урон не принимаем

        health -= damageAmount;

        if (healthBar != null)
        {
            healthBar.value = health;
            healthBar.gameObject.SetActive(true); // Показываем полоску здоровья
        }

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return; // Избегаем повторного вызова

        isDead = true;
        health = 0; // Устанавливаем здоровье в 0 на всякий случай

        // Сброс параметров взаимодействия
        targetEnemy = null;
        isEngaged = false;

        // Отключение коллайдера, чтобы враги перестали атаковать
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // Отключаем все анимации кроме "Death"
        animator?.SetBool("IsMoving", false);
        animator?.SetBool("IsAttacking", false);

        // Сразу запускаем анимацию смерти
        animator?.SetTrigger("Death");

        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(false); // Скрываем полоску здоровья при смерти
        }
    }

    public bool IsDead()
    {
        return isDead;
    }

    // Этот метод вызывается в конце анимации смерти (Animation Event)
    private void ReturnToPool()
    {
        if (poolController != null)
        {
            poolController.ReturnUnitToPool(gameObject);
        }

        // Сброс состояния перед возвратом в пул
        isDead = false;
        health = 100f; // Сброс здоровья
        if (healthBar != null)
        {
            healthBar.value = healthBar.maxValue;
            healthBar.gameObject.SetActive(false);
        }

        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = true; // Включаем коллайдер для следующего использования
        }
    }

    private void UpdateHealthBarRotation()
    {
        if (healthBar == null || mainCamera == null) return;

        // Полоска здоровья всегда смотрит на камеру
        Vector3 direction = mainCamera.transform.position - healthBar.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(-direction);
        healthBar.transform.rotation = Quaternion.Lerp(healthBar.transform.rotation, targetRotation, Time.deltaTime * 10f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange); // Отображаем радиус обнаружения
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange); // Отображаем радиус атаки
    }
}
