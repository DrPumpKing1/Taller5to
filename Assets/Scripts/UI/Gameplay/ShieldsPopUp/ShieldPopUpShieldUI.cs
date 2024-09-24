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
        ShieldsPopUpUI.OnShieldPopUpHide += ShieldsPopUpUI_OnShieldPopUpHide;
    }
    private void OnDisable()
    {
        ShieldsPopUpUI.OnShieldPopUpShow -= ShieldsPopUpUI_OnShieldPopUpShow;
        ShieldsPopUpUI.OnShieldPopUpHide -= ShieldsPopUpUI_OnShieldPopUpHide;
    }

    private void ShowShieldUI() => backgroundImage.enabled = true;
    private void HideShieldUI() => backgroundImage.enabled = false;

    private void CheckShouldShow(ShieldPieceSO shieldPieceSO)
    {
        if (dialect != shieldPieceSO.dialect) return;
        ShowShieldUI();
    }

    #region ShieldsPopUpUI Subscriptions
    private void ShieldsPopUpUI_OnShieldPopUpShow(object sender, ShieldsPopUpUI.OnShieldPopUpEventArgs e)
    {
        CheckShouldShow(e.shieldPieceSO);
    }

    private void ShieldsPopUpUI_OnShieldPopUpHide(object sender, ShieldsPopUpUI.OnShieldPopUpEventArgs e)
    {
        HideShieldUI();
    }
    #endregion
}
