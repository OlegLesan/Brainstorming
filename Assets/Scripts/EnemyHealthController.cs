using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthController : MonoBehaviour
{
    public float totalHealth;
    public Slider healthBar;
    public float rotationSpeed = 5f; // �������� ��������
    private Camera targetCamera; // ������, �� ������� ����� �������������� healthBar
    private float fixedXRotation = 70f; // ������������� ���� �� ��� X

    void Start()
    {
        healthBar.maxValue = totalHealth;
        healthBar.value = totalHealth;

        // ������������� �������� ������, ���� ��� �� ������� �������
        targetCamera = Camera.main;

        if (targetCamera == null)
        {
            Debug.LogError("������ �� �������! ���������, ��� � ����� ���� Main Camera.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetCamera != null)
        {
            // �������� ����������� � ������
            Vector3 directionToCamera = targetCamera.transform.position - healthBar.transform.position;

            // ���������� ��������� �� ��� Y, ����� ������ ������ �������������� �������
            directionToCamera.y = 0;

            // �������� �� ������� ����������� (����� ������ ����� ��� ��� ��� ��������)
            if (directionToCamera.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToCamera); // ������� ������� �� ��� Y
                Quaternion smoothRotation = Quaternion.Slerp(healthBar.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed); // ������� ������������

                // ��������� ������ Y-�������, � �� ��� X ������ ������������� ��������
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
