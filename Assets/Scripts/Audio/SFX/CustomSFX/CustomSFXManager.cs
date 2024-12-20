using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSFXManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected SFXPoolSO SFXPoolSO;

    [Header("Settings")]
    [SerializeField] protected bool stopOnPause;

    [Header("Debug")]
    [SerializeField] protected bool debug;

    protected AudioSource audioSource;

    protected virtual void OnEnable()
    {
        PauseManager.OnGamePaused += PauseManager_OnGamePaused;
        PauseManager.OnGameResumed += PauseManager_OnGameResumed;
    }

    protected virtual void OnDisable()
    {
        PauseManager.OnGamePaused -= PauseManager_OnGamePaused;
        PauseManager.OnGameResumed -= PauseManager_OnGameResumed;
    }

    protected void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected void StopAudioSource()
    {
        audioSource.Stop();
        audioSource.clip = null;
    }

    protected void PauseAudioSource()
    {
        if (audioSource.clip == null) return;
        audioSource.Pause();
    }

    protected void ResumeAudioSource()
    {
        if (audioSource.clip == null) return;
        audioSource.Play();
    }

    protected void ReplaceAudioClip(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = null;

        if (!clip)
        {
            if (debug) Debug.Log($"The clip {clip.name} is null");
            return;
        }

        audioSource.clip = clip;
        audioSource.Play();
    }

    protected void RepositionSFXManager(Vector3 position)
    {
        transform.position = position;
    }

    #region PauseManager Subscriptions
    private void PauseManager_OnGamePaused(object sender, System.EventArgs e)
    {
        if (stopOnPause) StopAudioSource();
        else PauseAudioSource();
    }
    private void PauseManager_OnGameResumed(object sender, System.EventArgs e)
    {
        if (stopOnPause) StopAudioSource();
        else ResumeAudioSource();
    }
    #endregion
}
