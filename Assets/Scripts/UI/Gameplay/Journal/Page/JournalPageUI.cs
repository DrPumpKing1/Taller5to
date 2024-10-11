using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalPageUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private List<JournalInfoUI> journalInfoUIs;
    [SerializeField] private GameObject pageNotCheckedIndicator;
    [Space]
    [SerializeField] private Animator journalPageAnimator;

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
        foreach (JournalInfoUI journalInfoUI in journalInfoUIs)
        {
            JournalInfoManager.JournalInfoCheck journalInfoCheck = JournalInfoManager.Instance.GetJournalInfoCheckInJournal(journalInfoUI.JournalInfoSO);
            if (journalInfoCheck == null) continue;
            if (!journalInfoCheck.hasBeenChecked) return;
        }

        HideNotCheckedIndicator();
    }

    #endregion

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
