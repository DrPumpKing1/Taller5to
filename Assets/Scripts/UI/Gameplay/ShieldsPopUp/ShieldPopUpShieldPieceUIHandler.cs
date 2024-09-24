using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPopUpShieldPieceUIHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ShieldPieceSO shieldPieceSO;

    [Header("Components")]
    [SerializeField] private Animator shieldsPopUpPieceUIAnimator;

    public ShieldPieceSO ShieldPieceSO => shieldPieceSO;
    public bool OnInventory {  get; private set; }

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private const string HIDDEN_ANIMATION = "Hidden";
    private const string SHOWING_ANIMATION = "Showing";

    private void CheckShieldPieceImageState()
    {
        if (ShieldPiecesManager.Instance.ShieldPiecesCollected.Contains(shieldPieceSO))
        {
            OnInventory = true;
        }
        else
        {
            OnInventory = false;
        }
    }

    private void ShowPieceUI()
    {
        shieldsPopUpPieceUIAnimator.ResetTrigger(HIDE_TRIGGER);
        shieldsPopUpPieceUIAnimator.SetTrigger(SHOW_TRIGGER);
    }
    private void HidePieceUI()
    {
        shieldsPopUpPieceUIAnimator.ResetTrigger(SHOW_TRIGGER);
        shieldsPopUpPieceUIAnimator.SetTrigger(HIDE_TRIGGER);
    }

    private void ShowPieceUIInmediately()
    {
        shieldsPopUpPieceUIAnimator.Play(SHOWING_ANIMATION);
    }

    private void HidePieceUIInmediately()
    {
        shieldsPopUpPieceUIAnimator.Play(HIDDEN_ANIMATION);
    }
}
