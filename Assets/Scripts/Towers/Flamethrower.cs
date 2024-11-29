using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    
    public float damagePerSecond = 10f; // ���� � �������
    public LayerMask enemyLayer; // ���� ������

   

    private void OnTriggerStay(Collider other)
    {
        // ���������, ��������� �� ������ �� ���� ������
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            EnemyHealthController enemy = other.GetComponent<EnemyHealthController>();
            if (enemy != null)
            {
                // ������� ���� �����
                float damage = damagePerSecond * Time.deltaTime;
                Debug.Log($"Dealing {damage} damage to {other.name}");
                enemy.TakeDamage(damage, "Fire");
            }
        }
    }
}
