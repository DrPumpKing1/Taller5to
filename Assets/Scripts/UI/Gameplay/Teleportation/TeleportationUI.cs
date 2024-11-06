using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator teleportationUIAnimator;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private void OnEnable()
    {
        PlayerTeleportationManager.OnTeleportationStarted += PlayerTeleportationManager_OnTeleportationStarted;
        PlayerTeleportationManager.OnTeleportationEnded += PlayerTeleportationManager_OnTeleportationEnded;
    }

    private void OnDisable()
    {
        PlayerTeleportationManager.OnTeleportationStarted -= PlayerTeleportationManager_OnTeleportationStarted;
        PlayerTeleportationManager.OnTeleportationEnded -= PlayerTeleportationManager_OnTeleportationEnded;
    }

    private void ShowUI()
    {
        teleportationUIAnimator.ResetTrigger(HIDE_TRIGGER);
        teleportationUIAnimator.SetTrigger(SHOW_TRIGGER);
    }

    private void HideUI()
    {
        teleportationUIAnimator.ResetTrigger(SHOW_TRIGGER);
        teleportationUIAnimator.SetTrigger(HIDE_TRIGGER);
    }

    #region  PlayerTeleportationManager Subscriptions
    private void PlayerTeleportationManager_OnTeleportationStarted(object sender, PlayerTeleportationManager.OnTeleportationEventArgs e)
    {
        ShowUI();
    }
    private void PlayerTeleportationManager_OnTeleportationEnded(object sender, PlayerTeleportationManager.OnTeleportationEventArgs e)
    {
        HideUI();
    }
    #endregion
}
