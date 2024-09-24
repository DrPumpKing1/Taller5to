using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShieldsPopUpUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator shieldsPopUpUIAnimator;

    [Header("Settings")]
    [SerializeField, Range(2f, 10f)] private float timeShowingPopUp;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private const float SHOW_TIME = 1f;
    private const float HIDE_TIME = 1f;

    public static event EventHandler<OnShieldPopUpEventArgs> OnShieldPopUpShow;
    public static event EventHandler<OnShieldPopUpEventArgs> OnShieldPopUpHide;

    public class OnShieldPopUpEventArgs: EventArgs
    {
        public ShieldPieceSO shieldPieceSO;
    }

    private void OnEnable()
    {
        ShieldPiecesManager.OnShieldPieceCollected += ShieldPiecesManager_OnShieldPieceCollected;
    }

    private void OnDisable()
    {
        ShieldPiecesManager.OnShieldPieceCollected -= ShieldPiecesManager_OnShieldPieceCollected;
    }

    private void ShowPopUp()
    {
        shieldsPopUpUIAnimator.ResetTrigger(HIDE_TRIGGER);
        shieldsPopUpUIAnimator.SetTrigger(SHOW_TRIGGER);
    }
    private void HidePopUp()
    {
        shieldsPopUpUIAnimator.ResetTrigger(SHOW_TRIGGER);
        shieldsPopUpUIAnimator.SetTrigger(HIDE_TRIGGER);
    }

    private IEnumerator PopUpShieldCoroutine(ShieldPieceSO shieldPieceSO)
    {
        OnShieldPopUpShow?.Invoke(this, new OnShieldPopUpEventArgs { shieldPieceSO = shieldPieceSO });

        ShowPopUp();
        yield return new WaitForSeconds(SHOW_TIME);
        yield return new WaitForSeconds(timeShowingPopUp);
        HidePopUp();
        yield return new WaitForSeconds(HIDE_TIME);

        OnShieldPopUpHide?.Invoke(this, new OnShieldPopUpEventArgs { shieldPieceSO = shieldPieceSO });
    }

    private void ShieldPiecesManager_OnShieldPieceCollected(object sender, ShieldPiecesManager.OnShieldPieceCollectedEventArgs e)
    {
        StartCoroutine(PopUpShieldCoroutine(e.shieldPieceSO));
    }
}
