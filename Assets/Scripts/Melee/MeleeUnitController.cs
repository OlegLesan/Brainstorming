using UnityEngine;
using UnityEngine.UI;

public class MeleeUnitController : MonoBehaviour
{
    public float detectionRange = 5f; // ������ ����������� ������
    public float attackRange = 1.5f; // ������ �����
    public float moveSpeed = 3f; // �������� ��������
    public float damage = 15f; // ��������� ����
    public float health = 100f; // �������� �����
    public Slider healthBar; // ������� ��������

    private Animator animator;
    private Transform targetEnemy; // ���� (����)
    private bool isEngaged = false; // ���� ��������� � ���
    private bool isDead = false;

    private Camera mainCamera;
    private MeleePlacementController poolController; // ���������� ����

    private void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;

        if (healthBar != null)
        {
            healthBar.maxValue = health;
            healthBar.value = health;
            healthBar.gameObject.SetActive(false); // ������� ������, ���� �� ������� ����
        }

        // ����� ����������� ����
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
            animator?.SetBool("IsMoving", true); // �������� ��������
        }
    }

    private void MoveTowardsEnemy()
    {
        if (targetEnemy == null) return;

        // ������������ ���������� �� �����
        float distanceToEnemy = Vector3.Distance(transform.position, targetEnemy.position);

        if (distanceToEnemy > attackRange)
        {
            // �������� � �����
            Vector3 direction = (targetEnemy.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            // ������������ ����� � ������� �����
            transform.rotation = Quaternion.LookRotation(direction);

            animator?.SetBool("IsMoving", true);
            animator?.SetBool("IsAttacking", false);
        }
        else
        {
            // ������������� �������� � �������
            animator?.SetBool("IsMoving", false);
            animator?.SetBool("IsAttacking", true);
        }
    }

    // ���� ����� ���������� �� Animation Event
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
        if (isDead) return; // ���� ��� �����, ���� �� ���������

        health -= damageAmount;

        if (healthBar != null)
        {
            healthBar.value = health;
            healthBar.gameObject.SetActive(true); // ���������� ������� ��������
        }

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return; // �������� ���������� ������

        isDead = true;
        health = 0; // ������������� �������� � 0 �� ������ ������

        // ����� ���������� ��������������
        targetEnemy = null;
        isEngaged = false;

        // ���������� ����������, ����� ����� ��������� ���������
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // ��������� ��� �������� ����� "Death"
        animator?.SetBool("IsMoving", false);
        animator?.SetBool("IsAttacking", false);

        // ����� ��������� �������� ������
        animator?.SetTrigger("Death");

        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(false); // �������� ������� �������� ��� ������
        }
    }

    public bool IsDead()
    {
        return isDead;
    }

    // ���� ����� ���������� � ����� �������� ������ (Animation Event)
    private void ReturnToPool()
    {
        if (poolController != null)
        {
            poolController.ReturnUnitToPool(gameObject);
        }

        // ����� ��������� ����� ��������� � ���
        isDead = false;
        health = 100f; // ����� ��������
        if (healthBar != null)
        {
            healthBar.value = healthBar.maxValue;
            healthBar.gameObject.SetActive(false);
        }

        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = true; // �������� ��������� ��� ���������� �������������
        }
    }

    private void UpdateHealthBarRotation()
    {
        if (healthBar == null || mainCamera == null) return;

        // ������� �������� ������ ������� �� ������
        Vector3 direction = mainCamera.transform.position - healthBar.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(-direction);
        healthBar.transform.rotation = Quaternion.Lerp(healthBar.transform.rotation, targetRotation, Time.deltaTime * 10f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange); // ���������� ������ �����������
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange); // ���������� ������ �����
    }
}
