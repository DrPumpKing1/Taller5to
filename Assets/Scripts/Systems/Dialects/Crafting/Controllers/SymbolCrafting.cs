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
    [SerializeField] private bool allSymbolsCrafted;

    public event EventHandler OnOpenSymbolCraftingUI;
    public event EventHandler OnSymbolsCrafted;

    public Dialect Dialect { get { return dialect; } }
    public List<SymbolCraftingSO> SymbolCraftingSOs { get { return symbolCraftingSOs; } }
    public bool AllSymbolsCrafted { get { return allSymbolsCrafted; } }

    private IRequiresSymbolCrafting iRequiresSymbolCrafting;

    private void OnEnable()
    {
        iRequiresSymbolCrafting.OnOpenSymbolCraftingUI += IRequiresSymbolCrafting_OnOpenSymbolCraftingUI;
        SymbolCrafingUIHandler.OnAllSymbolsCrafted += SymbolCrafingUIHandler_OnAllSymbolsCrafted;
    }

    private void OnDisable()
    {
        iRequiresSymbolCrafting.OnOpenSymbolCraftingUI -= IRequiresSymbolCrafting_OnOpenSymbolCraftingUI;
        SymbolCrafingUIHandler.OnAllSymbolsCrafted -= SymbolCrafingUIHandler_OnAllSymbolsCrafted;
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

    public void CraftSymbol() => allSymbolsCrafted = true;

    #region IRequiresSymbolCrafting Subscriptions
    private void IRequiresSymbolCrafting_OnOpenSymbolCraftingUI(object sender, EventArgs e)
    {
        OnOpenSymbolCraftingUI?.Invoke(this, EventArgs.Empty);
    }
    #endregion

    #region SymbolCraftingUIHandler Subscriptions
    private void SymbolCrafingUIHandler_OnAllSymbolsCrafted(object sender, SymbolCrafingUIHandler.OnAllSymbolsCraftedEventArgs e)
    {
        if (e.symbolCrafting != this) return;

        allSymbolsCrafted = true;
        OnSymbolsCrafted?.Invoke(this, EventArgs.Empty);
    }
    #endregion
}
