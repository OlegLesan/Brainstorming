using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [Tooltip("Массив индексов звуков в AudioManager.sfx")]
    public int[] soundIndexes;

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
                        tempSource.volume = sfxSource.volume * AudioManager.instance.sfxVolume; // Применяем глобальную громкость
                        tempSource.pitch = sfxSource.pitch;
                        tempSource.loop = sfxSource.loop;
                        tempSource.spatialBlend = sfxSource.spatialBlend;
                        tempSource.outputAudioMixerGroup = sfxSource.outputAudioMixerGroup;

                        // Проигрываем звук
                        tempSource.Play();

                        // Уничтожаем временный AudioSource после завершения воспроизведения
                        Destroy(tempSource, tempSource.clip.length);
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
