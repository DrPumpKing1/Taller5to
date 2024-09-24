using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldPopUpShieldUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image backgroundImage;

    [Header("Settings")]
    [SerializeField] private List<ShieldPopUpShieldPieceUIHandler> pieces;
    [SerializeField] private Dialect dialect;

    private void OnEnable()
    {
        ShieldsPopUpUI.OnShieldPopUpShow += ShieldsPopUpUI_OnShieldPopUpShow;
        ShieldsPopUpUI.OnShieldPopUpComplete += ShieldsPopUpUI_OnShieldPopUpComplete;
        ShieldsPopUpUI.OnShieldPopUpHide += ShieldsPopUpUI_OnShieldPopUpHide;
    }

    private void OnDisable()
    {
        ShieldsPopUpUI.OnShieldPopUpShow -= ShieldsPopUpUI_OnShieldPopUpShow;
        ShieldsPopUpUI.OnShieldPopUpComplete -= ShieldsPopUpUI_OnShieldPopUpComplete;
        ShieldsPopUpUI.OnShieldPopUpHide -= ShieldsPopUpUI_OnShieldPopUpHide;
    }

    private void Start()
    {
        HideUI();
    }

    private void ShowShieldUI() => backgroundImage.enabled = true;
    private void HideShieldUI() => backgroundImage.enabled = false;

    private void HideUI()
    {
        HideShieldUI();
        HideAllPieces();
    }

    private void CheckShouldShow(ShieldPieceSO shieldPieceSO)
    {
        if (shieldPieceSO.dialect != dialect) return;

        ShowShieldUI();
        CheckShowPiecesInmediately(shieldPieceSO);
    }

    private void CheckShowPiecesInmediately(ShieldPieceSO shieldPieceSO)
    {
        foreach (ShieldPopUpShieldPieceUIHandler piece in pieces)
        {
            if (piece.OnInventory && piece.ShieldPieceSO != shieldPieceSO) piece.ShowPieceUIInmediately();
            else piece.HidePieceUIInmediately();
        }
    }

    private void CheckShowPiece(ShieldPieceSO shieldPieceSO)
    {
        if (dialect != shieldPieceSO.dialect) return;

        foreach (ShieldPopUpShieldPieceUIHandler piece in pieces)
        {
            if (piece.ShieldPieceSO == shieldPieceSO) piece.ShowPieceUI();
        }
    }

    private void HideAllPieces()
    {
        foreach (ShieldPopUpShieldPieceUIHandler piece in pieces)
        {
            piece.HidePieceUIInmediately();
        }
    }

    #region ShieldsPopUpUI Subscriptions
    private void ShieldsPopUpUI_OnShieldPopUpShow(object sender, ShieldsPopUpUI.OnShieldPopUpEventArgs e)
    {
        CheckShouldShow(e.shieldPieceSO);
    }

    private void ShieldsPopUpUI_OnShieldPopUpComplete(object sender, ShieldsPopUpUI.OnShieldPopUpEventArgs e)
    {
        CheckShowPiece(e.shieldPieceSO);
    }

    private void ShieldsPopUpUI_OnShieldPopUpHide(object sender, ShieldsPopUpUI.OnShieldPopUpEventArgs e)
    {
        HideUI();
    }
    #endregion
}
