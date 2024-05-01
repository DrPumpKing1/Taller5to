using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PauseUI : BaseUI
{
    [Header("UI Components")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button inGameOptionsButton;

    private CanvasGroup canvasGroup;

    public static event EventHandler OnCloseFromUI;
    public static event EventHandler OnPauseUIOpen;
    public static event EventHandler OnPauseUIClose;

    protected override void OnEnable()
    {
        base.OnEnable();
        PauseManager.OnGamePaused += PauseManager_OnGamePaused;
        PauseManager.OnGameResumed += PauseManager_OnGameResumed;
    }

    protected override void OnDisable()
    {
        base.OnDisable(); 
        PauseManager.OnGamePaused -= PauseManager_OnGamePaused;
        PauseManager.OnGameResumed -= PauseManager_OnGameResumed;
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        InitializeButtonsListeners();
    }

    private void Start()
    {
        InitializeVariables();
        SetUIState(State.Closed);
    }
    private void InitializeButtonsListeners()
    {
        resumeButton.onClick.AddListener(CloseFromUI);
        inGameOptionsButton.onClick.AddListener(OpenInGameOptionsUI);
    }

    private void InitializeVariables()
    {
        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 0f);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    public void OpenUI()
    {
        if (state != State.Closed) return;

        SetUIState(State.Open);

        AddToUILayersList();

        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 1f);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        OnPauseUIOpen?.Invoke(this, EventArgs.Empty);
    }

    private void CloseUI()
    {
        if (state != State.Open) return;

        SetUIState(State.Closed);

        RemoveFromUILayersList();

        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 0f);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        OnPauseUIClose?.Invoke(this, EventArgs.Empty);
    }

    protected override void CloseFromUI()
    {
        OnCloseFromUI?.Invoke(this, EventArgs.Empty);
    }

    private void OpenInGameOptionsUI()
    {
        Debug.Log("InGameOptionsOpened");
    }

    #region PauseManager Subscriptions
    private void PauseManager_OnGamePaused(object sender, System.EventArgs e)
    {
        OpenUI();
    }

    private void PauseManager_OnGameResumed(object sender, System.EventArgs e)
    {
        CloseUI();
    }
    #endregion
}
