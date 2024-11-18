using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManipulatorTimer : MonoBehaviour
{
    public Button pauseButton;
    public Button x1Button;
    public Button x2Button;
    public Button x3Button;

    private float normalAlpha = 1f;
    private float transparentAlpha = 0.5f;
    public static bool IsPaused { get; private set; } = false; // Состояние паузы

    void Start()
    {
        // Назначаем события кнопок
        pauseButton.onClick.AddListener(() => SetTimeScale(0f, pauseButton));
        x1Button.onClick.AddListener(() => SetTimeScale(1f, x1Button));
        x2Button.onClick.AddListener(() => SetTimeScale(2f, x2Button));
        x3Button.onClick.AddListener(() => SetTimeScale(3f, x3Button));

        // Устанавливаем начальное состояние
        SetTimeScale(1f, x1Button);
    }

    void Update()
    {
        // Проверяем горячие клавиши
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetTimeScale(0f, pauseButton);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetTimeScale(1f, x1Button);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetTimeScale(2f, x2Button);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetTimeScale(3f, x3Button);
        }
    }

    void SetTimeScale(float timeScale, Button activeButton)
    {
        Time.timeScale = timeScale;
        IsPaused = timeScale == 0; // Обновляем флаг паузы

        // Обновляем прозрачность кнопок
        UpdateButtonAlpha(pauseButton, activeButton == pauseButton);
        UpdateButtonAlpha(x1Button, activeButton == x1Button);
        UpdateButtonAlpha(x2Button, activeButton == x2Button);
        UpdateButtonAlpha(x3Button, activeButton == x3Button);
    }

    void UpdateButtonAlpha(Button button, bool isActive)
    {
        Color color = button.image.color;
        color.a = isActive ? transparentAlpha : normalAlpha;
        button.image.color = color;
    }
}
