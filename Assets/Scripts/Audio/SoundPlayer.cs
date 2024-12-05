using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [Tooltip("Индекс звука в массиве sfx в AudioManager")]
    public int soundIndex; // Номер индекса звука в AudioManager.sfx

    public void PlaySound()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(soundIndex);
        }
        else
        {
            Debug.LogWarning("AudioManager не найден в сцене.");
        }
    }
}
