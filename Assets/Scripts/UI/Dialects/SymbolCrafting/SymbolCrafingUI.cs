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
    [SerializeField] private Transform symbolCraftingSingleUIContainer;
    [SerializeField] private Transform symbolCraftingSingleUIPrefab;
    [Space]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Button openDictionaryButton;
    [SerializeField] private Button closeButton;

    private List<SymbolCraftingSO> symbolCraftingSOs;
    private List<SymbolCraftingSingleUI> symbolCraftingSingleUIs = new List<SymbolCraftingSingleUI>();

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
