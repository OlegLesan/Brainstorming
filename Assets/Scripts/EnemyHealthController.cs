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

    public float health = 100f;

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
            Destroy(gameObject);

            MoneyManager.instance.GiveMoney(moneyOnDeath);
            LevelManager.instance.activeEnemies.Remove(this);

            Debug.Log("Враг уничтожен. Осталось активных врагов: " + LevelManager.instance.activeEnemies.Count);

            // Уменьшаем счетчик оставшихся врагов в WaveManager
            WaveManager.instance.DecreaseEnemyCount();
        }
        healthBar.value = totalHealth;
        healthBar.gameObject.SetActive(true);
    }
}
