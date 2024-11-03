using UnityEngine;

public class BazokaProjectile : MonoBehaviour
{
    public Rigidbody theRB;
    public float moveSpeed;
    public float damageAmount;
    public GameObject impactEffect;

    private ProjectileTower tower; // Ссылка на башню, чтобы вернуть снаряд в пул
    private bool hasCollided = false; // Флаг для отслеживания первого столкновения

    void Start()
    {
        theRB.velocity = transform.forward * moveSpeed;
    }

    public void SetTower(ProjectileTower towerRef)
    {
        tower = towerRef;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasCollided && other.CompareTag("Enemy"))
        {
            hasCollided = true; // Устанавливаем флаг, чтобы предотвратить повторное срабатывание

            // Создаем эффект взрыва
            GameObject impact = Instantiate(impactEffect, transform.position, Quaternion.identity);
            BazokaImpact impactScript = impact.GetComponent<BazokaImpact>();
            if (impactScript != null)
            {
                impactScript.SetDamageAmount(damageAmount);
            }

            // Возвращаем снаряд в пул
            if (tower != null)
            {
                tower.ReturnProjectileToPool(gameObject);
            }
        }
    }

    void Update()
    {
        // Проверяем, вышел ли снаряд за допустимые границы, и возвращаем в пул, если нужно
        if ((transform.position.y <= -10) || (transform.position.y >= 10))
        {
            if (tower != null)
            {
                tower.ReturnProjectileToPool(gameObject);
            }
        }
    }

    private void OnDisable()
    {
        // Сбрасываем флаг при отключении снаряда, чтобы он снова мог срабатывать при следующем запуске
        hasCollided = false;
    }
}
