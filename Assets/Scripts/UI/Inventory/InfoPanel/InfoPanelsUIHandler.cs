using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class InfoPanelsUIHandler : MonoBehaviour
{
    public event EventHandler OnHideAllPanels;

    public event EventHandler<OnShowProjectableObjectInfoPanelUIEventArgs> OnShowProjectableObjectInfoPanelUI;
    public event EventHandler<OnShowProjectionGemsInfoPanelUIEventArgs> OnShowProjectionGemsInfoPanelUI;
    public event EventHandler<OnShowSymbolSourceInfoPanelUIEventArgs> OnShowSymbolSourceInfoPanelUI;

    public class OnShowProjectableObjectInfoPanelUIEventArgs : EventArgs
    {
        public ProjectableObjectSO projectableObjectSO;
    }

    public class OnShowProjectionGemsInfoPanelUIEventArgs : EventArgs
    {
        public int totalProjectionGems;
        public int availableProjectionGems;
    }

    public class OnShowSymbolSourceInfoPanelUIEventArgs : EventArgs
    {
        public SymbolSourceSO symbolSourceSO;
    }

    private void OnEnable()
    {
        ProjectableObjectInventoryButtonHandler.OnProjectableObjectInventoryButtonClicked += ProjectableObjectInventoryButtonHandler_OnProjectableObjectInventoryButtonClicked;
        ProjectionGemsInventoryButtonHandler.OnProjectionGemsButtonClicked += ProjectionGemsInventoryButtonHandler_OnProjectionGemsButtonClicked;
        SymbolSourceInventoryButtonHandler.OnSymbolSourceInventoryButtonClicked += SymbolSourceInventoryButtonHandler_OnSymbolSourceInventoryButtonClicked;
    }

    private void OnDisable()
    {
        ProjectableObjectInventoryButtonHandler.OnProjectableObjectInventoryButtonClicked -= ProjectableObjectInventoryButtonHandler_OnProjectableObjectInventoryButtonClicked;
        ProjectionGemsInventoryButtonHandler.OnProjectionGemsButtonClicked -= ProjectionGemsInventoryButtonHandler_OnProjectionGemsButtonClicked;
        SymbolSourceInventoryButtonHandler.OnSymbolSourceInventoryButtonClicked -= SymbolSourceInventoryButtonHandler_OnSymbolSourceInventoryButtonClicked;
    }

    private void Start()
    {
        HideAllPanels();
    }

    private void HideAllPanels()
    {
        OnHideAllPanels?.Invoke(this, EventArgs.Empty);
    }

    #region Subscriptions
    private void ProjectableObjectInventoryButtonHandler_OnProjectableObjectInventoryButtonClicked(object sender, ProjectableObjectInventoryButtonHandler.OnProjectableObjectButtonUIClickedEventArgs e)
    {
        HideAllPanels();
        OnShowProjectableObjectInfoPanelUI?.Invoke(this, new OnShowProjectableObjectInfoPanelUIEventArgs { projectableObjectSO = e.projectableObjectSO });
    }

    private void ProjectionGemsInventoryButtonHandler_OnProjectionGemsButtonClicked(object sender, ProjectionGemsInventoryButtonHandler.OnProjectionGemsButtonClickedEventArgs e)
    {
        HideAllPanels();
        OnShowProjectionGemsInfoPanelUI?.Invoke(this, new OnShowProjectionGemsInfoPanelUIEventArgs { totalProjectionGems = e.totalProjectionGems, availableProjectionGems = e.availableProjectionGems });
    }

    private void SymbolSourceInventoryButtonHandler_OnSymbolSourceInventoryButtonClicked(object sender, SymbolSourceInventoryButtonHandler.OnSymbolSourceButtonUIClickedEventArgs e)
    {
        HideAllPanels();
        OnShowSymbolSourceInfoPanelUI.Invoke(this, new OnShowSymbolSourceInfoPanelUIEventArgs { symbolSourceSO = e.symbolSourceSO });
    }
    #endregion
}
