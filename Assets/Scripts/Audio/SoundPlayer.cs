using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [Tooltip("������ �������� ������ � AudioManager.sfx")]
    public int[] soundIndexes;

    /// <summary>
    /// ��������������� ����� ����� AudioManager �� ������� ������� soundIndexes.
    /// </summary>
    /// <param name="index">������ ������ ������� soundIndexes.</param>
    public void PlaySound(int index)
    {
        if (AudioManager.instance != null)
        {
            if (index >= 0 && index < soundIndexes.Length)
            {
                int soundIndex = soundIndexes[index];

                if (soundIndex >= 0 && soundIndex < AudioManager.instance.sfx.Length)
                {
                    // �������� AudioSource �� ������� sfx
                    AudioSource sfxSource = AudioManager.instance.sfx[soundIndex];

                    if (sfxSource != null)
                    {
                        // ������ ��������� AudioSource � �������� ���������
                        AudioSource tempSource = gameObject.AddComponent<AudioSource>();
                        tempSource.clip = sfxSource.clip;
                        tempSource.volume = sfxSource.volume * AudioManager.instance.sfxVolume; // ��������� ���������� ���������
                        tempSource.pitch = sfxSource.pitch;
                        tempSource.loop = sfxSource.loop;
                        tempSource.spatialBlend = sfxSource.spatialBlend;
                        tempSource.outputAudioMixerGroup = sfxSource.outputAudioMixerGroup;

                        // ����������� ����
                        tempSource.Play();

                        // ���������� ��������� AudioSource ����� ���������� ���������������
                        Destroy(tempSource, tempSource.clip.length);
                    }
                    else
                    {
                        Debug.LogWarning($"AudioSource � �������� {soundIndex} � ������� sfx �� ������.");
                    }
                }
                else
                {
                    Debug.LogWarning($"������ {soundIndex} ��� ��������� ������� sfx � AudioManager.");
                }
            }
            else
            {
                Debug.LogWarning($"������ {index} ��� ��������� ������� soundIndexes.");
            }
        }
        else
        {
            Debug.LogWarning("AudioManager �� ������ � �����.");
        }
    }
}
