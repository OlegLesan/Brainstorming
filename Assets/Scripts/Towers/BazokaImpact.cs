using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazokaImpact : MonoBehaviour
{
    private float damageAmount;
    private bool hasPlayedSound = false; // ���� ��� ������������ ��������������� �����

    // ����� ��� ��������� �������� �����
    public void SetDamageAmount(float amount)
    {
        damageAmount = amount;
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���������, ���� ������������ ��������� � ��������, ���������� ��� "Enemy"
        if (other.CompareTag("Enemy"))
        {
            // �������� ��������� �������� ����� � ������� ����
            EnemyHealthController enemyHealth = other.GetComponent<EnemyHealthController>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
            }

            // ������������� ���� ������ ���� ���
            if (!hasPlayedSound)
            {
                AudioManager.instance.PlaySFX(0);
                hasPlayedSound = true; // ������������� ����, ����� ���� ������ �� ���������������
            }
        }
    }
}
