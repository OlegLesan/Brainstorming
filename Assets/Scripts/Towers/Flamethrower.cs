using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    
    public float damagePerSecond = 10f; // Урон в секунду
    public LayerMask enemyLayer; // Слой врагов

   

    private void OnTriggerStay(Collider other)
    {
        // Проверяем, находится ли объект на слое врагов
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            EnemyHealthController enemy = other.GetComponent<EnemyHealthController>();
            if (enemy != null)
            {
                // Наносим урон врагу
                float damage = damagePerSecond * Time.deltaTime;
                Debug.Log($"Dealing {damage} damage to {other.name}");
                enemy.TakeDamage(damage, "Fire");
            }
        }
    }
}
