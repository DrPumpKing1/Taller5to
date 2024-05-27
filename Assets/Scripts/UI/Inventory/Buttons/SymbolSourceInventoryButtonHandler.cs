using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class SymbolSourceInventoryButtonHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SymbolSourceSO symbolSourceSO;

    [Header("UI Components")]
    [SerializeField] private Button symbolSourceButton;

    public static event EventHandler<OnSymbolSourceButtonUIClickedEventArgs> OnSymbolSourceInventoryButtonClicked;

    public class OnSymbolSourceButtonUIClickedEventArgs : EventArgs
    {
        public SymbolSourceSO symbolSourceSO;
    }

    private void OnEnable()
    {
        SymbolSourcesManager.OnSymbolSourceCollected += SymbolSourcesManager_OnSymbolSourceCollected;
    }

    private void OnDisable()
    {
        SymbolSourcesManager.OnSymbolSourceCollected -= SymbolSourcesManager_OnSymbolSourceCollected;
    }

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void Start()
    {
        CheckButtonState();
    }

    private void InitializeButtonsListeners()
    {
        symbolSourceButton.onClick.AddListener(ClickButton);
    }

    private void ClickButton()
    {
        OnSymbolSourceInventoryButtonClicked?.Invoke(this, new OnSymbolSourceButtonUIClickedEventArgs { symbolSourceSO = symbolSourceSO });
    }

    private void CheckButtonState()
    {
        if (SymbolSourcesManager.Instance.SymbolSourcesCollected.Contains(symbolSourceSO))
        {
            ShowButton();
        }
        else
        {
            HideButton();
        }
    }

    private void ShowButton() => symbolSourceButton.gameObject.SetActive(true);
    private void HideButton() => symbolSourceButton.gameObject.SetActive(false);

    #region SymbolSourcesManager Subscriptions
    private void SymbolSourcesManager_OnSymbolSourceCollected(object sender, SymbolSourcesManager.OnSymbolSourceCollectedEventArgs e)
    {
        CheckButtonState();
    }
    #endregion
}
