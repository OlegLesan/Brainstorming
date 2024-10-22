using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthController : MonoBehaviour
{
    public float totalHealth;
    public Slider healthBar;
    public float rotationSpeed = 5f; // скорость поворота
    private Camera targetCamera; // камера, на которую будет поворачиватьс€ healthBar
    private float fixedXRotation = 70f; // фиксированный угол по оси X

    void Start()
    {
        healthBar.maxValue = totalHealth;
        healthBar.value = totalHealth;

        // ”станавливаем основную камеру, если она не указана вручную
        targetCamera = Camera.main;

        if (targetCamera == null)
        {
            Debug.LogError(" амера не найдена! ”бедитесь, что в сцене есть Main Camera.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetCamera != null)
        {
            // ѕолучаем направление к камере
            Vector3 directionToCamera = targetCamera.transform.position - healthBar.transform.position;

            // »гнорируем изменение по оси Y, чтобы мен€ть только горизонтальный поворот
            directionToCamera.y = 0;

            // ѕроверка на наличие направлени€ (вдруг камера точно над или под объектом)
            if (directionToCamera.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToCamera); // ÷елевой поворот по оси Y
                Quaternion smoothRotation = Quaternion.Slerp(healthBar.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed); // ѕлавна€ интерпол€ци€

                // ѕримен€ем только Y-поворот, а по оси X всегда фиксированное значение
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
        }
        healthBar.value = totalHealth;
    }
}
