using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldPopUpShieldBackgroundUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image backgroundImage;

    [Header("Settings")]
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

    private void ShowUI()
    {
        backgroundImage.enabled = true;
    }

    private void HideUI()
    {
        backgroundImage.enabled = false;
    }

    private void CheckShouldShow(Dialect dialect)
    {
        if (this.dialect != dialect) return;
        ShowUI();
    }

    #region ShieldsPopUpUI Subscriptions
    private void ShieldsPopUpUI_OnShieldPopUpShow(object sender, ShieldsPopUpUI.OnShieldPopUpEventArgs e)
    {
        CheckShouldShow(e.shieldPieceSO.dialect);
    }

    private void ShieldsPopUpUI_OnShieldPopUpHide(object sender, ShieldsPopUpUI.OnShieldPopUpEventArgs e)
    {
        HideUI();
    }
    #endregion
}
