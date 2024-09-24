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

    private void OnEnable()
    {
        ShieldPiecesManager.OnShieldPieceCollected += ShieldPiecesManager_OnShieldPieceCollected;
    }
    private void OnDisable()
    {
        ShieldPiecesManager.OnShieldPieceCollected -= ShieldPiecesManager_OnShieldPieceCollected;
    }

    private void Start()
    {
        CheckShieldPieceState();
    }

    private void CheckShieldPieceState()
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

    public void ShowPieceUI()
    {
        shieldsPopUpPieceUIAnimator.ResetTrigger(HIDE_TRIGGER);
        shieldsPopUpPieceUIAnimator.SetTrigger(SHOW_TRIGGER);
    }
    public void HidePieceUI()
    {
        shieldsPopUpPieceUIAnimator.ResetTrigger(SHOW_TRIGGER);
        shieldsPopUpPieceUIAnimator.SetTrigger(HIDE_TRIGGER);
    }

    public void ShowPieceUIInmediately()
    {
        shieldsPopUpPieceUIAnimator.Play(SHOWING_ANIMATION);
    }

    public void HidePieceUIInmediately()
    {
        shieldsPopUpPieceUIAnimator.Play(HIDDEN_ANIMATION);
    }

    private void ShieldPiecesManager_OnShieldPieceCollected(object sender, ShieldPiecesManager.OnShieldPieceCollectedEventArgs e)
    {
        CheckShieldPieceState();
    }
}
