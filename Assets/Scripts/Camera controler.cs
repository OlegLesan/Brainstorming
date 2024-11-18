using UnityEngine;
using UnityEngine.UI;

public class TimeControl : MonoBehaviour
{
    // ������ �� ������
    public Button pauseButton;
    public Button normalSpeedButton;
    public Button doubleSpeedButton;
    public Button tripleSpeedButton;

    // ����� ��� �������� � ���������� ������
    private Color activeColor = new Color(1f, 1f, 1f, 0.5f); // ��������������
    private Color defaultColor = new Color(1f, 1f, 1f, 1f); // ��������� ������������

    // ��������� ������, �� ������� �� ������ Time.timeScale
    public Camera mainCamera;

    private float defaultCameraTimeScale = 1f;

    private void Start()
    {
        // ������������� ���������� �������� ��� ��������� ���������
        SetNormalSpeed();
    }

    private void Update()
    {
        // ��������� ������� ������
        if (Input.GetKeyDown(KeyCode.Space)) SetPause();
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetNormalSpeed();
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetDoubleSpeed();
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetTripleSpeed();
    }

    // ���������� �����
    public void SetPause()
    {
        Time.timeScale = 0f; // ��������� �������
        UpdateCameraTimeScale(1f); // ������ ���������� �������� � ���������� ������
        HighlightButton(pauseButton);
    }

    // ���������� ���������� �������� (1x)
    public void SetNormalSpeed()
    {
        Time.timeScale = 1f; // ���������� ��������
        UpdateCameraTimeScale(1f); // ������ �������� � ���������� ������
        HighlightButton(normalSpeedButton);
    }

    // ���������� ��������� �������� (2x)
    public void SetDoubleSpeed()
    {
        Time.timeScale = 2f; // ��������� � 2 ����
        UpdateCameraTimeScale(1f); // ������ �������� � ���������� ������
        HighlightButton(doubleSpeedButton);
    }

    // ���������� ��������� �������� (3x)
    public void SetTripleSpeed()
    {
        Time.timeScale = 3f; // ��������� � 3 ����
        UpdateCameraTimeScale(1f); // ������ �������� � ���������� ������
        HighlightButton(tripleSpeedButton);
    }

    // �������� TimeScale ������
    private void UpdateCameraTimeScale(float scale)
    {
        if (mainCamera != null)
        {
            // ������ �������� ���������� �� Time.timeScale
            mainCamera.GetComponent<Animator>().speed = scale;
        }
    }

    // ���������� ��������� ������
    private void HighlightButton(Button selectedButton)
    {
        // ����� ������������ ��� ���� ������
        ResetButtonColors();

        // ���������� ������������ ��� ��������� ������
        if (selectedButton != null)
        {
            Image buttonImage = selectedButton.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = activeColor;
            }
        }
    }

    // �������� ����� ���� ������
    private void ResetButtonColors()
    {
        SetButtonColor(pauseButton, defaultColor);
        SetButtonColor(normalSpeedButton, defaultColor);
        SetButtonColor(doubleSpeedButton, defaultColor);
        SetButtonColor(tripleSpeedButton, defaultColor);
    }

    // ���������� ���� ������
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
