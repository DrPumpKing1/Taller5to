using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialectKnowledgeSourceCollectedVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private DialectKnowledgeSourceCollection dialectKnowledgeSourceCollection;

    [Header("Knowledge Added Settings")]
    [SerializeField] private Transform dialectKnowledgeAddedUIPrefab;
    [SerializeField] private Vector3 instantiationPositionOffset;
    [SerializeField] private Vector3 offsetBetweenInstantiatedUIs;

    [Header("Symbols Added Settings")]
    [SerializeField] private Transform symbolsAddedUIPrefab;
    [SerializeField] private Vector3 symbolsInstantiationPositionOffset;

    private void OnEnable()
    {
        dialectKnowledgeSourceCollection.OnDialectKnowledgeAdded += DialectKnowledgeSourceCollection_OnDialectKnowledgeAdded;
        dialectKnowledgeSourceCollection.OnSymbolsAdded += DialectKnowledgeSourceCollection_OnSymbolsAdded;
    }

    private void OnDisable()
    {
        dialectKnowledgeSourceCollection.OnDialectKnowledgeAdded -= DialectKnowledgeSourceCollection_OnDialectKnowledgeAdded;
        dialectKnowledgeSourceCollection.OnSymbolsAdded -= DialectKnowledgeSourceCollection_OnSymbolsAdded;
    }

    private void DialectKnowledgeSourceCollection_OnDialectKnowledgeAdded(object sender, DialectKnowledgeSourceCollection.OnDialectKnowledgeAddedEventArgs e)
    {
        //InstantiateKnowledgeAddedUIs(e.dialectKnowledgeSourceSO.dialectKnowledgeLevelChanges);   
    }

    private void DialectKnowledgeSourceCollection_OnSymbolsAdded(object sender, DialectKnowledgeSourceCollection.OnSymbolsAddedEventArgs e)
    {
        InstantiateSymbolsAddedUI(e.dialectKnowledgeSourceSO.dialectSymbolSOs);
    }

    private void InstantiateKnowledgeAddedUIs(List<DialectKnowledge> dialectKnowledges)
    {
        int instantiatedUIs = 0;

        foreach (DialectKnowledge dialectKnowledge in dialectKnowledges)
        {
            GameObject knowledgeAddedUIGameObject = Instantiate(dialectKnowledgeAddedUIPrefab.gameObject, transform.position + instantiationPositionOffset + offsetBetweenInstantiatedUIs * instantiatedUIs, transform.rotation);

            DialectKnowledgeAddedUI knowledgeAddedUI = knowledgeAddedUIGameObject.GetComponentInChildren<DialectKnowledgeAddedUI>();

            if (!knowledgeAddedUI)
            {
                Debug.LogWarning("There's not a KnowledgeAddeddUI attached to instantiated prefab");
                continue;
            }

            knowledgeAddedUI.SetKnowledgeAddedText(dialectKnowledge);

            instantiatedUIs++;
        }
    }

    private void InstantiateSymbolsAddedUI(List<DialectSymbolSO> dialectSymbolsSOs)
    {
        GameObject symbolsAddedUIGameObject = Instantiate(symbolsAddedUIPrefab.gameObject, transform.position + instantiationPositionOffset, transform.rotation);

        SymbolsAddedUI symbolsAddedUI = symbolsAddedUIGameObject.GetComponentInChildren<SymbolsAddedUI>();

        if (!symbolsAddedUI)
        {
            Debug.LogWarning("There's not a SymbolsAddedUI attached to instantiated prefab");
            return;
        }

        symbolsAddedUI.SetSymbolsUIs(dialectSymbolsSOs);
    }
}
