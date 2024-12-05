using UnityEngine;

public class MolotovFireEffect : MonoBehaviour
{
    private SoundPlayer soundPlayer;
    private float damageAmount;
    private string damageType;
    private Collider impactCollider;
    
    private float duration;       // ������������ �������
    private float damageInterval = 1f; // �������� ����� ���������� �����
    private float timer;

    private void Awake()
    {
        soundPlayer = GetComponent<SoundPlayer>();
        impactCollider = GetComponent<Collider>();
        
    }
    
    public void SetDamageAmount(float amount, string type)
    {
        damageAmount = amount;
        damageType = type;
    }

    public void SetDuration(float effectDuration)
    {
        duration = effectDuration;
    }

    private void Start()
    {

        
        timer = duration;
        InvokeRepeating(nameof(ApplyDamageToAllEnemiesInRange), 0f, damageInterval);
        soundPlayer.PlaySound(0);
    }

    private void ApplyDamageToAllEnemiesInRange()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, impactCollider.bounds.extents.magnitude);

        foreach (Collider enemyCollider in enemiesInRange)
        {
            if (enemyCollider.CompareTag("Enemy"))
            {
                EnemyHealthController enemyHealth = enemyCollider.GetComponent<EnemyHealthController>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damageAmount, damageType);
                }
            }
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            CancelInvoke(nameof(ApplyDamageToAllEnemiesInRange));
            Destroy(gameObject);
        }
    }
}
