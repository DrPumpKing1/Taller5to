using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsUIContentsHandler : MonoBehaviour
{
    [Header("Contents")]
    [SerializeField] private Animator mainContentAnimator;
    [SerializeField] private Animator audioContentAnimator;
    [SerializeField] private Animator graphicsContentAnimator;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    public void ShowMainContent()
    {
        mainContentAnimator.ResetTrigger(HIDE_TRIGGER);

        mainContentAnimator.SetTrigger(SHOW_TRIGGER);
        audioContentAnimator.SetTrigger(HIDE_TRIGGER);
        graphicsContentAnimator.SetTrigger(HIDE_TRIGGER);
    }
    public void ShowAudioContent()
    {
        audioContentAnimator.ResetTrigger(HIDE_TRIGGER);

        audioContentAnimator.SetTrigger(SHOW_TRIGGER);
        mainContentAnimator.SetTrigger(HIDE_TRIGGER);
        graphicsContentAnimator.SetTrigger(HIDE_TRIGGER);
    }

    public void ShowGraphicsContent()
    {
        graphicsContentAnimator.ResetTrigger(HIDE_TRIGGER);

        graphicsContentAnimator.SetTrigger(SHOW_TRIGGER);
        mainContentAnimator.SetTrigger(HIDE_TRIGGER);
        audioContentAnimator.SetTrigger(HIDE_TRIGGER);
    }
}
