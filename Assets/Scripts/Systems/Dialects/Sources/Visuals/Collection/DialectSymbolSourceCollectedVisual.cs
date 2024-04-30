using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialectSymbolSourceCollectedVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private DialectSymbolSourceCollection dialectSymbolSourceCollection;

    [Header("Symbols Added Settings")]
    [SerializeField] private Transform symbolsAddedUIPrefab;
    [SerializeField] private Vector3 symbolsInstantiationPositionOffset;

    private void OnEnable()
    {
        dialectSymbolSourceCollection.OnSymbolsAdded += DialectSymbolSourceCollection_OnSymbolsAdded;
    }

    private void OnDisable()
    {
        dialectSymbolSourceCollection.OnSymbolsAdded -= DialectSymbolSourceCollection_OnSymbolsAdded;
    }

    private void DialectSymbolSourceCollection_OnSymbolsAdded(object sender, DialectSymbolSourceCollection.OnSymbolsAddedEventArgs e)
    {
        InstantiateSymbolsAddedUI(e.dialectSymbolSourceSO.dialectSymbolSOs);
    }

    private void InstantiateSymbolsAddedUI(List<DialectSymbolSO> dialectSymbolsSOs)
    {
        GameObject symbolsAddedUIGameObject = Instantiate(symbolsAddedUIPrefab.gameObject, transform.position + symbolsInstantiationPositionOffset, transform.rotation);

        SymbolsAddedUI symbolsAddedUI = symbolsAddedUIGameObject.GetComponentInChildren<SymbolsAddedUI>();

        if (!symbolsAddedUI)
        {
            Debug.LogWarning("There's not a SymbolsAddedUI attached to instantiated prefab");
            return;
        }

        symbolsAddedUI.SetSymbolsUIs(dialectSymbolsSOs);
    }
}
