using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private void Awake() => Instance = this;
        
    
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundFX( AudioClip audioClip) 
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
