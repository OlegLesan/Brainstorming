using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHorde : MonoBehaviour
{
    private SoundPlayer soundPlayer;

    private void Awake()
    {
        soundPlayer = GetComponent<SoundPlayer>();


    }

    // Method to play sound, triggered by an animation event
    public void Start()
    {
        soundPlayer.PlaySound(0);

    }
}
