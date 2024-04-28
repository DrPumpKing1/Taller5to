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
    [SerializeField] private Button openDictionaryButton;
    [SerializeField] private Button closeButton;

    [Header("Settings")]
    [SerializeField] private SymbolCraftingSO symbolCraftingSO;

    public static event EventHandler OnAnySymbolCraftingUIOpen;
    public static event EventHandler OnAnySymbolCraftingUIClose;

    public IRequiresSymbolCrafting iRequiresSymbolCrafting;

    public static event EventHandler<OnSymbolCraftingOpenDictionaryEventArgs> OnSymbolCraftingUIOpenDIctionary;

    public class OnSymbolCraftingOpenDictionaryEventArgs : EventArgs
    {
        public Dialect dialect;
    }

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
        closeButton.onClick.AddListener(CloseUI);
        openDictionaryButton.onClick.AddListener(OpenDictionary);
    }

    public void SetUI(SymbolCraftingSO symbolCraftingSO)
    {
        this.symbolCraftingSO = symbolCraftingSO;

        SetTitleText(symbolCraftingSO.symbolToCraft.dialect);
        SetImageToTranslate(symbolCraftingSO.imageToTranslateSprite);
    }

    private void SetTitleText(Dialect dialect) => titleText.text = $"Translate to dialect {dialect}";
    private void SetImageToTranslate(Sprite sprite) => imageToTranslate.sprite = sprite;

    private void OpenDictionary()
    {
        OnSymbolCraftingUIOpenDIctionary?.Invoke(this, new OnSymbolCraftingOpenDictionaryEventArgs { dialect = symbolCraftingSO.symbolToCraft.dialect });
    }

    protected override void CloseUI()
    {
        if (state != State.Open) return;

        SetUIState(State.Closed);
        Destroy(transform.parent.gameObject);
    }
}
