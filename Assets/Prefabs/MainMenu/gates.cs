using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gates : MonoBehaviour
{
    private SoundPlayer soundPlayer;

    private void Awake()
    {
        soundPlayer = GetComponent<SoundPlayer>();
       

    }

    // Method to play sound, triggered by an animation event
    public void PlayGateSound()
    {
        soundPlayer.PlaySound(0);
        
    }
}
