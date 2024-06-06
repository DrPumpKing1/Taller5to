using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPauseHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool pauseMusicOnPause;

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

    private void PauseMusic()
    {
        if (!pauseMusicOnPause) return;
        audioSource.Pause();
    }

    private void ResumeMusic()
    {
        if (!pauseMusicOnPause) return;
        audioSource.UnPause();
    }

    private void PauseManager_OnGamePaused(object sender, System.EventArgs e)
    {
        PauseMusic();
    }

    private void PauseManager_OnGameResumed(object sender, System.EventArgs e)
    {
        ResumeMusic();
    }
}
