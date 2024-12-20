using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalInfoCollectedPopUpUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator journalInfoCollectedPopUpUIAnimator;

    [Header("Settings")]
    [SerializeField, Range(2f, 10f)] private float timeShowingPopUp;

    [Header("States")]
    [SerializeField] private State state;

    private enum State { Hidden, ShowingIn, Showing, ShowingOut } 

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private const float SHOW_TIME = 1.5f;
    private const float HIDE_TIME = 1.5f;

    private void OnEnable()
    {
        JournalInfoManager.OnJournalInfoCollected += JournalInfoManager_OnJournalInfoCollected;
    }

    private void OnDisable()
    {
        JournalInfoManager.OnJournalInfoCollected -= JournalInfoManager_OnJournalInfoCollected;
    }

    private void Start()
    {
        SetState(State.Hidden);
    }

    private void SetState(State state) => this.state = state;

    private void ShowPopUp()
    {
        journalInfoCollectedPopUpUIAnimator.ResetTrigger(HIDE_TRIGGER);
        journalInfoCollectedPopUpUIAnimator.SetTrigger(SHOW_TRIGGER);
    }
    private void HidePopUp()
    {
        journalInfoCollectedPopUpUIAnimator.ResetTrigger(SHOW_TRIGGER);
        journalInfoCollectedPopUpUIAnimator.SetTrigger(HIDE_TRIGGER);
    }

    private IEnumerator PopUpIndicatorCoroutine()
    {
        SetState(State.ShowingIn);

        ShowPopUp();
        yield return new WaitForSeconds(SHOW_TIME);

        SetState(State.Showing);

        yield return new WaitForSeconds(timeShowingPopUp);

        SetState(State.ShowingOut);

        HidePopUp();

        yield return new WaitForSeconds(HIDE_TIME);

        SetState(State.Hidden);
    }

    private void JournalInfoManager_OnJournalInfoCollected(object sender, JournalInfoManager.OnJournalInfoEventArgs e)
    {
        if (state != State.Hidden) return;
        StopAllCoroutines();
        StartCoroutine(PopUpIndicatorCoroutine());
    }
}
