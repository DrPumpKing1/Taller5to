using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDAnimationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private HUDVisibilityHandler HUDVisibilityHandler;

    private Animator animator;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private void OnEnable()
    {
        HUDVisibilityHandler.OnShowHUD += HUDVisibilityHandler_OnShowHUD;
        HUDVisibilityHandler.OnHideHUD += HUDVisibilityHandler_OnHideHUD;
        HUDVisibilityHandler.OnShowHUDFirstTime += HUDVisibilityHandler_OnShowHUDFirstTime;
    }

    private void OnDisable()
    {
        HUDVisibilityHandler.OnShowHUD -= HUDVisibilityHandler_OnShowHUD;
        HUDVisibilityHandler.OnHideHUD -= HUDVisibilityHandler_OnHideHUD;
        HUDVisibilityHandler.OnShowHUDFirstTime -= HUDVisibilityHandler_OnShowHUDFirstTime;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void HUDVisibilityHandler_OnShowHUD(object sender, System.EventArgs e) => animator.SetTrigger(SHOW_TRIGGER);
    private void HUDVisibilityHandler_OnHideHUD(object sender, System.EventArgs e) => animator.SetTrigger(HIDE_TRIGGER);
    private void HUDVisibilityHandler_OnShowHUDFirstTime(object sender, System.EventArgs e) => animator.SetTrigger(SHOW_TRIGGER);
}
