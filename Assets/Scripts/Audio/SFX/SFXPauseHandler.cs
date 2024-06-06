using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPauseHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool pauseSFXOnPause;

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
        PauseManager.OnGamePaused += PauseManager_OnGamePaused;
        PauseManager.OnGameResumed += PauseManager_OnGameResumed;
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void PauseGlobalSFX()
    {
        if (!pauseSFXOnPause) return;
        audioSource.Pause();
    }

    private void ResumeGlobalSFX()
    {
        if (!pauseSFXOnPause) return;
        audioSource.UnPause();
    }

    private void PauseAllTemporalSFX()
    {
        if (!pauseSFXOnPause) return;

        TemporalSFXController[] temporalSFXControllers = FindObjectsOfType<TemporalSFXController>();

        foreach (TemporalSFXController temporalSFXController in temporalSFXControllers)
        {
            AudioSource audioSource = temporalSFXController.GetComponent<AudioSource>();

            if (!audioSource)
            {
                if (debug) Debug.LogWarning("TemporalSFX does not have an audiosource component");
                continue;
            }

            audioSource.Pause();
        }
    }

    private void ResumeAllTemporalSFX()
    {
        if (!pauseSFXOnPause) return;

        TemporalSFXController[] temporalSFXControllers = FindObjectsOfType<TemporalSFXController>();

        foreach (TemporalSFXController temporalSFXController in temporalSFXControllers)
        {
            AudioSource audioSource = temporalSFXController.GetComponent<AudioSource>();

            if (!audioSource)
            {
                if (debug) Debug.LogWarning("TemporalSFX does not have an audiosource component");
                continue;
            }

            audioSource.UnPause();
        }
    }
    private void PauseManager_OnGamePaused(object sender, System.EventArgs e)
    {
        PauseGlobalSFX();
        PauseAllTemporalSFX();
    }

    private void PauseManager_OnGameResumed(object sender, System.EventArgs e)
    {
        ResumeGlobalSFX();
        ResumeAllTemporalSFX();
    }
}
