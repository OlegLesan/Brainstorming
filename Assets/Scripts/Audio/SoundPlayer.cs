using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [Tooltip("Массив индексов звуков в AudioManager.sfx")]
    public int[] soundIndexes;

    private AudioSource prefabAudioSource;

    private void Awake()
    {
        // Получаем существующий AudioSource на объекте
        prefabAudioSource = GetComponent<AudioSource>();
        if (prefabAudioSource == null)
        {
            Debug.LogError("AudioSource не найден на объекте! Убедитесь, что компонент AudioSource добавлен.");
        }
    }

    /// <summary>
    /// Воспроизведение звука через AudioManager по индексу массива soundIndexes.
    /// </summary>
    /// <param name="index">Индекс внутри массива soundIndexes.</param>
    public void PlaySound(int index)
    {
        if (prefabAudioSource == null)
        {
            Debug.LogWarning("Попытка воспроизвести звук, но AudioSource не найден.");
            return;
        }

        if (AudioManager.instance != null)
        {
            if (index >= 0 && index < soundIndexes.Length)
            {
                int soundIndex = soundIndexes[index];

                // Устанавливаем только AudioClip, оставляя остальные настройки AudioSource объекта
                prefabAudioSource.clip = AudioManager.instance.GetSFXClip(soundIndex);
                if (prefabAudioSource.clip != null)
                {
                    prefabAudioSource.Play();
                }
                else
                {
                    Debug.LogWarning($"AudioClip для индекса {soundIndex} не найден в AudioManager.");
                }
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
        if (prefabAudioSource == null)
        {
            Debug.LogWarning("Попытка воспроизвести звук, но AudioSource не найден.");
            return;
        }

        if (AudioManager.instance != null)
        {
            foreach (int soundIndex in soundIndexes)
            {
                prefabAudioSource.clip = AudioManager.instance.GetSFXClip(soundIndex);
                if (prefabAudioSource.clip != null)
                {
                    prefabAudioSource.Play();
                }
                else
                {
                    Debug.LogWarning($"AudioClip для индекса {soundIndex} не найден в AudioManager.");
                }
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
