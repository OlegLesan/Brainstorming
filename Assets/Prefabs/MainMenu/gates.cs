using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gates : MonoBehaviour
{
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource component is missing on the GameObject.");
        }
    }

    // Method to play sound, triggered by an animation event
    public void PlayGateSound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
