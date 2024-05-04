using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SymbolCraftingUIDialectSetUpHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SymbolCrafting symbolCrafting;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Button openDictionaryButton;

    public static event EventHandler<OnSymbolCraftingOpenDictionaryEventArgs> OnSymbolCraftingUIOpenDictionary;

    public class OnSymbolCraftingOpenDictionaryEventArgs : EventArgs
    {
        public Dialect dialect;
    }

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void Start()
    {
        SetTitleText(symbolCrafting.Dialect);
    }

    private void InitializeButtonsListeners()
    {
        openDictionaryButton.onClick.AddListener(OpenDictionary);
    }

    private void OpenDictionary()
    {
        OnSymbolCraftingUIOpenDictionary?.Invoke(this, new OnSymbolCraftingOpenDictionaryEventArgs { dialect = symbolCrafting.Dialect });
    }

    private void SetTitleText(Dialect dialect) => titleText.text = $"Translate to dialect {dialect}";
}
