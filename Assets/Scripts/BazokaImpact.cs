using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazokaImpact : MonoBehaviour
{
    private float damageAmount;

    // Метод для установки значения урона
    public void SetDamageAmount(float amount)
    {
        damageAmount = amount;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, если столкновение произошло с объектом, отмеченным как "Enemy"
        if (other.CompareTag("Enemy"))
        {
            // Получаем компонент здоровья врага и наносим урон
            EnemyHealthController enemyHealth = other.GetComponent<EnemyHealthController>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
            }
        }
    }
}
