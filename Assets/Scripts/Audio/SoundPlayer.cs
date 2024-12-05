using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [Tooltip("Массив индексов звуков в AudioManager.sfx")]
    public int[] soundIndexes;

    private AudioSource prefabAudioSource;

    private void Awake()
    {
        // Получаем или добавляем компонент AudioSource на этот объект
        prefabAudioSource = GetComponent<AudioSource>();
        if (prefabAudioSource == null)
        {
            prefabAudioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    /// <summary>
    /// Воспроизведение звука через AudioManager по индексу массива soundIndexes.
    /// </summary>
    /// <param name="index">Индекс внутри массива soundIndexes.</param>
    public void PlaySound(int index)
    {
        if (AudioManager.instance != null)
        {
            if (index >= 0 && index < soundIndexes.Length)
            {
                int soundIndex = soundIndexes[index];

                // Настройка и воспроизведение через локальный AudioSource
                AudioManager.instance.ConfigureSFX(soundIndex, prefabAudioSource);
                prefabAudioSource.Play();
            }
            else
            {
                Debug.LogWarning($"Индекс {index} вне диапазона массива soundIndexes.");
            }
        }
        else
        {
            Debug.LogWarning("AudioManager не найден в сцене.");
        }
    }

    /// <summary>
    /// Воспроизведение всех звуков из массива soundIndexes.
    /// </summary>
    public void PlayAllSounds()
    {
        if (AudioManager.instance != null)
        {
            foreach (int soundIndex in soundIndexes)
            {
                AudioManager.instance.ConfigureSFX(soundIndex, prefabAudioSource);
                prefabAudioSource.Play();
            }
        }
        else
        {
            Debug.LogWarning("AudioManager не найден в сцене.");
        }
    }

    private void OnDisable()
    {
        // Если AudioManager существует, остановить звуки, связанные с этим объектом
        if (AudioManager.instance != null && prefabAudioSource != null)
        {
            AudioManager.instance.StopSFX(prefabAudioSource);
        }

        // Останавливаем локальный звук, если объект отключен
        if (prefabAudioSource != null && prefabAudioSource.isPlaying)
        {
            prefabAudioSource.Stop();
        }
    }

    private void OnDestroy()
    {
        // Если объект уничтожен, также остановить звуки
        if (AudioManager.instance != null && prefabAudioSource != null)
        {
            AudioManager.instance.StopSFX(prefabAudioSource);
        }
    }
}
