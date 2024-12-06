using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [Tooltip("������ �������� ������ � AudioManager.sfx")]
    public int[] soundIndexes;

    [Header("��������� ������������ AudioSource")]
    [Tooltip("��������� ����� (0 - ����� �������, 256 - ����� ������)")]
    [Range(0, 256)] public int priority = 128;
    [Tooltip("�������������� ��������� ��������� (0.0 - ��� �����, 1.0 - ����������� ���������)")]
    [Range(0f, 1f)] public float volumeMultiplier = 1f;
    [Tooltip("��������� ����?")]
    public bool loop = false;

    [Header("��������� ���������� �����")]
    [Tooltip("����������� ����")]
    [Range(0.1f, 3f)] public float minPitch = 0.9f;
    [Tooltip("������������ ����")]
    [Range(0.1f, 3f)] public float maxPitch = 1.1f;

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
                        tempSource.volume = sfxSource.volume * AudioManager.instance.sfxVolume * volumeMultiplier; // ��������� ���������� ��������� � ���������
                        tempSource.pitch = Random.Range(minPitch, maxPitch); // ������������� ��������� ����
                        tempSource.loop = loop; // ���������� ��������� �� SoundPlayer
                        tempSource.spatialBlend = sfxSource.spatialBlend;
                        tempSource.priority = priority; // ������������� ���������
                        tempSource.outputAudioMixerGroup = sfxSource.outputAudioMixerGroup;

                        // ����������� ����
                        tempSource.Play();

                        // ���������� ��������� AudioSource, ������ ���� ���� �� ��������
                        if (!loop)
                        {
                            Destroy(tempSource, tempSource.clip.length);
                        }
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
