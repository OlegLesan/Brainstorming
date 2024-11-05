using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthController : MonoBehaviour
{
    public float totalHealth;
    public Slider healthBar;
    public float rotationSpeed = 5f;
    private Camera targetCamera;
    private float fixedXRotation = 70f;
    public int moneyOnDeath = 50;
    public float destroyTime;

    public float health = 100f;
    private Collider enemyCollider;
    private Animator animator;
    private AudioSource audioSource; // Ссылка на AudioSource

    void Start()
    {
        healthBar.maxValue = totalHealth;
        healthBar.value = totalHealth;
        healthBar.gameObject.SetActive(false);

        LevelManager.instance.activeEnemies.Add(this);
        Debug.Log("Добавлен враг. Активных врагов: " + LevelManager.instance.activeEnemies.Count);

        targetCamera = Camera.main;

        if (targetCamera == null)
        {
            Debug.LogError("Камера не найдена! Убедитесь, что в сцене есть Main Camera.");
        }

        enemyCollider = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (animator == null)
        {
            Debug.LogError("Аниматор не найден! Убедитесь, что на объекте есть компонент Animator.");
        }
    }

    public void TakeDamage(float damageAmount)
    {
        totalHealth -= damageAmount;
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

        Debug.Log("Враг уничтожен. Осталось активных врагов: " + LevelManager.instance.activeEnemies.Count);

        WaveManager.instance.DecreaseEnemyCount();

        StartCoroutine(DestroyAfterDelay(destroyTime));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
