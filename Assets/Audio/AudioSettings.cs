using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    private static bool isMuted; // ���� ��������� ����� (����������� ����� �������)
    private static bool initialized = false; // ���� ��� �������� �������������

    [SerializeField] private Image buttonImage; // ������ �� ��������� Image ������
    [SerializeField] private Sprite soundOnSprite; // ������ ��� ����������� �����
    [SerializeField] private Sprite soundOffSprite; // ������ ��� ������������ �����

    private void Awake()
    {
        // ���������, ���� �� ��������� ����� ��� ���������
        if (!initialized)
        {
            // ��������� ��������� �����
            isMuted = PlayerPrefs.GetInt("AudioMuted", 0) == 1;
            AudioListener.pause = isMuted;
            initialized = true;
        }

        // ��������� ������ ������ ��� ������
        UpdateButtonSprite();
    }

    public void ToggleMute()
    {
        isMuted = !isMuted; // ����������� ����
        AudioListener.pause = isMuted; // ������ ���� �� ����� ��� ��������

        // ��������� ��������� �����
        PlayerPrefs.SetInt("AudioMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();

        // ��������� ������ ������
        UpdateButtonSprite();
    }

    private void UpdateButtonSprite()
    {
        if (buttonImage != null)
        {
            buttonImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
        }
    }
}
