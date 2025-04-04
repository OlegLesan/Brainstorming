using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Base : MonoBehaviour
{
    public float totalHealth = 100f;
    public float currentHealth;
    public Image healthImage; // ���� ��� ����������� ��������
    public TextMeshProUGUI healthText; // TextMeshPro ��� ����������� ��������� ��������

    void Start()
    {
        currentHealth = totalHealth;
        if (healthImage == null || healthText == null)
        {
            Debug.LogError("Health Image or Health Text is not assigned!");
            return;
        }
        UpdateHealthUI();
    }

    public void TakeDamage(float damageToTake)
    {
        currentHealth -= damageToTake;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Game Over");
            UIController.instance.ShowLevelFailScreen();
        }
        else
        {
            LevelManager.instance.CheckForLevelCompletion(); // �������� ���������� ������
        }

        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        healthImage.fillAmount = currentHealth / totalHealth;
        healthText.text = Mathf.RoundToInt((currentHealth / totalHealth) * 100) + "%";
    }
}
