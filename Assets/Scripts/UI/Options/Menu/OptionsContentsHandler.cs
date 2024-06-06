using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsContentsHandler : MonoBehaviour
{
    [Header("Contents")]
    [SerializeField] private Animator mainContentAnimator;
    [SerializeField] private Animator audioContentAnimator;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    public void ShowMainContent()
    {
        mainContentAnimator.SetTrigger(SHOW_TRIGGER);
        audioContentAnimator.SetTrigger(HIDE_TRIGGER);
    }
    public void ShowAudioContent()
    {
        audioContentAnimator.SetTrigger(SHOW_TRIGGER);
        mainContentAnimator.SetTrigger(HIDE_TRIGGER);
    }
}
