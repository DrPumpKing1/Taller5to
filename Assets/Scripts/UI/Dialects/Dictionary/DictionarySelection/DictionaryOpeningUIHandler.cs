using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class DictionaryOpeningUIHandler : MonoBehaviour
{
    [Header("Dictionary Button Panels")]
    [SerializeField] private List<DictionaryButtonPanel> dictionaryButtonPanels;

    public static event EventHandler<OnOpenSingleDictionaryUIEventArgs> OnOpenSingleDictionaryUI;

    public class OnOpenSingleDictionaryUIEventArgs
    {
        public SingleDictionaryUI singleDictionaryUI;
    }

    [Serializable]
    private class DictionaryButtonPanel
    {
        public Dialect dialect;
        public Button dictionaryButton;
        public SingleDictionaryUI singleDictionaryUI;
    }

    private void OnEnable()
    {
        SymbolCraftingUIDialectSetUpHandler.OnSymbolCraftingUIOpenDictionary += SymbolCrafingUI_OnSymbolCraftingUIOpenDIctionary;
    }

    private void OnDisable()
    {
        SymbolCraftingUIDialectSetUpHandler.OnSymbolCraftingUIOpenDictionary -= SymbolCrafingUI_OnSymbolCraftingUIOpenDIctionary;
    }

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        foreach (DictionaryButtonPanel dictionaryButtonPanel in dictionaryButtonPanels)
        {
            dictionaryButtonPanel.dictionaryButton.onClick.AddListener(() => OpenSingleDictionary(dictionaryButtonPanel.singleDictionaryUI));
        }
    }

    private void OpenSingleDictionary(SingleDictionaryUI singleDictionaryUI)
    {
        OnOpenSingleDictionaryUI?.Invoke(this, new OnOpenSingleDictionaryUIEventArgs { singleDictionaryUI = singleDictionaryUI });
    }

    private DictionaryButtonPanel GetDictionaryButtonPanelByDialect(Dialect dialect)
    {
        foreach (DictionaryButtonPanel dictionaryButtonPanel in dictionaryButtonPanels)
        {
            if (dictionaryButtonPanel.dialect == dialect)
            {
                return dictionaryButtonPanel;
            }
        }

        return null;
    }

    #region SymbolCraftingUI Subscriptions
    private void SymbolCrafingUI_OnSymbolCraftingUIOpenDIctionary(object sender, SymbolCraftingUIDialectSetUpHandler.OnSymbolCraftingOpenDictionaryEventArgs e)
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
