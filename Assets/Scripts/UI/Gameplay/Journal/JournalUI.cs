using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class JournalUI : BaseUI
{
    [Header("Components")]
    [SerializeField] private Animator journalUIAnimator;

    [Header("UI Components")]
    [SerializeField] private Button closeButton;

    private CanvasGroup canvasGroup;

    public static event EventHandler OnCloseFromUI;
    public static event EventHandler OnJournalUIOpen;
    public static event EventHandler OnJournalUIClose;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    protected override void OnEnable()
    {
        base.OnEnable();
        JournalOpeningManager.OnJournalOpen += JournalOpeningManager_OnJournalOpen;
        JournalOpeningManager.OnJournalClose += JournalOpeningManager_OnJournalClose;
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

        ShowInventoryUI();

        OnJournalUIOpen?.Invoke(this, EventArgs.Empty);
    }

    private void CloseUI()
    {
        if (state != State.Open) return;

        SetUIState(State.Closed);

        RemoveFromUILayersList();

        HideInventoryUI();

        OnJournalUIClose?.Invoke(this, EventArgs.Empty);
    }

    protected override void CloseFromUI()
    {
        OnCloseFromUI?.Invoke(this, EventArgs.Empty);
    }

    public void ShowInventoryUI()
    {
        journalUIAnimator.ResetTrigger(HIDE_TRIGGER);
        journalUIAnimator.SetTrigger(SHOW_TRIGGER);
    }

    public void HideInventoryUI()
    {
        journalUIAnimator.ResetTrigger(SHOW_TRIGGER);
        journalUIAnimator.SetTrigger(HIDE_TRIGGER);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        JournalOpeningManager.OnJournalOpen += JournalOpeningManager_OnJournalOpen;
        JournalOpeningManager.OnJournalClose += JournalOpeningManager_OnJournalClose;
    }

    #region JournalOpeningManager Subscriptions

    private void JournalOpeningManager_OnJournalOpen(object sender, EventArgs e)
    {
        OpenUI();
    }

    private void JournalOpeningManager_OnJournalClose(object sender, EventArgs e)
    {
        CloseUI();
    }
    #endregion
}