using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialectKnowledgeSourceCollectedVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private DialectKnowledgeSource dialectKnowledgeSource;

    [Header("Knowledge Added Settings")]
    [SerializeField] private Transform dialectKnowledgeAddedUIPrefab;
    [SerializeField] private Vector3 instantiationPositionOffset;
    [SerializeField] private Vector3 offsetBetweenInstantiatedUIs;

    private void OnEnable()
    {
        dialectKnowledgeSource.OnDialectKnowledgeAdded += DialectKnowledgeSource_OnDialectKnowledgeAdded;
    }

    private void OnDisable()
    {
        dialectKnowledgeSource.OnDialectKnowledgeAdded -= DialectKnowledgeSource_OnDialectKnowledgeAdded;
    }

    private void DialectKnowledgeSource_OnDialectKnowledgeAdded(object sender, DialectKnowledgeSource.OnKnowledgeAddedEventArgs e)
    {
        int instantiatedUIs = 0;

        foreach (DialectKnowledge dialectKnowledge in e.dialectKnowledgeSourceSO.dialectKnowledgeLevelChanges) 
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
}
