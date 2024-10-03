using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhaseChangeUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator phaseChangeUIAnimator;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private void OnEnable()
    {
        BossStateHandler.OnBossPhaseChangePreMid += BossStateHandler_OnBossPhaseChangePreMid;
        BossStateHandler.OnBossPhaseChangePostMid += BossStateHandler_OnBossPhaseChangePostMid;
    }

    private void OnDisable()
    {
        BossStateHandler.OnBossPhaseChangePreMid -= BossStateHandler_OnBossPhaseChangePreMid;
        BossStateHandler.OnBossPhaseChangePostMid -= BossStateHandler_OnBossPhaseChangePostMid;
    }

    private void ShowUI()
    {
        phaseChangeUIAnimator.ResetTrigger(HIDE_TRIGGER);
        phaseChangeUIAnimator.SetTrigger(SHOW_TRIGGER);
    }

    private void HideUI()
    {
        phaseChangeUIAnimator.ResetTrigger(SHOW_TRIGGER);
        phaseChangeUIAnimator.SetTrigger(HIDE_TRIGGER);
    }

    #region  BossStateHandler Subscriptions
    private void BossStateHandler_OnBossPhaseChangePreMid(object sender, BossStateHandler.OnPhaseChangeEventArgs e)
    {
        ShowUI();
    }
    private void BossStateHandler_OnBossPhaseChangePostMid(object sender, BossStateHandler.OnPhaseChangeEventArgs e)
    {
        HideUI();
    }
    #endregion
}
