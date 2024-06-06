using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InGameOptionsUI : BaseUI
{
    [Header("UI Components")]
    [SerializeField] private Button closeButton;

    private CanvasGroup canvasGroup;

    public static event EventHandler OnInGameOptionsUIOpen;
    public static event EventHandler OnInGameOptionsUIClose;

    protected override void OnEnable()
    {
        base.OnEnable();
        PauseUIButtonsHandler.OnOpenInGameOptionsUI += PauseUIButtonsHandler_OnOpenInGameOptionsUI;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        PauseUIButtonsHandler.OnOpenInGameOptionsUI -= PauseUIButtonsHandler_OnOpenInGameOptionsUI;
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

        OnInGameOptionsUIOpen?.Invoke(this, EventArgs.Empty);
    }

    private void CloseUI()
    {
        if (state != State.Open) return;

        SetUIState(State.Closed);

        RemoveFromUILayersList();

        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 0f);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        OnInGameOptionsUIClose?.Invoke(this, EventArgs.Empty);
    }

    protected override void CloseFromUI()
    {
        CloseUI();
    }

    #region PauseUI Subscriptions

    private void PauseUIButtonsHandler_OnOpenInGameOptionsUI(object sender, EventArgs e)
    {
        OpenUI();
    }
    #endregion
}
