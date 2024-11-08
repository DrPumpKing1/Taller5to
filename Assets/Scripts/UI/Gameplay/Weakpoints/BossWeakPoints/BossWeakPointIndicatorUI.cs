using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeakPointIndicatorUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private BossWeakPoint bossWeakPoint;
    [SerializeField] private Animator indicatorUIAnimator;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private void OnEnable()
    {
        bossWeakPoint.OnWeakPointEnable += BossWeakPoint_OnWeakPointEnable;
        bossWeakPoint.OnWeakPointDisable += BossWeakPoint_OnWeakPointDisable;
    }
    private void OnDisable()
    {
        bossWeakPoint.OnWeakPointEnable -= BossWeakPoint_OnWeakPointEnable;
        bossWeakPoint.OnWeakPointDisable -= BossWeakPoint_OnWeakPointDisable;
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

    #region BossWeakPoint Subscriptions
    private void BossWeakPoint_OnWeakPointEnable(object sender, System.EventArgs e)
    {
        ShowWeakpoint();
    }
    private void BossWeakPoint_OnWeakPointDisable(object sender, System.EventArgs e)
    {
        HideWeakpoint();
    }

    #endregion
}
