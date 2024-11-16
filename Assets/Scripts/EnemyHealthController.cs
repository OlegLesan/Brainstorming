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

    private float initialHealth; // ��������� ��������� �������� �������� �� �������
    private Collider enemyCollider;
    private Animator animator;
    private AudioSource audioSource;
    private EnemyPool enemyPool;

    [System.Serializable]
    public struct DamageResistance
    {
        public string damageType;
        public int resistanceLevel; // 1 - 25%, 2 - 50%, 3 - 75%
    }

    public List<DamageResistance> resistances;

    void Awake()
    {
        // ��������� �������� �������� �� ������� � initialHealth �� ������ ������ Start
        initialHealth = totalHealth;
    }

    void Start()
    {
        healthBar.maxValue = initialHealth;
        healthBar.value = totalHealth;
        healthBar.gameObject.SetActive(false);

        LevelManager.instance.activeEnemies.Add(this); // ��������� ����� � ��������
        targetCamera = Camera.main;
        enemyCollider = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        enemyPool = FindObjectOfType<EnemyPool>(); // ������ ��� � �����
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
                switch (resistance.resistanceLevel)
                {
                    case 1: return 0.25f;
                    case 2: return 0.50f;
                    case 3: return 0.75f;
                }
            }
        }
        return 0f; // ��� ������ �� ����� ���� �����
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

        // ������� ����� �� ������ �������� ������
        LevelManager.instance.RemoveEnemyFromActiveList(this);

        MoneyManager.instance.GiveMoney(moneyOnDeath);

        WaveManager.instance.DecreaseEnemyCount();

        // ��������� �������� ��� �������� ����� � ���
        StartCoroutine(ReturnToPoolAfterDelay());
    }

    private IEnumerator ReturnToPoolAfterDelay()
    {
        yield return new WaitForSeconds(10f); // ���� 10 ������
        enemyPool.ReturnEnemy(gameObject); // ���������� ����� � ���
    }

    public void ResetEnemy()
    {
        // ��������������� �������� �� ���������� ��������
        totalHealth = initialHealth;
        healthBar.maxValue = initialHealth;
        healthBar.value = totalHealth;
        healthBar.gameObject.SetActive(false);

        if (enemyCollider != null)
        {
            enemyCollider.enabled = true;
        }

        if (audioSource != null)
        {
            audioSource.enabled = true;
            audioSource.Play();
        }

        if (animator != null)
        {
            animator.ResetTrigger("Death");
        }

        // ���������� ���� � ����
        EnemyControler controller = GetComponent<EnemyControler>();
        if (controller != null && controller.thePath != null)
        {
            controller.Setup(controller.thePath); // ��������� ����� ����
        }
    }
}
