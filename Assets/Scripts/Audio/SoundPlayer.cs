using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [Tooltip("������ �������� ������ � AudioManager.sfx")]
    public int[] soundIndexes;

    private AudioSource prefabAudioSource;

    private void Awake()
    {
        // �������� ��� ��������� ��������� AudioSource �� ���� ������
        prefabAudioSource = GetComponent<AudioSource>();
        if (prefabAudioSource == null)
        {
            prefabAudioSource = gameObject.AddComponent<AudioSource>();
        }
    }

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

                // ��������� � ��������������� ����� ��������� AudioSource
                AudioManager.instance.ConfigureSFX(soundIndex, prefabAudioSource);
                prefabAudioSource.Play();
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
