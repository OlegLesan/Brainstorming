using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    public ParticleSystem flamethrowerParticles; // Ссылка на Particle System
    public float damagePerSecond = 10f; // Урон в секунду

    // Список для хранения событий столкновений
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    void Start()
    {
        if (flamethrowerParticles == null)
        {
            flamethrowerParticles = GetComponent<ParticleSystem>();
        }
    }

    void OnParticleCollision(GameObject other)
    {
        // Получаем список всех событий столкновений
        int numCollisionEvents = ParticlePhysicsExtensions.GetCollisionEvents(flamethrowerParticles, other, collisionEvents);

        for (int i = 0; i < numCollisionEvents; i++)
        {
            // Проверяем, есть ли у объекта EnemyHealthController
            EnemyHealthController enemy = other.GetComponent<EnemyHealthController>();
            if (enemy != null)
            {
                // Наносим урон врагу
                enemy.TakeDamage(damagePerSecond * Time.deltaTime, "Fire");
            }
        }
    }
}
