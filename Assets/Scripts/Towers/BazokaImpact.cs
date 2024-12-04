using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazokaImpact : MonoBehaviour
{
    private float damageAmount;
    private string damageType; // ���������� ��� �������� ���� �����
    private Collider impactCollider;
    

    private void Awake()
    {
        // �������� ������ �� ��������� � ��������������
        impactCollider = GetComponent<Collider>();
        
    }

    // ����� ��� ��������� �������� ����� � ���� �����
    public void SetDamageAmount(float amount, string type)
    {
        damageAmount = amount;
        damageType = type;
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���� ������������ ��������� � ��������, ���������� ��� "Enemy"
        if (other.CompareTag("Enemy"))
        {
            // ������� ���� ���� ������ � �������, ��������� ���� � ��������� ���������
            ApplyDamageToAllEnemiesInRange();
            impactCollider.enabled = false;
        }
    }

    private void ApplyDamageToAllEnemiesInRange()
    {
        

        // ������� ��� ���������� � ����� "Enemy" � ���� ���������
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, impactCollider.bounds.extents.magnitude);

        foreach (Collider enemyCollider in enemiesInRange)
        {
            if (enemyCollider.CompareTag("Enemy"))
            {
                EnemyHealthController enemyHealth = enemyCollider.GetComponent<EnemyHealthController>();
                if (enemyHealth != null)
                {
                    // �������� � ����, � ��� �����
                    enemyHealth.TakeDamage(damageAmount, damageType);
                }
            }
        }
    }
}
