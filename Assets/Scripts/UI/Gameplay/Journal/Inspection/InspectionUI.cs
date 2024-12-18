using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectionUI : BaseUI
{
    [Header("Components")]
    [SerializeField] private Animator journalInfoPopUpUIAnimator;
    [SerializeField] private Button closeButton;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    public static event EventHandler OnInspectionUIOpen;
    public static event EventHandler OnInspectionUIClose;

    public static event EventHandler OnInspectionUICloseFromUI;

    protected override void OnEnable()
    {
        base.OnEnable();
        InspectableJournalInfoPopUpHandler.OnInspectionUIOpen += InspectableJournalInfoPopUpHandler_OnInspectionUIOpen;
        JournalPagesHandler.OnJournalPageButtonEffectiveClick += JournalPagesHandler_OnJournalPageButtonEffectiveClick;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        InspectableJournalInfoPopUpHandler.OnInspectionUIOpen -= InspectableJournalInfoPopUpHandler_OnInspectionUIOpen;
        JournalPagesHandler.OnJournalPageButtonEffectiveClick -= JournalPagesHandler_OnJournalPageButtonEffectiveClick;
    }

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void Start()
    {
        SetUIState(State.Closed);
    }

    private void InitializeButtonsListeners()
    {
        closeButton.onClick.AddListener(CloseFromUI);
    }

    private void OpenUI()
    {
        if (state != State.Closed) return;

        SetUIState(State.Open);

        AddToUILayersList();

        ShowPopUpUI();

        OnInspectionUIOpen?.Invoke(this, EventArgs.Empty);
    }

    private void CloseUI()
    {
        if (state != State.Open) return;

        SetUIState(State.Closed);

        RemoveFromUILayersList();

        HidePopUpUI();

        OnInspectionUIClose?.Invoke(this, EventArgs.Empty);

    }

    protected override void CloseFromUI()
    {
        CloseUI();
        OnInspectionUICloseFromUI?.Invoke(this, EventArgs.Empty);
    }

    public void CloseFromPhysicalButtonClick()
    {
        CloseUI();
    }

    public void ShowPopUpUI()
    {
        journalInfoPopUpUIAnimator.ResetTrigger(HIDE_TRIGGER);
        journalInfoPopUpUIAnimator.SetTrigger(SHOW_TRIGGER);
    }

    public void HidePopUpUI()
    {
        journalInfoPopUpUIAnimator.ResetTrigger(SHOW_TRIGGER);
        journalInfoPopUpUIAnimator.SetTrigger(HIDE_TRIGGER);
    }

    #region InspectableJournalInfoPopUpHandler Subscriptions
    private void InspectableJournalInfoPopUpHandler_OnInspectionUIOpen(object sender, InspectableJournalInfoPopUpHandler.OnInspectionUIOpenEventArgs e)
    {
        OpenUI();
    }
    #endregion

    #region JournalPagesHandler Subscriptions
    private void JournalPagesHandler_OnJournalPageButtonEffectiveClick(object sender, EventArgs e)
    {
        if (state == State.Closed) return;
        if (state == State.Closing) return;

        CloseFromPhysicalButtonClick();
    }
    #endregion
}
