using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthController : MonoBehaviour
{
    public float totalHealth;
    public Slider healthBar;
    public float rotationSpeed = 5f;
    private Camera targetCamera;
    public int moneyOnDeath = 50;
    public float destroyTime;

    private Collider enemyCollider;
    private Animator animator;
    private AudioSource audioSource;

    // —труктура дл€ хранени€ типа урона и уровн€ защиты
    [System.Serializable]
    public struct DamageResistance
    {
        public string damageType;
        public int resistanceLevel; // 1 - 25%, 2 - 50%, 3 - 75%
    }

    // —писок защит врага
    public List<DamageResistance> resistances;

    void Start()
    {
        healthBar.maxValue = totalHealth;
        healthBar.value = totalHealth;
        healthBar.gameObject.SetActive(false);

        LevelManager.instance.activeEnemies.Add(this);
        targetCamera = Camera.main;

        enemyCollider = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (healthBar != null && targetCamera != null)
        {
            // ѕоворачиваем здоровье врага к камере
            Vector3 direction = targetCamera.transform.position - healthBar.transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            healthBar.transform.rotation = Quaternion.Lerp(healthBar.transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
    }

    // ћетод дл€ получени€ урона с учетом типа урона
    public void TakeDamage(float damageAmount, string damageType)
    {
        float resistanceFactor = GetResistanceFactor(damageType);
        float effectiveDamage = damageAmount * (1 - resistanceFactor);

        totalHealth -= effectiveDamage;
        if (totalHealth <= 0)
        {
            totalHealth = 0;
            HandleDeath();
        }
        else
        {
            healthBar.value = totalHealth;
            healthBar.gameObject.SetActive(true);
        }
    }

    // ћетод дл€ получени€ фактора поглощени€ урона
    private float GetResistanceFactor(string damageType)
    {
        foreach (var resistance in resistances)
        {
            if (resistance.damageType == damageType)
            {
                switch (resistance.resistanceLevel)
                {
                    case 1: return 0.25f;
                    case 2: return 0.50f;
                    case 3: return 0.75f;
                }
            }
        }
        return 0f; // Ќет защиты от этого типа урона
    }

    private void HandleDeath()
    {
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

        healthBar.gameObject.SetActive(false);

        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.enabled = false;
        }

        if (animator != null)
        {
            animator.SetTrigger("Death");
        }

        MoneyManager.instance.GiveMoney(moneyOnDeath);
        LevelManager.instance.activeEnemies.Remove(this);

        WaveManager.instance.DecreaseEnemyCount();

        StartCoroutine(DestroyAfterDelay(destroyTime));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
