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
    private float fixedXRotation = 70f;

    public int moneyOnDeath = 50;

    void Start()
    {
        healthBar.maxValue = totalHealth;
        healthBar.value = totalHealth;
        healthBar.gameObject.SetActive(false);
        
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
                Quaternion targetRotation = Quaternion.LookRotation(directionToCamera); // Целевой поворот по оси Y
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
        }
        healthBar.value = totalHealth;
        healthBar.gameObject.SetActive(true);
    }
}
