using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class SymbolCraftingUI : BaseUI
{
    [Header("Components")]
    [SerializeField] private SymbolCrafingUIHandler symbolCrafingUIHandler;

    [Header("UI Components")]
    [SerializeField] private Button closeButton;

    public static event EventHandler OnAnySymbolCraftingUIOpen;
    public static event EventHandler OnAnySymbolCraftingUIClose;

    protected override void OnEnable()
    {
        base.OnEnable();
        AddToUILayersList();
        OnAnySymbolCraftingUIOpen?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        RemoveFromUILayersList();
        OnAnySymbolCraftingUIClose?.Invoke(this, EventArgs.Empty);
    }

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void Start()
    {
        SetUIState(State.Open);
    }

    private void InitializeButtonsListeners()
    {
        closeButton.onClick.AddListener(CloseFromUI);
    }

    private void CloseUI()
    {
        if (state != State.Open) return;

        SetUIState(State.Closed);
        Destroy(transform.parent.gameObject);
    }

    protected override void CloseFromUI()
    {
        CloseUI();
    }
}
