using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    AudioSource audioSource;
    public AudioClip dashSound;
    public AudioClip jumpSound;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    public void PlaySoundDash()
    {
        audioSource.PlayOneShot(dashSound);
    }

    public void PlaySoundJump()
    {
        audioSource.PlayOneShot(jumpSound);
    }
}
