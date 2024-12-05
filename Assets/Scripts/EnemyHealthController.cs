using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthController : MonoBehaviour
{
    private SoundPlayer soundPlayer;
    public float totalHealth;
    public Slider healthBar;
    public float rotationSpeed = 5f;
    private Camera targetCamera;
    public int moneyOnDeath = 50;
    public float destroyTime = 10f;

    private float initialHealth;
    private Collider enemyCollider;
    private Animator animator;
    
    private EnemyPool enemyPool;

    private bool isDead = false;

    [System.Serializable]
    public struct DamageResistance
    {
        public string damageType;
        [Range(0, 100)] public float resistancePercentage;
    }

    public List<DamageResistance> resistances;

    void Awake()
    {
        soundPlayer = GetComponent<SoundPlayer>();
        initialHealth = totalHealth;
    }

    void Start()
    {


        soundPlayer.PlaySound(0);
        healthBar.maxValue = initialHealth;
        healthBar.value = totalHealth;
        healthBar.gameObject.SetActive(false);

        LevelManager.instance.activeEnemies.Add(this);
        targetCamera = Camera.main;
        enemyCollider = GetComponent<Collider>();
        animator = GetComponent<Animator>();
       
        enemyPool = FindObjectOfType<EnemyPool>();
    }

    void Update()
    {
        
        if (healthBar != null && targetCamera != null)
        {
            Vector3 direction = targetCamera.transform.position - healthBar.transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            healthBar.transform.rotation = Quaternion.Lerp(healthBar.transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void TakeDamage(float damageAmount, string damageType)
    {
        if (isDead) return;

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

    private float GetResistanceFactor(string damageType)
    {
        foreach (var resistance in resistances)
        {
            if (resistance.damageType == damageType)
            {
                return Mathf.Clamp(resistance.resistancePercentage / 100f, 0f, 1f);
            }
        }
        return 0f;
    }

    private void HandleDeath()
    {
        if (isDead) return; // Проверяем, что смерть уже обработана
        isDead = true;

        Debug.Log("Enemy died: " + gameObject.name);

        // Отключение коллайдера
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

        // Отключение здоровья
        healthBar.gameObject.SetActive(false);

        

        // Сброс параметров аниматора и установка IsDead
        if (animator != null)
        {
            Debug.Log("Playing death animation for: " + gameObject.name);

            // Устанавливаем состояние смерти в аниматоре
            animator.SetBool("IsDead", true);
        }
        else
        {
            Debug.LogWarning("Animator not found on: " + gameObject.name);
        }

        // Добавляем деньги
        MoneyManager.instance.GiveMoney(moneyOnDeath);

        // Убираем из активного списка
        LevelManager.instance.RemoveEnemyFromActiveList(this);
        WaveManager.instance.DecreaseEnemyCount();

        // Остановка движения
        EnemyControler enemyController = GetComponent<EnemyControler>();
        if (enemyController != null)
        {
            enemyController.StopMoving();
        }

        // Уничтожение или возврат в пул
        StartCoroutine(ReturnToPoolAfterDelay());
    }


    private IEnumerator ReturnToPoolAfterDelay()
    {
        if (animator != null)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // Ждем, пока анимация смерти начнется
            while (!stateInfo.IsName("Death"))
            {
                yield return null;
                stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            }

            // Ждем окончания анимации смерти
            yield return new WaitForSeconds(stateInfo.length);
        }

        yield return new WaitForSeconds(destroyTime);
        enemyPool.ReturnEnemy(gameObject);
    }

    public void ResetEnemy()
    {
        totalHealth = initialHealth;
        healthBar.maxValue = initialHealth;
        healthBar.value = totalHealth;
        healthBar.gameObject.SetActive(false);

        if (enemyCollider != null)
        {
            enemyCollider.enabled = true;
        }

       

        if (animator != null)
        {
            animator.ResetTrigger("Death");
        }

        isDead = false;

        EnemyControler controller = GetComponent<EnemyControler>();
        if (controller != null && controller.thePath != null)
        {
            controller.Setup(controller.thePath);
        }
    }

    public bool IsDead()
    {
        return isDead;
    }
}
