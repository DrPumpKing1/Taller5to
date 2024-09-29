using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicsTransitionPanelUIHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator transitionPanelUIAnimator;

    [Header("Settings")]
    [SerializeField, Range(0.5f, 2f)] private float fullBlackTime;

    public float FullBlackTime => fullBlackTime;    

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
