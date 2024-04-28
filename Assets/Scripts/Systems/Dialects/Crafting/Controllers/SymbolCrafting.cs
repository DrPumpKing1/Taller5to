using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolCrafting : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Component iRequiresSymbolCraftingComponent;

    [Header("Symbol Crafting Settings")]
    [SerializeField] private SymbolCraftingSO symbolCraftingSO;

    [Header("SymbolCraftingSettings")]
    [SerializeField] private Transform symbolCraftingUIPrefab;
    [SerializeField] private bool symbolCrafted;

    public SymbolCraftingSO SymbolCraftingSO { get { return symbolCraftingSO; } }
    public bool SymbolCrafted { get { return symbolCrafted; } }

    private IRequiresSymbolCrafting iRequiresSymbolCrafting;

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

        symbolCraftingUI.SetUI(SymbolCraftingSO);
    }

    public void CraftSymbol() => symbolCrafted = true;

    #region IRequiresSymbolCrafting Subscriptions
    private void IRequiresSymbolCrafting_OnOpenSymbolCraftingUI(object sender, EventArgs e)
    {
        if (UIManager.Instance.UIActive) return;

        OpenSymbolCraftingUI();
    }
    #endregion
}
