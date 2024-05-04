using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class SymbolCrafingUIHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SymbolCrafting symbolCrafting;

    [Header("UI Components")]
    [SerializeField] private Transform symbolCraftingSingleUIContainer;
    [SerializeField] private Transform symbolCraftingSingleUIPrefab;

    private List<SymbolCraftingSO> symbolCraftingSOs => symbolCrafting.SymbolCraftingSOs;
    private List<SymbolCraftingSingleUI> symbolCraftingSingleUIs = new List<SymbolCraftingSingleUI>();

    public static event EventHandler<OnAllSymbolsCraftedEventArgs> OnAllSymbolsCrafted;

    private void OnDisable()
    {
        UnsubscribeToAllSymbolCraftingSingleUIs();
    }

    public class OnAllSymbolsCraftedEventArgs : EventArgs
    {
        public SymbolCrafting symbolCrafting;
    }

    private void Start()
    {
        InstantiateSymbolCraftingSingleUIs();
    }

    private void InstantiateSymbolCraftingSingleUIs()
    {
        foreach(SymbolCraftingSO symbolCraftingSO in symbolCraftingSOs)
        {
            Transform symbolCraftingSingleUITransform = Instantiate(symbolCraftingSingleUIPrefab, symbolCraftingSingleUIContainer);

            SymbolCraftingSingleUI symbolCraftingSingleUI = symbolCraftingSingleUITransform.GetComponentInChildren<SymbolCraftingSingleUI>();

            if (!symbolCraftingSingleUI)
            {
                Debug.LogWarning("The instantiated transform does not have a SymbolCraftingSingleUI component");
                continue;
            }

            symbolCraftingSingleUI.SetSymbolCraftingSingleUI(symbolCraftingSO);
            symbolCraftingSingleUIs.Add(symbolCraftingSingleUI);
        }

        SubscribeToAllSymbolCraftingSingleUIs();
    }

    private void SubscribeToAllSymbolCraftingSingleUIs()
    {
        foreach(SymbolCraftingSingleUI symbolAddedSingleUI in symbolCraftingSingleUIs)
        {
            symbolAddedSingleUI.OnSymbolCrafted += SymbolAddedSingleUI_OnSymbolCrafted;
        }
    }

    private void UnsubscribeToAllSymbolCraftingSingleUIs()
    {
        foreach (SymbolCraftingSingleUI symbolAddedSingleUI in symbolCraftingSingleUIs)
        {
            symbolAddedSingleUI.OnSymbolCrafted -= SymbolAddedSingleUI_OnSymbolCrafted;
        }
    }

    private void CheckAllSymbolsCrafted()
    {
        foreach(SymbolCraftingSingleUI symbolCraftingSingleUI in symbolCraftingSingleUIs)
        {
            if (!symbolCraftingSingleUI.IsCrafted) return;
        }

        OnAllSymbolsCrafted?.Invoke(this, new OnAllSymbolsCraftedEventArgs { symbolCrafting = symbolCrafting });
    }

    #region SymbolCraftingSingleUI Subscriptions
    private void SymbolAddedSingleUI_OnSymbolCrafted(object sender, EventArgs e)
    {
        CheckAllSymbolsCrafted();
    }
    #endregion

}
