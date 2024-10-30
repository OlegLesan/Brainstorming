using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazokaImpact : MonoBehaviour
{
    private float damageAmount;
    private bool hasPlayedSound = false; // Флаг для отслеживания воспроизведения звука

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

            // Воспроизводим звук только один раз
            if (!hasPlayedSound)
            {
                AudioManager.instance.PlaySFX(0);
                hasPlayedSound = true; // Устанавливаем флаг, чтобы звук больше не воспроизводился
            }
        }
    }
}
