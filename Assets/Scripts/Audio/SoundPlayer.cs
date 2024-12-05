using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [Tooltip("������ ����� � ������� sfx � AudioManager")]
    public int soundIndex; // ����� ������� ����� � AudioManager.sfx

    public void PlaySound()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(soundIndex);
        }
        else
        {
            Debug.LogWarning("AudioManager �� ������ � �����.");
        }
    }
}
