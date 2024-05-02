using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolCrafting : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Component iRequiresSymbolCraftingComponent;

    [Header("Symbol Crafting Settings")]
    [SerializeField] private Dialect dialect;
    [SerializeField] private List<SymbolCraftingSO> symbolCraftingSOs;

    [Header("SymbolCraftingSettings")]
    [SerializeField] private Transform symbolCraftingUIPrefab;
    [SerializeField] private bool symbolCrafted;

    public event EventHandler OnSymbolCrafted;

    public Dialect Dialect { get { return dialect; } }
    public List<SymbolCraftingSO> SymbolCraftingSOs { get { return symbolCraftingSOs; } }
    public bool SymbolCrafted { get { return symbolCrafted; } }

    private IRequiresSymbolCrafting iRequiresSymbolCrafting;
    private SymbolCrafingUI currentSymbolCraftingUI;

    private void OnEnable()
    {
        iRequiresSymbolCrafting.OnOpenSymbolCraftingUI += IRequiresSymbolCrafting_OnOpenSymbolCraftingUI;
    }
    private void OnDisable()
    {
        iRequiresSymbolCrafting.OnOpenSymbolCraftingUI -= IRequiresSymbolCrafting_OnOpenSymbolCraftingUI;
    }

    private void Awake()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {      
        iRequiresSymbolCrafting = iRequiresSymbolCraftingComponent.GetComponent<IRequiresSymbolCrafting>();
        if (iRequiresSymbolCrafting == null) Debug.LogError("The IRequiresSymbolCraftingComponent component does not implement IRequiresSymbolCrafting");      
    }

    public void OpenSymbolCraftingUI()
    {
        GameObject symbolCraftingUIGameObject = Instantiate(symbolCraftingUIPrefab.gameObject);

        SymbolCrafingUI symbolCraftingUI = symbolCraftingUIGameObject.GetComponentInChildren<SymbolCrafingUI>();

        if (!symbolCraftingUI)
        {
            Debug.LogWarning("There's not a SymbolCrafingUI attached to instantiated prefab");
            return;
        }

        currentSymbolCraftingUI = symbolCraftingUI;
        currentSymbolCraftingUI.SetUI(this);
        currentSymbolCraftingUI.OnSymbolDrawnCorrectely += SymbolCraftingUI_OnSymbolDrawnCorrectely;
    }

    public void ResetSymbolCraftingUIRefference()
    {
        currentSymbolCraftingUI.OnSymbolDrawnCorrectely -= SymbolCraftingUI_OnSymbolDrawnCorrectely;
        currentSymbolCraftingUI = null;
    }

    public void CraftSymbol() => symbolCrafted = true;

    #region IRequiresSymbolCrafting Subscriptions
    private void IRequiresSymbolCrafting_OnOpenSymbolCraftingUI(object sender, EventArgs e)
    {
        if (UIManager.Instance.UIActive) return;

        OpenSymbolCraftingUI();
    }
    #endregion

    #region SymbolCraftingUI Subscriptions

    private void SymbolCraftingUI_OnSymbolDrawnCorrectely(object sender, EventArgs e)
    {
        symbolCrafted = true;
        OnSymbolCrafted?.Invoke(this, EventArgs.Empty);
    }
    #endregion
}
