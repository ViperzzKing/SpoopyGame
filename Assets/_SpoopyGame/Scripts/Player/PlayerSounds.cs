using System;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] private AudioSource sprintSteps;
    [SerializeField] private AudioSource walkSteps;
    [SerializeField] public AudioSource flashlightSound;
    
    public void PlayAudio(AudioSource currentAudio)
    {
        if (!currentAudio.isPlaying)
            currentAudio.Play();
    }
    
    public void StopAudio(AudioSource previousAudio)
    {
        if (previousAudio.isPlaying)
            previousAudio.Stop();
    }

    public AudioSource GetCurrentAudioSource()
    {
        bool sprintState = BasicMovement.Instance.CurrentState == BasicMovement.PlayerState.Sprint;
        
        if (sprintState)
            return sprintSteps;

        return walkSteps;
    }
    
    public AudioSource GetPreviousAudioSource()
    {
        bool sprintState = BasicMovement.Instance.PreviousState == BasicMovement.PlayerState.Sprint;
        
        if (sprintState)
            return sprintSteps;

        return walkSteps;
    }

    public void ChangeAudioSource()
    {
        StopAudio(GetPreviousAudioSource());
        PlayAudio(GetCurrentAudioSource());
    }
}
