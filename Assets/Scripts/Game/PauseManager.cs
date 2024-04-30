using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private UIInput UIInput;

    public static event EventHandler OnGamePaused;
    public static event EventHandler OnGameResumed;

    private bool PauseInput => UIInput.GetPauseDown();
    public bool GamePaused { get; private set; }

    public bool GamePausedThisFrame { get; private set; }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void Update()
    {
        CheckPauseResumeGame();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one PauseManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
    private void InitializeVariables()
    {
        GamePaused = false;
        GamePausedThisFrame = false;
    }

    private void CheckPauseResumeGame()
    {
        GamePausedThisFrame = false;

        if (!PauseInput) return;

        if (!GamePaused)
        {
            if (UIManager.Instance.UIActive) return;
            PauseGame();
            GamePausedThisFrame = true;
        }
        else
        {
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        OnGamePaused?.Invoke(this, EventArgs.Empty);
        Time.timeScale = 0f;
        GamePaused = true;
    }

    private void ResumeGame()
    {
        OnGameResumed?.Invoke(this, EventArgs.Empty);
        Time.timeScale = 1f;
        GamePaused = false;
    }
}
