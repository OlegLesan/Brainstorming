using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHorde : MonoBehaviour
{
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();

        // Play the sound at the start of the game
        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource component is missing on the GameObject.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
