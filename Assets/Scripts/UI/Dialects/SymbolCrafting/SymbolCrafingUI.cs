using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class SymbolCrafingUI : BaseUI
{
    [Header("Components")]
    [SerializeField] private SymbolCrafting symbolCrafting;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Image imageToTranslate;
    [SerializeField] private Button openDictionaryButton;
    [SerializeField] private Button closeButton;

    [Header("Settings")]
    [SerializeField] private List<SymbolCraftingSO> symbolCraftingSOs;

    public static event EventHandler OnAnySymbolCraftingUIOpen;
    public static event EventHandler OnAnySymbolCraftingUIClose;

    public IRequiresSymbolCrafting iRequiresSymbolCrafting;

    public static event EventHandler<OnSymbolCraftingOpenDictionaryEventArgs> OnSymbolCraftingUIOpenDictionary;
    public event EventHandler OnSymbolDrawnCorrectely;

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

        symbolCrafting.ResetSymbolCraftingUIRefference();
    }

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void Start()
    {
        SetUIState(State.Open);
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
        closeButton.onClick.AddListener(CloseFromUI);
        openDictionaryButton.onClick.AddListener(OpenDictionary);
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

    public void SetUI(SymbolCrafting symbolCrafting)
    {
        this.symbolCrafting = symbolCrafting;
        symbolCraftingSOs = symbolCrafting.SymbolCraftingSOs;

        SetTitleText(symbolCrafting.Dialect);
        //SetImageToTranslate(symbolCraftingSO.imageToTranslateSprite);
    }

    private void SetTitleText(Dialect dialect) => titleText.text = $"Translate to dialect {dialect}";
    private void SetImageToTranslate(Sprite sprite) => imageToTranslate.sprite = sprite;

    private void OpenDictionary()
    {
        OnSymbolCraftingUIOpenDictionary?.Invoke(this, new OnSymbolCraftingOpenDictionaryEventArgs { dialect = symbolCrafting.Dialect });
    }
}
