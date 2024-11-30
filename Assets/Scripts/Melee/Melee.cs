using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Melee : MonoBehaviour
{
    public float detectionRadius = 5f; // ������ ����������� ������
    public float attackCooldown = 1f; // �������� ����� �������
    public int damage = 10; // ����
    public Transform weaponPoint; // �����, ������ ��������� ���� (����� ���������������)

    private EnemyHealthController targetEnemy;
    private float attackTimer = 0;
    private NavMeshAgent navAgent;
    private bool inCombat = false;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        attackTimer += Time.deltaTime;

        if (!inCombat)
        {
            SearchForEnemy(); // ���� �����, ���� �� � ���
        }
        else if (targetEnemy != null && !targetEnemy.isDead)
        {
            // ���� ���� ����, ������� �
            FaceTarget();
            if (Vector3.Distance(transform.position, targetEnemy.transform.position) <= navAgent.stoppingDistance)
            {
                AttackEnemy();
            }
        }
        else
        {
            inCombat = false; // ���� ���� ���������, ������� �� ���
        }
    }

    private void SearchForEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (var hitCollider in hitColliders)
        {
            EnemyHealthController enemy = hitCollider.GetComponent<EnemyHealthController>();
            if (enemy != null && !enemy.isDead)
            {
                targetEnemy = enemy;
                inCombat = true;
                navAgent.SetDestination(enemy.transform.position);
                navAgent.stoppingDistance = 1.5f; // ����� ��������� ��� �������� ���
                return;
            }
        }
    }

    private void FaceTarget()
    {
        Vector3 direction = (targetEnemy.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void AttackEnemy()
    {
        if (attackTimer >= attackCooldown)
        {
            targetEnemy.TakeDamage(damage, "Melee"); // ������� ���� �����
            attackTimer = 0;
        }
    }

    void OnDrawGizmosSelected()
    {
        // ������������ ������� �����������
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
