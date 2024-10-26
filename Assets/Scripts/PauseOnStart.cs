using UnityEngine;

public class PauseOnStart : MonoBehaviour
{
    void Start()
    {
        // ������������� timeScale � 0, ����� ��������� ���� �� �����
        Time.timeScale = 0f;
    }

    // �������������� ����� ��� ������ ���� � �����
    public void ResumeGame()
    {
        Time.timeScale = 1f; // ��������������� timeScale, ����� ����� ���� � �����
    }
}
