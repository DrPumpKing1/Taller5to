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
    [SerializeField] private bool symbolsCrafted;

    public event EventHandler OnSymbolsCrafted;

    public Dialect Dialect { get { return dialect; } }
    public List<SymbolCraftingSO> SymbolCraftingSOs { get { return symbolCraftingSOs; } }
    public bool SymbolsCrafted { get { return symbolsCrafted; } }

    private IRequiresSymbolCrafting iRequiresSymbolCrafting;
    private SymbolCrafingUIHandler currentSymbolCraftingUIHandler;

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

        SymbolCrafingUIHandler symbolCraftingUI = symbolCraftingUIGameObject.GetComponentInChildren<SymbolCrafingUIHandler>();

        if (!symbolCraftingUI)
        {
            Debug.LogWarning("There's not a SymbolCrafingUI attached to instantiated prefab");
            return;
        }

        currentSymbolCraftingUIHandler = symbolCraftingUI;
        currentSymbolCraftingUIHandler.SetUI(this);
        currentSymbolCraftingUIHandler.OnSymbolDrawnCorrectely += SymbolCraftingUI_OnSymbolDrawnCorrectely;
    }

    public void ResetSymbolCraftingUIRefference()
    {
        currentSymbolCraftingUIHandler.OnSymbolDrawnCorrectely -= SymbolCraftingUI_OnSymbolDrawnCorrectely;
        currentSymbolCraftingUIHandler = null;
    }

    public void CraftSymbol() => symbolsCrafted = true;

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
        symbolsCrafted = true;
        OnSymbolsCrafted?.Invoke(this, EventArgs.Empty);
    }
    #endregion
}
