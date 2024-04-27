using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUI : MonoBehaviour
{
    protected bool shouldClose = false;
    protected virtual void OnEnable()
    {
        AddToUILayersList();
        UIManager.OnUIToCloseInput += UIManager_OnUIToCloseInput;
    }

    protected virtual void OnDisable()
    {
        UIManager.OnUIToCloseInput -= UIManager_OnUIToCloseInput;
        RemoveFromUILayersList();
    }

    protected void AddToUILayersList() => UIManager.Instance.AddToLayersList(this);
    protected void RemoveFromUILayersList() => UIManager.Instance.RemoveFromLayersList(this);

    protected abstract void CloseUI();

    #region UIManager Subscriptions
    private void UIManager_OnUIToCloseInput(object sender, UIManager.OnUIToCloseInputEventArgs e)
    {
        if (e.UIToClose != this) return;
        if (shouldClose) return;

        shouldClose = true;

        CloseUI();
    }
    #endregion

}
