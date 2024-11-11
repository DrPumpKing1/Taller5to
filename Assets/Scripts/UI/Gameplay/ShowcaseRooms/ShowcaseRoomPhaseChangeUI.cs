using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowcaseRoomPhaseChangeUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator phaseChangeUIAnimator;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private void OnEnable()
    {
        ShowcaseRoomStateHandler.OnShowcaseRoomPhaseChangePreMid += ShowcaseRoomStateHandler_OnShowcaseRoomPhaseChangePreMid;
        ShowcaseRoomStateHandler.OnShowcaseRoomPhaseChangePostMid += BossStateHandler_OnShowcaseRoomPhaseChangePostMid;
    }

    private void OnDisable()
    {
        ShowcaseRoomStateHandler.OnShowcaseRoomPhaseChangePreMid -= ShowcaseRoomStateHandler_OnShowcaseRoomPhaseChangePreMid;
        ShowcaseRoomStateHandler.OnShowcaseRoomPhaseChangePostMid -= BossStateHandler_OnShowcaseRoomPhaseChangePostMid;
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
    private void ShowcaseRoomStateHandler_OnShowcaseRoomPhaseChangePreMid(object sender, ShowcaseRoomStateHandler.OnPhaseChangeEventArgs e)
    {
        ShowUI();
    }
    private void BossStateHandler_OnShowcaseRoomPhaseChangePostMid(object sender, ShowcaseRoomStateHandler.OnPhaseChangeEventArgs e)
    {
        HideUI();
    }
    #endregion
}
