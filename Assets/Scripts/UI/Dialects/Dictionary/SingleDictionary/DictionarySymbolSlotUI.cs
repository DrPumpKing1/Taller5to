using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DictionarySymbolSlotUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image symbolImage;
    [SerializeField] private Image meaningImage;

    [Header("Settings")]
    [SerializeField] private DialectSymbolSO dialectSymbolSO;
    [SerializeField] private bool symbolShown;

    public DialectSymbolSO DialectSymbolSO { get { return dialectSymbolSO; } }

    private void OnEnable()
    {
        SymbolsDictionaryManager.OnDialectSymbolCollected += SymbolsDictionaryManager_OnDialectSymbolCollected;
    }

    private void OnDisable()
    {
        SymbolsDictionaryManager.OnDialectSymbolCollected += SymbolsDictionaryManager_OnDialectSymbolCollected;
    }

    private void Start()
    {
        CheckShowSymbol();
    }

    private void CheckShowSymbol()
    {
        if (!SymbolsDictionaryManager.Instance.SymbolsDictionary.Contains(dialectSymbolSO)) return;

        if (!symbolShown)
        {
            EnableSymbolDiscovery();
        }
        else
        {
            ShowSymbol();
        }
    }

    private void ShowSymbol()
    {
        ShowSymbolImage();
        ShowMeaningImage();

        symbolShown = true;
    }

    private void ShowSymbolImage() => symbolImage.sprite = dialectSymbolSO.symbolImage;
    private void ShowMeaningImage() => meaningImage.sprite = dialectSymbolSO.meaningImage;

    private void EnableSymbolDiscovery()
    {
        ShowSymbol();
    }

    public void SetSymbolShown() => symbolShown = true;

    private void CheckComposedSymbolFormed()
    {
        if (SymbolsDictionaryManager.Instance.SymbolsDictionary.Contains(dialectSymbolSO)) return;
        if (dialectSymbolSO.IsPrimary()) return;

        foreach(DialectSymbolSO originator in dialectSymbolSO.originators)
        {
            if (!SymbolsDictionaryManager.Instance.SymbolsDictionary.Contains(originator)) return;
        }

        EnableSymbolDiscovery();
    }

    #region SymbolsDictionaryManager Subscriptions
    private void SymbolsDictionaryManager_OnDialectSymbolCollected(object sender, SymbolsDictionaryManager.OnDialectSymbolCollectedEventArgs e)
    {
        if (dialectSymbolSO == e.collectedSymbol) ShowSymbol();
        else CheckComposedSymbolFormed();
    }

    #endregion
}
