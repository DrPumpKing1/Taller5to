using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class SymbolCrafingUI : BaseUI
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Image imageToTranslate;
    [SerializeField] private Button closeButton;

    [Header("Settings")]
    [SerializeField] private SymbolCraftingSO symbolCraftingSO;

    public static event EventHandler OnAnySymbolCraftingUIOpen;
    public static event EventHandler OnAnySymbolCraftingUIClose;

    public IRequiresSymbolCrafting iRequiresSymbolCrafting;

    protected override void OnEnable()
    {
        base.OnEnable();
        OnAnySymbolCraftingUIOpen?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        OnAnySymbolCraftingUIClose?.Invoke(this, EventArgs.Empty);
    }

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        closeButton.onClick.AddListener(CloseUI);
    }

    public void SetUI(SymbolCraftingSO symbolCraftingSO)
    {
        this.symbolCraftingSO = symbolCraftingSO;

        SetTitleText(symbolCraftingSO.symbolToCraft.dialect);
        SetImageToTranslate(symbolCraftingSO.imageToTranslateSprite);
    }

    private void SetTitleText(Dialect dialect) => titleText.text = $"Translate to dialect {dialect}";
    private void SetImageToTranslate(Sprite sprite) => imageToTranslate.sprite = sprite;

    protected override void CloseUI()
    {
        Destroy(transform.parent.gameObject);
    }
}
