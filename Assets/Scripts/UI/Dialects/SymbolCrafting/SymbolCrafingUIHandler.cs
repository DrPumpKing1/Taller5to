using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class SymbolCrafingUIHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SymbolCrafting symbolCrafting;
    [SerializeField] private SymbolCraftingUI symbolCraftingUI;

    [Header("UI Components")]
    [SerializeField] private Transform symbolCraftingSingleUIContainer;
    [SerializeField] private Transform symbolCraftingSingleUIPrefab;
    [Space]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Button openDictionaryButton;

    private List<SymbolCraftingSO> symbolCraftingSOs;
    private List<SymbolCraftingSingleUI> symbolCraftingSingleUIs = new List<SymbolCraftingSingleUI>();

    public static event EventHandler<OnSymbolCraftingOpenDictionaryEventArgs> OnSymbolCraftingUIOpenDictionary;
    public event EventHandler OnSymbolDrawnCorrectely;

    public class OnSymbolCraftingOpenDictionaryEventArgs : EventArgs
    {
        public Dialect dialect;
    }

    private void OnDisable()
    {
        symbolCrafting.ResetSymbolCraftingUIRefference();
    }

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) //TestToCraftSymbol
        {
            OnSymbolDrawnCorrectely?.Invoke(this, EventArgs.Empty);
        }
    }

    private void InitializeButtonsListeners()
    {
        openDictionaryButton.onClick.AddListener(OpenDictionary);
    }

    public void SetUI(SymbolCrafting symbolCrafting)
    {
        this.symbolCrafting = symbolCrafting;
        symbolCraftingSOs = symbolCrafting.SymbolCraftingSOs;

        SetTitleText(symbolCrafting.Dialect);
        SetSingleUIs(symbolCraftingSOs);
    }

    private void SetTitleText(Dialect dialect) => titleText.text = $"Translate to dialect {dialect}";

    private void SetSingleUIs(List<SymbolCraftingSO> symbolCraftingSOs)
    {
        foreach(SymbolCraftingSO symbolCraftingSO in symbolCraftingSOs)
        {
            Transform symbolCraftingSingleUITransform = Instantiate(symbolCraftingSingleUIPrefab, symbolCraftingSingleUIContainer);
            SymbolCraftingSingleUI symbolCratingSingleUI = symbolCraftingSingleUITransform.GetComponentInChildren<SymbolCraftingSingleUI>();

            if (!symbolCratingSingleUI)
            {
                Debug.LogWarning("The instantiated prefab does not have a SymbolCraftingSingleUI component");
            }

            symbolCraftingSingleUIs.Add(symbolCratingSingleUI);
            symbolCratingSingleUI.SetSymbolCraftingSingleUI(symbolCraftingSO);
        }
    }

    private bool CheckAllSymbolsCrafted()
    {
        foreach(SymbolCraftingSingleUI symbolCraftingSingleUI in symbolCraftingSingleUIs)
        {
            if (!symbolCraftingSingleUI.IsCrafted) return false;
        }

        return true;
    }

    private void OpenDictionary()
    {
        OnSymbolCraftingUIOpenDictionary?.Invoke(this, new OnSymbolCraftingOpenDictionaryEventArgs { dialect = symbolCrafting.Dialect });
    }
}
