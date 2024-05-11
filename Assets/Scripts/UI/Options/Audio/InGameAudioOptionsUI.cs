using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InGameAudioOptionsUI : BaseUI
{
    [Header("UI Components")]
    [SerializeField] private Button closeButton;

    private CanvasGroup canvasGroup;

    public static event EventHandler OnInGameAudioOptionsUIOpen;
    public static event EventHandler OnInGameAudioOptionsUIClose;

    protected override void OnEnable()
    {
        base.OnEnable();
        InGameOptionsUIButtonsHandler.OnOpenInGameAudioOptionsUI += InGameOptionsUIButtonsHandler_OnOpenInGameAudioOptionsUI;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        InGameOptionsUIButtonsHandler.OnOpenInGameAudioOptionsUI -= InGameOptionsUIButtonsHandler_OnOpenInGameAudioOptionsUI;
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
        closeButton.onClick.AddListener(CloseFromUI);
    }

    private void InitializeVariables()
    {
        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 0f);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void OpenUI()
    {
        if (state != State.Closed) return;

        SetUIState(State.Open);

        AddToUILayersList();

        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 1f);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        OnInGameAudioOptionsUIOpen?.Invoke(this, EventArgs.Empty);
    }

    private void CloseUI()
    {
        if (state != State.Open) return;

        SetUIState(State.Closed);

        RemoveFromUILayersList();

        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 0f);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        OnInGameAudioOptionsUIClose?.Invoke(this, EventArgs.Empty);
    }

    protected override void CloseFromUI()
    {
        CloseUI();
    }

    #region InGameOptionsUI Subscriptions

    private void InGameOptionsUIButtonsHandler_OnOpenInGameAudioOptionsUI(object sender, EventArgs e)
    {
        OpenUI();
    }
    #endregion
}
