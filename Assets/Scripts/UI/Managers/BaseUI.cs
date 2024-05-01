using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUI : MonoBehaviour
{
    protected enum State {Closed, Opening, Open, Closing}

    protected State state;

    protected virtual void OnEnable()
    {
        UIManager.OnUIToCloseInput += UIManager_OnUIToCloseInput;
    }

    protected virtual void OnDisable()
    {
        UIManager.OnUIToCloseInput -= UIManager_OnUIToCloseInput;
    }

    protected void AddToUILayersList() => UIManager.Instance.AddToLayersList(this);
    protected void RemoveFromUILayersList() => UIManager.Instance.RemoveFromLayersList(this);

    protected void SetUIState(State state) => this.state = state;

    protected abstract void CloseFromUI();

    #region UIManager Subscriptions
    private void UIManager_OnUIToCloseInput(object sender, UIManager.OnUIToCloseInputEventArgs e)
    {
        if (e.UIToClose != this) return;
        if (state != State.Open) return;

        CloseFromUI();
    }
    #endregion

}
