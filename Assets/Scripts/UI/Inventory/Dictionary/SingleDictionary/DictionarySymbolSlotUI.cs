using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DictionarySymbolSlotUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private DialectSymbolSO dialectSymbolSO;

    [Header("UI Components")]
    [SerializeField] private Image symbolImage;
    [SerializeField] private Image meaningImage;
    [SerializeField] private Button discoverButton;

    public DialectSymbolSO DialectSymbolSO => dialectSymbolSO;
    private bool symbolDiscoveryUIEnabled;

    private void OnEnable()
    {
        SymbolsDictionaryManager.OnDialectSymbolCollected += SymbolsDictionaryManager_OnDialectSymbolCollected;
    }

    private void OnDisable()
    {
        SymbolsDictionaryManager.OnDialectSymbolCollected -= SymbolsDictionaryManager_OnDialectSymbolCollected;
    }

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void Start()
    {
        InitializeVariables();
        CheckShowSymbol();
    }

    private void InitializeButtonsListeners()
    {
        discoverButton.onClick.AddListener(CollectDiscoveredSymbol);
    }

    private void InitializeVariables()
    {
        symbolDiscoveryUIEnabled = false;
    }

    private void CheckShowSymbol()
    {
        if (CheckSymbolInDictionary())
        {
            ShowSymbol();
        }
        else if (CheckOriginatorsInDictionary())
        {
            EnableSymbolDiscoveryUI();
        }
    }

    private void ShowSymbol()
    {
        ShowSymbolImage();
        ShowMeaningImage();
    }

    private void ShowSymbolImage() => symbolImage.sprite = dialectSymbolSO.symbolImage;
    private void ShowMeaningImage() => meaningImage.sprite = dialectSymbolSO.meaningImage;
    private void CollectDiscoveredSymbol()
    {
        DisableSymbolDiscoveryUI();

        SymbolsDictionaryManager.Instance.CollectSymbol(dialectSymbolSO);
        ShowSymbol();
    }

    private void EnableSymbolDiscoveryUI()
    {
        if (symbolDiscoveryUIEnabled) return;

        discoverButton.gameObject.SetActive(true);
        symbolDiscoveryUIEnabled = true;
    }

    private void DisableSymbolDiscoveryUI()
    {
        if (!symbolDiscoveryUIEnabled) return;

        discoverButton.gameObject.SetActive(false);
        symbolDiscoveryUIEnabled = false;
    }


    private bool CheckSymbolInDictionary() => SymbolsDictionaryManager.Instance.SymbolsDictionary.Contains(dialectSymbolSO);
    private bool CheckOriginatorsInDictionary()
    {
        if (dialectSymbolSO.IsPrimary()) return false;

        foreach (DialectSymbolSO originator in dialectSymbolSO.originators)
        {
            if (!SymbolsDictionaryManager.Instance.SymbolsDictionary.Contains(originator)) return false;
        }

        return true;
    }

    private void CheckComposedSymbolFormed()
    {
        if (CheckSymbolInDictionary()) return;
        if (!CheckOriginatorsInDictionary()) return;

        EnableSymbolDiscoveryUI();
    }

    #region SymbolsDictionaryManager Subscriptions
    private void SymbolsDictionaryManager_OnDialectSymbolCollected(object sender, SymbolsDictionaryManager.OnDialectSymbolCollectedEventArgs e)
    {
        if (dialectSymbolSO == e.collectedSymbol)
        {
            ShowSymbol();
        }
        else
        {
            CheckComposedSymbolFormed();
        }
    }

    #endregion
}
