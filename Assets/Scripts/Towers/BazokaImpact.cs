using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazokaImpact : MonoBehaviour
{
    private SoundPlayer soundPlayer;
    private float damageAmount;
    private string damageType; // Переменная для хранения типа урона
    private Collider impactCollider;
    

    private void Awake()
    {
        soundPlayer = GetComponent<SoundPlayer>();
        // Кэшируем ссылки на коллайдер и аудиокомпонент
        impactCollider = GetComponent<Collider>();
        
    }
    private void Start()
    {
        soundPlayer.PlaySound(0);
    }
    // Метод для установки значения урона и типа урона
    public void SetDamageAmount(float amount, string type)
    {
        damageAmount = amount;
        damageType = type;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Если столкновение произошло с объектом, отмеченным как "Enemy"
        if (other.CompareTag("Enemy"))
        {
            // Наносим урон всем врагам в радиусе, запускаем звук и отключаем коллайдер
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
                    // Передаем и урон, и тип урона
                    enemyHealth.TakeDamage(damageAmount, damageType);
                }
            }
        }
    }
}
