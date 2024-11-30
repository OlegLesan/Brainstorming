using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Melee : MonoBehaviour
{
    public float detectionRadius = 5f; // Радиус обнаружения врагов
    public float attackCooldown = 1f; // Задержка между атаками
    public int damage = 10; // Урон
    public Transform weaponPoint; // Точка, откуда наносится урон (можно визуализировать)

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
            SearchForEnemy(); // Ищем врага, если не в бою
        }
        else if (targetEnemy != null && !targetEnemy.isDead)
        {
            // Если есть цель, атакуем её
            FaceTarget();
            if (Vector3.Distance(transform.position, targetEnemy.transform.position) <= navAgent.stoppingDistance)
            {
                AttackEnemy();
            }
        }
        else
        {
            inCombat = false; // Если враг уничтожен, выходим из боя
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
                navAgent.stoppingDistance = 1.5f; // Задаём дистанцию для ближнего боя
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
            targetEnemy.TakeDamage(damage, "Melee"); // Наносим урон врагу
            attackTimer = 0;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Визуализация радиуса обнаружения
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
