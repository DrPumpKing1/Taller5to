using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DictionarySelectionUI : BaseUI
{
    [Header("Components")]
    [SerializeField] private UIInput UIInput;

    [Header("UI Components")]
    [SerializeField] private Button closeButton;

    [Header("UI Components")]
    [SerializeField] private List<DictionaryButtonPanel> dictionaryButtonPanels;

    private bool DictionaryInput => UIInput.GetDictionaryDown();
    private CanvasGroup canvasGroup;

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
        SymbolCrafingUI.OnSymbolCraftingUIOpenDIctionary += SymbolCrafingUI_OnSymbolCraftingUIOpenDIctionary;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        SymbolCrafingUI.OnSymbolCraftingUIOpenDIctionary -= SymbolCrafingUI_OnSymbolCraftingUIOpenDIctionary;
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

    private void Update()
    {
        CheckOpenClose();
    }

    private void InitializeButtonsListeners()
    {
        closeButton.onClick.AddListener(CloseUI);

        foreach(DictionaryButtonPanel dictionaryButtonPanel in dictionaryButtonPanels)
        {
            dictionaryButtonPanel.dictionaryButton.onClick.AddListener(() => OpenDictionary(dictionaryButtonPanel.singleDictionaryUI));
        }
    }

    private void InitializeVariables()
    {
        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 0f);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void OpenDictionary(SingleDictionaryUI singleDictionaryUI)
    {
        singleDictionaryUI.OpenUI();
    }

    private void CheckOpenClose()
    {
        bool openThisFrame = CheckOpen();

        if (openThisFrame) return;

        CheckClose();
    }

    private bool CheckOpen()
    {
        if (UIManager.Instance.UIActive) return false;
        if (!DictionaryInput) return false;
        if (state != State.Closed) return false;

        OpenUI();

        return true;
    }

    private bool CheckClose()
    {
        if (!UIManager.Instance.IsFirstOnList(this)) return false;
        if (!DictionaryInput) return false;
        if (state != State.Open) return false;

        CloseUI();

        return true;
    }

    private void OpenUI()
    {
        if (state != State.Closed) return;

        SetUIState(State.Open);

        AddToUILayersList();

        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 1f);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    protected override void CloseUI()
    {
        if (state != State.Open) return;

        SetUIState(State.Closed);

        RemoveFromUILayersList();

        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 0f);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
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

    #region SymbolCraftingUI Subscriptions
    private void SymbolCrafingUI_OnSymbolCraftingUIOpenDIctionary(object sender, SymbolCrafingUI.OnSymbolCraftingOpenDictionaryEventArgs e)
    {
        DictionaryButtonPanel dictionaryButtonPanel = GetDictionaryButtonPanelByDialect(e.dialect);

        if (dictionaryButtonPanel == null)
        {
            Debug.LogWarning($"The dialect {e.dialect} does not match any DictionaryButtonPanel on list");
            return;
        }

        OpenDictionary(dictionaryButtonPanel.singleDictionaryUI);
    }
    #endregion
}
