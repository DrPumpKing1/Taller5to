using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicsTransitionPanelUIHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator transitionPanelUIAnimator;

    [Header("Settings")]
    [SerializeField, Range(0.5f, 2f)] private float fullBlackTime;
    [SerializeField, Range(0.5f, 2f)] private float fullBlackTimeAfterPlayStart;

    public float FullBlackTime => fullBlackTime;
    public float FullBlackTimeAfterPlayStart => fullBlackTimeAfterPlayStart;
    public float TransitionTime => TRANSITION_TIME;

    private const float TRANSITION_TIME = 0.5f;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    public void ShowTransitionPanel()
    {
        transitionPanelUIAnimator.ResetTrigger(HIDE_TRIGGER);
        transitionPanelUIAnimator.SetTrigger(SHOW_TRIGGER);
    }

    public void HideTransitionPanel()
    {
        transitionPanelUIAnimator.ResetTrigger(SHOW_TRIGGER);
        transitionPanelUIAnimator.SetTrigger(HIDE_TRIGGER);
    }
}
