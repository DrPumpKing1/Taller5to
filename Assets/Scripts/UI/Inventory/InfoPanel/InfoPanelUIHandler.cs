using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class InfoPanelUIHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected InfoPanelsUIHandler infoPanelsUIHandler;

    private CanvasGroup canvasGroup;

    public static event EventHandler OnAnyInventoryInfoPanelShown;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    protected virtual void OnEnable()
    {
        infoPanelsUIHandler.OnHideAllPanels += InfoPanelsUIHandler_OnHideAllPanels;
    }

    protected virtual void OnDisable()
    {
        infoPanelsUIHandler.OnHideAllPanels -= InfoPanelsUIHandler_OnHideAllPanels;
    }

    protected void ShowPanel()
    {
        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 1f);

        OnAnyInventoryInfoPanelShown?.Invoke(this, EventArgs.Empty);
    }
    protected void HidePanel() 
    {
        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 0f);
    }

    private void InfoPanelsUIHandler_OnHideAllPanels(object sender, System.EventArgs e)
    {
        HidePanel();
    }
}
