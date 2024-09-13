using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalPageUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator journalPageAnimator;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private const string SHOWING_ANIMATION = "JournalPageUIShowing";
    private const string HIDDEN_ANIMATION = "JournalPageUItHidden";

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
}
