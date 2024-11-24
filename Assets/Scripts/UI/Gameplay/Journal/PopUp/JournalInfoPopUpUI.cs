using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class JournalInfoPopUpUI : BaseUI
{
    [Header("Components")]
    [SerializeField] private Animator journalInfoPopUpUIAnimator;
    [SerializeField] private Button closeButton;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    public static event EventHandler OnJournalInfoPopUpOpen;
    public static event EventHandler OnJournalInfoPopUpClose;

    protected override void OnEnable()
    {
        base.OnEnable();
        JournalPagesHandler.OnJournalPageOpen += JournalPagesHandler_OnJournalPageOpen;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        JournalPagesHandler.OnJournalPageOpen -= JournalPagesHandler_OnJournalPageOpen;
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

    public void OpenUIFromButton() => OpenUI();

    private void OpenUI()
    {
        if (state != State.Closed) return;

        SetUIState(State.Open);

        AddToUILayersList();

        ShowPopUpUI();

        OnJournalInfoPopUpOpen?.Invoke(this, EventArgs.Empty);
    }

    private void CloseUI()
    {
        if (state != State.Open) return;

        SetUIState(State.Closed);

        RemoveFromUILayersList();

        HidePopUpUI();

        OnJournalInfoPopUpClose?.Invoke(this, EventArgs.Empty);
    }

    protected override void CloseFromUI()
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

    #region JournalPagesHandler Subscriptions
    private void JournalPagesHandler_OnJournalPageOpen(object sender, JournalPagesHandler.OnJournalPageEventArgs e)
    {
        if (JournalPagesHandler.Instance.JournalPagesHierarchyOverPopUps) CloseUI();
    }
    #endregion
}
