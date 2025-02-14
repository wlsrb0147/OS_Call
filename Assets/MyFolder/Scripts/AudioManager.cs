using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    [SerializeField] private AudioClip phoneCall;
    
    
    
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        instance = this;
    }

    public void PlayCall()
    {
        audioSource.Play();
    }

    public void StopCall()
    {
        audioSource.Stop();
    }

    public void AcceptCall()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(phoneCall);
    }
}
