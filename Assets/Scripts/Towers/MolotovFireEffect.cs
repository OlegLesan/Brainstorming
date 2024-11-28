using UnityEngine;

public class MolotovFireEffect : MonoBehaviour
{
    private float damageAmount;
    private string damageType;
    private Collider impactCollider;
    private AudioSource explosionSound;
    private float duration;       // Длительность эффекта
    private float damageInterval = 1f; // Интервал между нанесением урона
    private float timer;

    private void Awake()
    {
        impactCollider = GetComponent<Collider>();
        explosionSound = GetComponent<AudioSource>();
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
        if (explosionSound != null)
        {
            explosionSound.Play();
        }

        timer = duration;
        InvokeRepeating(nameof(ApplyDamageToAllEnemiesInRange), 0f, damageInterval);
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
