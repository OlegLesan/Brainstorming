using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazokaProjectile : MonoBehaviour
{
    public Rigidbody theRB;
    public float moveSpeed;

    public float damageAmount;

    public GameObject impactEffect;

    void Start()
    {
        // Устанавливаем начальную скорость для движения снаряда вперёд
        theRB.velocity = transform.forward * moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Создаем эффект удара в месте столкновения
        GameObject impact = Instantiate(impactEffect, transform.position, Quaternion.identity);

        // Передаем значение урона в скрипт BazokaImpact
        BazokaImpact impactScript = impact.GetComponent<BazokaImpact>();
        if (impactScript != null)
        {
            impactScript.SetDamageAmount(damageAmount);
        }

        // Уничтожаем снаряд после создания эффекта
        Destroy(gameObject);
    }

    void Update()
    {
        // Проверяем выход за границы и уничтожаем снаряд, если это произошло
        if ((transform.position.y <= -10) || (transform.position.y >= 10))
        {
            Destroy(gameObject);
        }
    }
}

