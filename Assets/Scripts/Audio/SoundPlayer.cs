using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [Tooltip("Массив индексов звуков в AudioManager.sfx")]
    public int[] soundIndexes;

    [Header("Настройки создаваемого AudioSource")]
    [Tooltip("Приоритет звука (0 - самый высокий, 256 - самый низкий)")]
    [Range(0, 256)] public int priority = 128;
    [Tooltip("Дополнительный множитель громкости (0.0 - без звука, 1.0 - стандартная громкость)")]
    [Range(0f, 1f)] public float volumeMultiplier = 1f;
    [Tooltip("Зациклить звук?")]
    public bool loop = false;

    [Header("Настройки случайного питча")]
    [Tooltip("Минимальный питч")]
    [Range(0.1f, 3f)] public float minPitch = 0.9f;
    [Tooltip("Максимальный питч")]
    [Range(0.1f, 3f)] public float maxPitch = 1.1f;

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

                if (soundIndex >= 0 && soundIndex < AudioManager.instance.sfx.Length)
                {
                    // Получаем AudioSource из массива sfx
                    AudioSource sfxSource = AudioManager.instance.sfx[soundIndex];

                    if (sfxSource != null)
                    {
                        // Создаём временный AudioSource и копируем настройки
                        AudioSource tempSource = gameObject.AddComponent<AudioSource>();
                        tempSource.clip = sfxSource.clip;
                        tempSource.volume = sfxSource.volume * AudioManager.instance.sfxVolume * volumeMultiplier; // Применяем глобальную громкость и множитель
                        tempSource.pitch = Random.Range(minPitch, maxPitch); // Устанавливаем случайный питч
                        tempSource.loop = loop; // Используем настройку из SoundPlayer
                        tempSource.spatialBlend = sfxSource.spatialBlend;
                        tempSource.priority = priority; // Устанавливаем приоритет
                        tempSource.outputAudioMixerGroup = sfxSource.outputAudioMixerGroup;

                        // Проигрываем звук
                        tempSource.Play();

                        // Уничтожаем временный AudioSource, только если звук не зациклен
                        if (!loop)
                        {
                            Destroy(tempSource, tempSource.clip.length);
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"AudioSource с индексом {soundIndex} в массиве sfx не найден.");
                    }
                }
                else
                {
                    Debug.LogWarning($"Индекс {soundIndex} вне диапазона массива sfx в AudioManager.");
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
}
