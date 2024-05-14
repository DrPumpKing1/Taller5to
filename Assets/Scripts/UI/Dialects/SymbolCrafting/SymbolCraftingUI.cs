using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class SymbolCraftingUI : BaseUI
{
    [Header("Components")]
    [SerializeField] private SymbolCrafting symbolCrafting;
    [SerializeField] private SymbolCrafingUIHandler symbolCraftingUIHandler;

    [Header("UI Components")]
    [SerializeField] private Button closeButton;

    public static event EventHandler OnAnySymbolCraftingUIOpen;
    public static event EventHandler OnAnySymbolCraftingUIClose;

    private CanvasGroup canvasGroup;

    protected override void OnEnable()
    {
        base.OnEnable();
        symbolCrafting.OnOpenSymbolCraftingUI += SymbolCrafting_OnOpenSymbolCraftingUI;
        symbolCrafting.OnAllSymbolsCrafted += SymbolCrafting_OnAllSymbolsCrafted;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        symbolCrafting.OnOpenSymbolCraftingUI -= SymbolCrafting_OnOpenSymbolCraftingUI;
        symbolCrafting.OnAllSymbolsCrafted -= SymbolCrafting_OnAllSymbolsCrafted;
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        InitializeButtonsListeners();
    }

    private void Start()
    {
        InitializeVariables();
        SetUIState(State.Closed);
    }

    private void InitializeButtonsListeners()
    {
        closeButton.onClick.AddListener(CloseFromUI);
    }

    private void InitializeVariables()
    {
        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 0f);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void OpenUI()
    {
        if (state != State.Closed) return;

        SetUIState(State.Open);

        AddToUILayersList();

        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 1f);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        OnAnySymbolCraftingUIOpen?.Invoke(this, EventArgs.Empty);

    }

    private void CloseUI()
    {
        if (state != State.Open) return;

        SetUIState(State.Closed);

        RemoveFromUILayersList();

        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 0f);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        OnAnySymbolCraftingUIClose?.Invoke(this, EventArgs.Empty);
    }

    protected override void CloseFromUI()
    {
        CloseUI();
    }

    #region SymbolCraftingSubscriptions
    private void SymbolCrafting_OnOpenSymbolCraftingUI(object sender, EventArgs e)
    {
        OpenUI();
    }
    #endregion

    #region SymbolCraftingUIHandler Subscriptions
    private void SymbolCrafting_OnAllSymbolsCrafted(object sender, EventArgs e)
    {
        CloseUI();
    }

    #endregion
}
