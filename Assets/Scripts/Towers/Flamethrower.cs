using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    public ParticleSystem flamethrowerParticles; // ������ �� Particle System
    public float damagePerSecond = 10f; // ���� � �������

    // ������ ��� �������� ������� ������������
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
        // �������� ������ ���� ������� ������������
        int numCollisionEvents = ParticlePhysicsExtensions.GetCollisionEvents(flamethrowerParticles, other, collisionEvents);

        for (int i = 0; i < numCollisionEvents; i++)
        {
            // ���������, ���� �� � ������� EnemyHealthController
            EnemyHealthController enemy = other.GetComponent<EnemyHealthController>();
            if (enemy != null)
            {
                // ������� ���� �����
                enemy.TakeDamage(damagePerSecond * Time.deltaTime, "Fire");
            }
        }
    }
}
