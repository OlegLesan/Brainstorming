using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownTower : MonoBehaviour
{
    public float slowDownMultiplier = 0.5f; // Коэффициент замедления

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // Проверяем, что это объект врага
        {
            EnemyControler enemy = other.GetComponent<EnemyControler>();
            if (enemy != null)
            {
                enemy.speedMod = slowDownMultiplier; // Применяем замедление
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy")) // Проверяем, что это объект врага
        {
            EnemyControler enemy = other.GetComponent<EnemyControler>();
            if (enemy != null)
            {
                enemy.speedMod = 1f; // Восстанавливаем нормальную скорость
            }
        }
    }
}
