using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalPageUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private List<JournalInfoUI> journalInfoUIs;
    [SerializeField] private GameObject pageNotCheckedIndicator;
    [Space]
    [SerializeField] private Animator journalPageAnimator;

    [Header("Scroll Components")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private Image scrollBarImage;
    [SerializeField] private Image scrollBarHandleImage;
    
    [Header("Settings")]
    [SerializeField] private bool scrollable;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private const string SHOWING_ANIMATION = "JournalPageUIShowing";
    private const string HIDDEN_ANIMATION = "JournalPageUIHidden";

    private void OnEnable()
    {
        JournalInfoManager.OnJournalInfoCollected += JournalInfoManager_OnJournalInfoCollected;
        JournalInfoManager.OnJournalInfoChecked += JournalInfoManager_OnJournalInfoChecked;
    }

    private void OnDisable()
    {
        JournalInfoManager.OnJournalInfoCollected -= JournalInfoManager_OnJournalInfoCollected;
        JournalInfoManager.OnJournalInfoChecked -= JournalInfoManager_OnJournalInfoChecked;
    }

    private void Start()
    {
        CheckJournalPageState();
        SetScrollable(scrollable);
    }

    private void CheckJournalPageState()
    {
        CheckShowIndicator();
        CheckHideIndicator();
    }

    #region Show & Hide Page
    public void ShowPage()
    {
        journalPageAnimator.ResetTrigger(HIDE_TRIGGER);
        journalPageAnimator.SetTrigger(SHOW_TRIGGER);
    }

    public void HidePage()
    {
        journalPageAnimator.ResetTrigger(SHOW_TRIGGER);
        journalPageAnimator.SetTrigger(HIDE_TRIGGER);
    }

    public void ShowPageInmediately()
    {
        journalPageAnimator.ResetTrigger(HIDE_TRIGGER);
        journalPageAnimator.ResetTrigger(SHOW_TRIGGER);

        journalPageAnimator.Play(SHOWING_ANIMATION);
    }
    public void HidePageInmediately()
    {
        journalPageAnimator.ResetTrigger(HIDE_TRIGGER);
        journalPageAnimator.ResetTrigger(SHOW_TRIGGER);

        journalPageAnimator.Play(HIDDEN_ANIMATION);
    }
    #endregion

    #region Show & Hide Indicator

    public void ShowNotCheckedIndicator()
    {
        pageNotCheckedIndicator.SetActive(true);
    }

    public void HideNotCheckedIndicator()
    {
        pageNotCheckedIndicator.SetActive(false);
    }

    private void CheckShowIndicator()
    {
        foreach (JournalInfoUI journalInfoUI in journalInfoUIs)
        {
            JournalInfoManager.JournalInfoCheck journalInfoCheck = JournalInfoManager.Instance.GetJournalInfoCheckInJournal(journalInfoUI.JournalInfoSO);
            if (journalInfoCheck == null) continue;

            if (!journalInfoCheck.hasBeenChecked)
            {
                ShowNotCheckedIndicator();
                return;
            }
        }
    }
    private void CheckHideIndicator()
    {
        if(journalInfoUIs.Count <= 0)
        {
            HideNotCheckedIndicator();
            return;
        }

        foreach (JournalInfoUI journalInfoUI in journalInfoUIs)
        {
            JournalInfoManager.JournalInfoCheck journalInfoCheck = JournalInfoManager.Instance.GetJournalInfoCheckInJournal(journalInfoUI.JournalInfoSO);
            if (journalInfoCheck == null) continue;
            if (!journalInfoCheck.hasBeenChecked) return;
        }

        HideNotCheckedIndicator();
    }

    #endregion

    private void SetScrollable(bool scrollable)
    {
        scrollRect.enabled = scrollable;
        scrollbar.enabled = scrollable;
        scrollBarImage.enabled = scrollable;
        scrollBarHandleImage.enabled = scrollable;
    }

    #region JournalInfoManager Susbcriptions
    private void JournalInfoManager_OnJournalInfoCollected(object sender, JournalInfoManager.OnJournalInfoEventArgs e)
    {
        CheckShowIndicator();
    }

    private void JournalInfoManager_OnJournalInfoChecked(object sender, JournalInfoManager.OnJournalInfoEventArgs e)
    {
        CheckHideIndicator();
    }
    #endregion
}
