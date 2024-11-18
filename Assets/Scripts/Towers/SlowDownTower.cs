using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownTower : MonoBehaviour
{
    public Light spotlight; // ������ �� Spot Light ��������
    public float slowDownAmount = 0.5f; // ���������� ����������
    public float checkInterval = 0.2f; // �������� �������� ��� ������������������

    private List<EnemyControler> affectedEnemies = new List<EnemyControler>();

    private void Start()
    {
        if (spotlight == null)
        {
            spotlight = GetComponent<Light>(); // ������������� ������� ��������� �����, ���� �� �� ��������
        }

        // ��������� �������� ����� ������������ ��������� �������
        InvokeRepeating(nameof(CheckEnemiesInLightCone), 0f, checkInterval);
    }

    private void CheckEnemiesInLightCone()
    {
        // ������� ���� ������ � �����
        EnemyControler[] allEnemies = FindObjectsOfType<EnemyControler>();

        // �������� �� ���� ������ � ��������� �� ���������� � ���� �� ��������
        foreach (EnemyControler enemy in allEnemies)
        {
            if (enemy != null)
            {
                Vector3 directionToEnemy = enemy.transform.position - spotlight.transform.position;
                float distanceToEnemy = directionToEnemy.magnitude;

                // ���������, ��������� �� ���� � �������� ������� �����
                if (distanceToEnemy <= spotlight.range)
                {
                    // ���������, ��������� �� ���� ������ ���� ��������� Spot Light
                    float angleToEnemy = Vector3.Angle(spotlight.transform.forward, directionToEnemy);

                    if (angleToEnemy <= spotlight.spotAngle / 2)
                    {
                        // ���� ���� ��� �� �������� ������ ���������
                        if (!affectedEnemies.Contains(enemy) && enemy.currentSlowingTower == null)
                        {
                            enemy.speedMod -= slowDownAmount;
                            enemy.currentSlowingTower = this;
                            affectedEnemies.Add(enemy);
                        }
                    }
                    else
                    {
                        // ���� ��� ������ �����, ��������������� ��������, ���� �� ��� ��� ���� ���������
                        RemoveEnemyFromEffect(enemy);
                    }
                }
                else
                {
                    // ���� ��� ������� �����, ��������������� ��������, ���� �� ��� ��� ���� ���������
                    RemoveEnemyFromEffect(enemy);
                }
            }
        }
    }

    private void RemoveEnemyFromEffect(EnemyControler enemy)
    {
        if (affectedEnemies.Contains(enemy) && enemy.currentSlowingTower == this)
        {
            enemy.speedMod += slowDownAmount;
            enemy.currentSlowingTower = null;
            affectedEnemies.Remove(enemy);
        }
    }

    private void OnDisable()
    {
        // ��������������� �������� ��� ���� ������ ��� ���������� ��������
        foreach (EnemyControler enemy in affectedEnemies)
        {
            if (enemy != null && enemy.currentSlowingTower == this)
            {
                enemy.speedMod += slowDownAmount;
                enemy.currentSlowingTower = null;
            }
        }
        affectedEnemies.Clear();
    }
}
