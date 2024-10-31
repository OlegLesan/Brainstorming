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

        // Получаем компоненты коллайдера, аниматора и аудиосорса
        enemyCollider = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (animator == null)
        {
            Debug.LogError("Аниматор не найден! Убедитесь, что на объекте есть компонент Animator.");
        }
    }

    void Update()
    {
        if (targetCamera != null)
        {
            Vector3 directionToCamera = targetCamera.transform.position - healthBar.transform.position;
            directionToCamera.y = 0;

            if (directionToCamera.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
                Quaternion smoothRotation = Quaternion.Slerp(healthBar.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

                // Применяем только Y-поворот, а по оси X всегда фиксированное значение
                healthBar.transform.rotation = Quaternion.Euler(fixedXRotation, smoothRotation.eulerAngles.y, 0);
            }
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
        // Отключаем коллайдер
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

        // Отключаем индикатор здоровья
        healthBar.gameObject.SetActive(false);

        // Останавливаем и отключаем аудио
        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.enabled = false;
        }

        // Запускаем анимацию смерти
        if (animator != null)
        {
            animator.SetTrigger("Death");
        }

        // Увеличиваем деньги и уменьшаем количество врагов
        MoneyManager.instance.GiveMoney(moneyOnDeath);
        LevelManager.instance.activeEnemies.Remove(this);

        Debug.Log("Враг уничтожен. Осталось активных врагов: " + LevelManager.instance.activeEnemies.Count);
        WaveManager.instance.DecreaseEnemyCount();

        // Удаляем объект через 30 секунд
        StartCoroutine(DestroyAfterDelay(destroyTime));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
