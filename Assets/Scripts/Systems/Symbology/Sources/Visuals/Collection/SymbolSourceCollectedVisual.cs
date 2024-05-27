using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolSourceCollectedVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SymbolSourceCollection symbolSourceCollection;

    [Header("Symbols Added Settings")]
    [SerializeField] private Transform symbolsAddedUIPrefab;
    [SerializeField] private Vector3 symbolsInstantiationPositionOffset;

    private void OnEnable()
    {
        symbolSourceCollection.OnSymbolsAdded += SymbolSourceCollection_OnSymbolsAdded;
    }

    private void OnDisable()
    {
        symbolSourceCollection.OnSymbolsAdded -= SymbolSourceCollection_OnSymbolsAdded;
    }

    private void SymbolSourceCollection_OnSymbolsAdded(object sender, SymbolSourceCollection.OnSymbolsAddedEventArgs e)
    {
        InstantiateSymbolsAddedUI(e.symbolSourceSO.symbolSOs);
    }

    private void InstantiateSymbolsAddedUI(List<SymbolSO> symbolSOs)
    {
        GameObject symbolsAddedUIGameObject = Instantiate(symbolsAddedUIPrefab.gameObject, transform.position + symbolsInstantiationPositionOffset, transform.rotation);

        SymbolsAddedUI symbolsAddedUI = symbolsAddedUIGameObject.GetComponentInChildren<SymbolsAddedUI>();

        if (!symbolsAddedUI)
        {
            Debug.LogWarning("There's not a SymbolsAddedUI attached to instantiated prefab");
            return;
        }

        symbolsAddedUI.SetSymbolsUIs(symbolSOs);
    }
}
