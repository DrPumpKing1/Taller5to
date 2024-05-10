using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DictionarySymbolSlotUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image symbolImage;
    [SerializeField] private Image meaningImage;
    [SerializeField] private Button discoverButton;

    [Header("Settings")]
    [SerializeField] private DialectSymbolSO dialectSymbolSO;

    [Header("Identifiers")]
    [SerializeField] private int id;
    [SerializeField] private bool symbolDiscovered;

    public DialectSymbolSO DialectSymbolSO { get { return dialectSymbolSO; } }

    private void OnEnable()
    {
        SymbolsDictionaryManager.OnDialectSymbolCollected += SymbolsDictionaryManager_OnDialectSymbolCollected;
    }

    private void OnDisable()
    {
        SymbolsDictionaryManager.OnDialectSymbolCollected += SymbolsDictionaryManager_OnDialectSymbolCollected;
    }

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void Start()
    {
        CheckShowSymbol();
    }

    private void InitializeButtonsListeners()
    {
        discoverButton.onClick.AddListener(CollectDiscoveredSymbol);
    }

    private void CheckShowSymbol()
    {
        if (SymbolsDictionaryManager.Instance.SymbolsDictionary.Contains(dialectSymbolSO))
        {
            ShowSymbol();
        }
        else if (symbolDiscovered)
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

    private void EnableSymbolDiscoveryUI()
    {
        discoverButton.gameObject.SetActive(true);
    }

    private void CollectDiscoveredSymbol()
    {
        discoverButton.gameObject.SetActive(false);

        ShowSymbol();
        SymbolsDictionaryManager.Instance.CollectSymbol(dialectSymbolSO);
    }

    public void SetSymbolDiscovered() => symbolDiscovered = true;

    private void CheckComposedSymbolFormed()
    {
        if (symbolDiscovered) return;
        if (SymbolsDictionaryManager.Instance.SymbolsDictionary.Contains(dialectSymbolSO)) return;
        if (dialectSymbolSO.IsPrimary()) return;

        foreach(DialectSymbolSO originator in dialectSymbolSO.originators)
        {
            if (!SymbolsDictionaryManager.Instance.SymbolsDictionary.Contains(originator)) return;
        }

        SetSymbolDiscovered();
        EnableSymbolDiscoveryUI();
    }

    #region SymbolsDictionaryManager Subscriptions
    private void SymbolsDictionaryManager_OnDialectSymbolCollected(object sender, SymbolsDictionaryManager.OnDialectSymbolCollectedEventArgs e)
    {
        if (dialectSymbolSO == e.collectedSymbol) ShowSymbol();
        else CheckComposedSymbolFormed();
    }

    #endregion
}
