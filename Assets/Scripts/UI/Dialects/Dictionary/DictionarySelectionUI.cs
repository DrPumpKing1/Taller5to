using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DictionarySelectionUI : BaseUI
{
    [Header("UI Components")]
    [SerializeField] private Button closeButton;

    [Header("Dictionary Button Panels")]
    [SerializeField] private List<DictionaryButtonPanel> dictionaryButtonPanels;

    private CanvasGroup canvasGroup;

    public static event EventHandler OnCloseFromUI;
    public static event EventHandler OnDictionarySelectionUIOpen;
    public static event EventHandler OnDictionarySelectionUIClose;

    [Serializable]
    private class DictionaryButtonPanel
    {
        public Dialect dialect;
        public Button dictionaryButton;
        public SingleDictionaryUI singleDictionaryUI;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        DictionarySelectionOpeningManager.OnDictionarySelectionOpen += DictionarySelectionOpeningManager_OnDictionarySelectionOpen;
        DictionarySelectionOpeningManager.OnDictionarySelectionClose += DictionarySelectionOpeningManager_OnDictionarySelectionClose;
        SymbolCrafingUI.OnSymbolCraftingUIOpenDictionary += SymbolCrafingUI_OnSymbolCraftingUIOpenDIctionary;
    }


    protected override void OnDisable()
    {
        base.OnDisable();
        DictionarySelectionOpeningManager.OnDictionarySelectionOpen -= DictionarySelectionOpeningManager_OnDictionarySelectionOpen;
        DictionarySelectionOpeningManager.OnDictionarySelectionClose -= DictionarySelectionOpeningManager_OnDictionarySelectionClose;
        SymbolCrafingUI.OnSymbolCraftingUIOpenDictionary -= SymbolCrafingUI_OnSymbolCraftingUIOpenDIctionary;
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

        foreach(DictionaryButtonPanel dictionaryButtonPanel in dictionaryButtonPanels)
        {
            dictionaryButtonPanel.dictionaryButton.onClick.AddListener(() => OpenSingleDictionary(dictionaryButtonPanel.singleDictionaryUI));
        }
    }

    private void InitializeVariables()
    {
        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 0f);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void OpenSingleDictionary(SingleDictionaryUI singleDictionaryUI)
    {
        singleDictionaryUI.OpenUI();
    }

    private void OpenUI()
    {
        if (state != State.Closed) return;

        SetUIState(State.Open);

        AddToUILayersList();

        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 1f);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        OnDictionarySelectionUIOpen?.Invoke(this, EventArgs.Empty);
    }

    private void CloseUI()
    {
        if (state != State.Open) return;

        SetUIState(State.Closed);

        RemoveFromUILayersList();

        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 0f);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        OnDictionarySelectionUIClose?.Invoke(this, EventArgs.Empty);
    }

    protected override void CloseFromUI()
    {
        OnCloseFromUI?.Invoke(this, EventArgs.Empty);
    }

    private DictionaryButtonPanel GetDictionaryButtonPanelByDialect(Dialect dialect)
    {
        foreach (DictionaryButtonPanel dictionaryButtonPanel in dictionaryButtonPanels)
        {
            if(dictionaryButtonPanel.dialect == dialect)
            {
                return dictionaryButtonPanel;
            }
        }

        return null;
    }

    #region DictionarySelectionOpeningManager Subscriptions
    private void DictionarySelectionOpeningManager_OnDictionarySelectionOpen(object sender, EventArgs e)
    {
        OpenUI();
    }

    private void DictionarySelectionOpeningManager_OnDictionarySelectionClose(object sender, EventArgs e)
    {
        CloseUI();
    }
    #endregion

    #region SymbolCraftingUI Subscriptions
    private void SymbolCrafingUI_OnSymbolCraftingUIOpenDIctionary(object sender, SymbolCrafingUI.OnSymbolCraftingOpenDictionaryEventArgs e)
    {
        DictionaryButtonPanel dictionaryButtonPanel = GetDictionaryButtonPanelByDialect(e.dialect);

        if (dictionaryButtonPanel == null)
        {
            Debug.LogWarning($"The dialect {e.dialect} does not match any DictionaryButtonPanel on list");
            return;
        }

        OpenSingleDictionary(dictionaryButtonPanel.singleDictionaryUI);
    }
    #endregion
}
