using UnityEngine;
using UnityEngine.UI;

public class TimeControl : MonoBehaviour
{
    // Ссылки на кнопки
    public Button pauseButton;
    public Button normalSpeedButton;
    public Button doubleSpeedButton;
    public Button tripleSpeedButton;

    // Цвета для активной и неактивной кнопки
    private Color activeColor = new Color(1f, 1f, 1f, 0.5f); // Полупрозрачный
    private Color defaultColor = new Color(1f, 1f, 1f, 1f); // Полностью непрозрачный

    // Компонент камеры, на который не влияет Time.timeScale
    public Camera mainCamera;

    private float defaultCameraTimeScale = 1f;

    private void Start()
    {
        // Устанавливаем нормальную скорость как начальное состояние
        SetNormalSpeed();
    }

    private void Update()
    {
        // Обработка горячих клавиш
        if (Input.GetKeyDown(KeyCode.Space)) SetPause();
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetNormalSpeed();
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetDoubleSpeed();
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetTripleSpeed();
    }

    // Установить паузу
    public void SetPause()
    {
        Time.timeScale = 0f; // Остановка времени
        UpdateCameraTimeScale(1f); // Камера продолжает работать в нормальном режиме
        HighlightButton(pauseButton);
    }

    // Установить нормальную скорость (1x)
    public void SetNormalSpeed()
    {
        Time.timeScale = 1f; // Нормальная скорость
        UpdateCameraTimeScale(1f); // Камера остается в нормальном режиме
        HighlightButton(normalSpeedButton);
    }

    // Установить удвоенную скорость (2x)
    public void SetDoubleSpeed()
    {
        Time.timeScale = 2f; // Ускорение в 2 раза
        UpdateCameraTimeScale(1f); // Камера остается в нормальном режиме
        HighlightButton(doubleSpeedButton);
    }

    // Установить утроенную скорость (3x)
    public void SetTripleSpeed()
    {
        Time.timeScale = 3f; // Ускорение в 3 раза
        UpdateCameraTimeScale(1f); // Камера остается в нормальном режиме
        HighlightButton(tripleSpeedButton);
    }

    // Обновить TimeScale камеры
    private void UpdateCameraTimeScale(float scale)
    {
        if (mainCamera != null)
        {
            // Камера работает независимо от Time.timeScale
            mainCamera.GetComponent<Animator>().speed = scale;
        }
    }

    // Подсветить выбранную кнопку
    private void HighlightButton(Button selectedButton)
    {
        // Сброс прозрачности для всех кнопок
        ResetButtonColors();

        // Установить прозрачность для выбранной кнопки
        if (selectedButton != null)
        {
            Image buttonImage = selectedButton.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = activeColor;
            }
        }
    }

    // Сбросить цвета всех кнопок
    private void ResetButtonColors()
    {
        SetButtonColor(pauseButton, defaultColor);
        SetButtonColor(normalSpeedButton, defaultColor);
        SetButtonColor(doubleSpeedButton, defaultColor);
        SetButtonColor(tripleSpeedButton, defaultColor);
    }

    // Установить цвет кнопки
    private void SetButtonColor(Button button, Color color)
    {
        if (button != null)
        {
            Image buttonImage = button.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = color;
            }
        }
    }
}
