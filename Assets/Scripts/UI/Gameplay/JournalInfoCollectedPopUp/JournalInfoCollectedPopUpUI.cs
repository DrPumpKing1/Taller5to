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
        ShowPopUp();
        yield return new WaitForSeconds(SHOW_TIME);

        yield return new WaitForSeconds(timeShowingPopUp);
        HidePopUp();

        yield return new WaitForSeconds(HIDE_TIME);
    }

    private void JournalInfoManager_OnJournalInfoCollected(object sender, JournalInfoManager.OnJournalInfoEventArgs e)
    {
        StopAllCoroutines();
        StartCoroutine(PopUpIndicatorCoroutine());
    }
}
