using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [Tooltip("������ �������� ������ � AudioManager.sfx")]
    public int[] soundIndexes;

    private AudioSource prefabAudioSource;

    private void Awake()
    {
        // �������� ������������ AudioSource �� �������
        prefabAudioSource = GetComponent<AudioSource>();
        if (prefabAudioSource == null)
        {
            Debug.LogError("AudioSource �� ������ �� �������! ���������, ��� ��������� AudioSource ��������.");
        }
    }

    /// <summary>
    /// ��������������� ����� ����� AudioManager �� ������� ������� soundIndexes.
    /// </summary>
    /// <param name="index">������ ������ ������� soundIndexes.</param>
    public void PlaySound(int index)
    {
        if (prefabAudioSource == null)
        {
            Debug.LogWarning("������� ������������� ����, �� AudioSource �� ������.");
            return;
        }

        if (AudioManager.instance != null)
        {
            if (index >= 0 && index < soundIndexes.Length)
            {
                int soundIndex = soundIndexes[index];

                // ������������� ������ AudioClip, �������� ��������� ��������� AudioSource �������
                prefabAudioSource.clip = AudioManager.instance.GetSFXClip(soundIndex);
                if (prefabAudioSource.clip != null)
                {
                    prefabAudioSource.Play();
                }
                else
                {
                    Debug.LogWarning($"AudioClip ��� ������� {soundIndex} �� ������ � AudioManager.");
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

    /// <summary>
    /// ��������������� ���� ������ �� ������� soundIndexes.
    /// </summary>
    public void PlayAllSounds()
    {
        if (prefabAudioSource == null)
        {
            Debug.LogWarning("������� ������������� ����, �� AudioSource �� ������.");
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
                    Debug.LogWarning($"AudioClip ��� ������� {soundIndex} �� ������ � AudioManager.");
                }
            }
        }
        else
        {
            Debug.LogWarning("AudioManager �� ������ � �����.");
        }
    }

    private void OnDisable()
    {
        // ���� AudioManager ����������, ���������� �����, ��������� � ���� ��������
        if (AudioManager.instance != null && prefabAudioSource != null)
        {
            AudioManager.instance.StopSFX(prefabAudioSource);
        }

        // ������������� ��������� ����, ���� ������ ��������
        if (prefabAudioSource != null && prefabAudioSource.isPlaying)
        {
            prefabAudioSource.Stop();
        }
    }

    private void OnDestroy()
    {
        // ���� ������ ���������, ����� ���������� �����
        if (AudioManager.instance != null && prefabAudioSource != null)
        {
            AudioManager.instance.StopSFX(prefabAudioSource);
        }
    }
}
