using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody theRB;
    public float moveSpeed;

    public float damageAmount;

    public GameObject impactEffect;

    private bool hasDamaged;

    void Start()
    {
        theRB.velocity = transform.forward * moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, является ли объект врагом и еще не был ли нанесен урон
        if (other.CompareTag("Enemy") && !hasDamaged)
        {
            // Проверяем, есть ли у объекта компонент EnemyHealthController
            EnemyHealthController enemyHealth = other.GetComponent<EnemyHealthController>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
                hasDamaged = true;
            }
        }

        // Создаём эффект удара, если он задан
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }

        // Уничтожаем снаряд после столкновения
        Destroy(gameObject);
    }

    void Update()
    {
        // Удаляем снаряд, если он выходит за пределы допустимых координат
        if (transform.position.y <= -10 || transform.position.y >= 10)
        {
            Destroy(gameObject);
        }
    }
}
