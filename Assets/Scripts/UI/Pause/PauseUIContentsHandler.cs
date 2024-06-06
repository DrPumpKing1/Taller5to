using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUIContentsHandler : MonoBehaviour
{
    [Header("Contents")]
    [SerializeField] private Animator mainContentAnimator;
    [SerializeField] private Animator inGameOptionsContentAnimator;
    [SerializeField] private Animator inGameAudioOptionsContentAnimator;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    public void ShowMainContent()
    {
        mainContentAnimator.ResetTrigger(HIDE_TRIGGER);

        mainContentAnimator.SetTrigger(SHOW_TRIGGER);
        inGameOptionsContentAnimator.SetTrigger(HIDE_TRIGGER);
        inGameAudioOptionsContentAnimator.SetTrigger(HIDE_TRIGGER);
    }

    public void ShowInGameOptionsContent()
    {
        inGameOptionsContentAnimator.ResetTrigger(HIDE_TRIGGER);

        inGameOptionsContentAnimator.SetTrigger(SHOW_TRIGGER);
        mainContentAnimator.SetTrigger(HIDE_TRIGGER);
        inGameAudioOptionsContentAnimator.SetTrigger(HIDE_TRIGGER);
    }

    public void ShowInGameAudioOptionsContent()
    {
        inGameAudioOptionsContentAnimator.ResetTrigger(HIDE_TRIGGER);

        inGameAudioOptionsContentAnimator.SetTrigger(SHOW_TRIGGER);
        mainContentAnimator.SetTrigger(HIDE_TRIGGER);
        inGameOptionsContentAnimator.SetTrigger(HIDE_TRIGGER);
    }
}
