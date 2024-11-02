using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazokaImpact : MonoBehaviour
{
    private float damageAmount;
    private Collider impactCollider;

    private void Awake()
    {
        // Кэшируем ссылку на коллайдер
        impactCollider = GetComponent<Collider>();
    }

    // Метод для установки значения урона
    public void SetDamageAmount(float amount)
    {
        damageAmount = amount;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Если столкновение произошло с объектом, отмеченным как "Enemy"
        if (other.CompareTag("Enemy"))
        {
            // Наносим урон всем врагам в радиусе и отключаем коллайдер
            ApplyDamageToAllEnemiesInRange();
            impactCollider.enabled = false;
        }
    }

    private void ApplyDamageToAllEnemiesInRange()
    {
        // Находим все коллайдеры с тегом "Enemy" в зоне поражения
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, impactCollider.bounds.extents.magnitude);

        foreach (Collider enemyCollider in enemiesInRange)
        {
            if (enemyCollider.CompareTag("Enemy"))
            {
                EnemyHealthController enemyHealth = enemyCollider.GetComponent<EnemyHealthController>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damageAmount);
                }
            }
        }
    }
}
