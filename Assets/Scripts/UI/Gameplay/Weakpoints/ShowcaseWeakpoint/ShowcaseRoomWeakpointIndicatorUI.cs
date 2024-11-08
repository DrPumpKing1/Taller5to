using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowcaseRoomWeakpointIndicatorUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ShowcaseRoomWeakpoint showcaseRoomWeakpoint;
    [SerializeField] private Animator indicatorUIAnimator;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private void OnEnable()
    {
        showcaseRoomWeakpoint.OnWeakPointEnable += ShowcaseRoom_OnWeakPointEnable;
        showcaseRoomWeakpoint.OnWeakPointDisable += ShowcaseRoom_OnWeakPointDisable;
    }
    private void OnDisable()
    {
        showcaseRoomWeakpoint.OnWeakPointEnable -= ShowcaseRoom_OnWeakPointEnable;
        showcaseRoomWeakpoint.OnWeakPointDisable -= ShowcaseRoom_OnWeakPointDisable;
    }


    private void ShowWeakpoint()
    {
        indicatorUIAnimator.ResetTrigger(HIDE_TRIGGER);
        indicatorUIAnimator.SetTrigger(SHOW_TRIGGER);
    }

    private void HideWeakpoint()
    {
        indicatorUIAnimator.ResetTrigger(SHOW_TRIGGER);
        indicatorUIAnimator.SetTrigger(HIDE_TRIGGER);
    }

    #region ShowcaseRoomWeakpoint Subscriptions
    private void ShowcaseRoom_OnWeakPointEnable(object sender, System.EventArgs e)
    {
        ShowWeakpoint();
    }
    private void ShowcaseRoom_OnWeakPointDisable(object sender, System.EventArgs e)
    {
        HideWeakpoint();
    }

    #endregion
}
