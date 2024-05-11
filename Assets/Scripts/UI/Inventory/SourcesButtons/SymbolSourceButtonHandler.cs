using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class SymbolSourceButtonHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private DialectSymbolSourceSO dialectSymbolSourceSO;

    [Header("UI Components")]
    [SerializeField] private Button symbolSourceButton;

    public static event EventHandler<OnSymbolSourceButtonUIClickedEventArgs> OnSymbolSourceInventoryButtonClicked;

    public class OnSymbolSourceButtonUIClickedEventArgs : EventArgs
    {
        public DialectSymbolSourceSO dialectSymbolSourceSO;
    }

    private void OnEnable()
    {
        SymbolSourcesManager.OnDialectSymbolSourceCollected += SymbolSourcesManager_OnDialectSymbolSourceCollected;
    }

    private void OnDisable()
    {
        SymbolSourcesManager.OnDialectSymbolSourceCollected -= SymbolSourcesManager_OnDialectSymbolSourceCollected;
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
        OnSymbolSourceInventoryButtonClicked?.Invoke(this, new OnSymbolSourceButtonUIClickedEventArgs { dialectSymbolSourceSO = dialectSymbolSourceSO });
    }

    private void CheckButtonState()
    {
        if (SymbolSourcesManager.Instance.SymbolSourcesCollected.Contains(dialectSymbolSourceSO))
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
    private void SymbolSourcesManager_OnDialectSymbolSourceCollected(object sender, SymbolSourcesManager.OnDialectSymbolSourceCollectedEventArgs e)
    {
        CheckButtonState();
    }
    #endregion
}
