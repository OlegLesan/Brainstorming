using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownTower : MonoBehaviour
{
    public float slowDownMultiplier = 0.5f; // ����������� ����������

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // ���������, ��� ��� ������ �����
        {
            EnemyControler enemy = other.GetComponent<EnemyControler>();
            if (enemy != null)
            {
                enemy.speedMod = slowDownMultiplier; // ��������� ����������
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy")) // ���������, ��� ��� ������ �����
        {
            EnemyControler enemy = other.GetComponent<EnemyControler>();
            if (enemy != null)
            {
                enemy.speedMod = 1f; // ��������������� ���������� ��������
            }
        }
    }
}
