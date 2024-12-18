using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public int[] soundIndexes;
    public int priority = 128;
    public float volumeMultiplier = 1f;
    public bool loop = false;
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;

    private List<AudioSource> activeAudioSources = new List<AudioSource>();

    public void PlaySound(int index)
    {
        if (AudioManager.instance != null && index >= 0 && index < soundIndexes.Length)
        {
            int soundIndex = soundIndexes[index];
            if (soundIndex >= 0 && soundIndex < AudioManager.instance.sfx.Length)
            {
                AudioSource sfxSource = AudioManager.instance.sfx[soundIndex];
                if (sfxSource != null)
                {
                    AudioSource tempSource = gameObject.AddComponent<AudioSource>();
                    tempSource.clip = sfxSource.clip;
                    tempSource.volume = sfxSource.volume * AudioManager.instance.sfxVolume * volumeMultiplier;
                    tempSource.pitch = Random.Range(minPitch, maxPitch);
                    tempSource.loop = loop;
                    tempSource.spatialBlend = sfxSource.spatialBlend;
                    tempSource.priority = priority;
                    tempSource.outputAudioMixerGroup = sfxSource.outputAudioMixerGroup;

                    tempSource.Play();

                    activeAudioSources.Add(tempSource);

                    if (!loop)
                    {
                        Destroy(tempSource, tempSource.clip.length);
                        activeAudioSources.Remove(tempSource);
                    }
                }
            }
        }
    }

    public void UpdateVolume()
    {
        foreach (var source in activeAudioSources)
        {
            if (source != null)
            {
                source.volume = AudioManager.instance.sfxVolume * volumeMultiplier;
            }
        }
    }

    private void OnDestroy()
    {
        foreach (var source in activeAudioSources)
        {
            if (source != null)
            {
                Destroy(source);
            }
        }
        activeAudioSources.Clear();
    }
}
