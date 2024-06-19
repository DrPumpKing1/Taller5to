using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePauseHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool pauseAudioSourceOnPause;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private AudioSource audioSource;

    private void OnEnable()
    {
        PauseManager.OnGamePaused += PauseManager_OnGamePaused;
        PauseManager.OnGameResumed += PauseManager_OnGameResumed;
    }

    private void OnDisable()
    {
        PauseManager.OnGamePaused -= PauseManager_OnGamePaused;
        PauseManager.OnGameResumed -= PauseManager_OnGameResumed;
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void PauseAudioSource()
    {
        if (!pauseAudioSourceOnPause) return;
        audioSource.Pause();
    }

    private void ResumeAudioSource()
    {
        if (!pauseAudioSourceOnPause) return;
        audioSource.UnPause();
    }

    private void PauseManager_OnGamePaused(object sender, System.EventArgs e)
    {
        PauseAudioSource();
    }

    private void PauseManager_OnGameResumed(object sender, System.EventArgs e)
    {
        ResumeAudioSource();
    }
}
