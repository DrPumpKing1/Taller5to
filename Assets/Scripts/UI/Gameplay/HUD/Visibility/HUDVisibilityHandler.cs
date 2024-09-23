using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDVisibilityHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string logToShow;
    [SerializeField] private bool isVisible;

    public bool IsVisible => isVisible;

    private CanvasGroup canvasGroup;

    public event EventHandler OnShowHUD;
    public event EventHandler OnHideHUD;
    public event EventHandler OnShowHUDFirstTime;

    private void OnEnable()
    {
        GameLogManager.OnLogAdd += GameLogManager_OnLogAdd;
    }
    private void OnDisable()
    {
        GameLogManager.OnLogAdd -= GameLogManager_OnLogAdd;
    }

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        CheckIsVisible();
    }

    public void SetIsVisible(bool visible)
    {
        isVisible = visible;
        canvasGroup.blocksRaycasts = visible;
    }

    private void CheckIsVisible()
    {
        if (isVisible)
        {
            OnShowHUD?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnHideHUD?.Invoke(this, EventArgs.Empty);
        }
    }

    #region GameLogManager Subscriptions
    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        if (e.gameplayAction.log != logToShow) return;
        OnShowHUDFirstTime?.Invoke(this, EventArgs.Empty);
        SetIsVisible(true);
    }
    #endregion
}
