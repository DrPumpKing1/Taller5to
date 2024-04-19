using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnowledgeSourceCollectedVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private KnowledgeSource knowledgeSource;

    [Header("Knowledge Added Settings")]
    [SerializeField] private Transform knowledgeAddedUIPrefab;
    [SerializeField] private Vector3 instantiationPositionOffset;
    [SerializeField] private Vector3 offsetBetweenInstantiatedUIs;

    private void OnEnable()
    {
        knowledgeSource.OnKnowledgeAdded += KnowledgeSource_OnKnowledgeAdded;
    }

    private void OnDisable()
    {
        knowledgeSource.OnKnowledgeAdded -= KnowledgeSource_OnKnowledgeAdded;
    }

    private void KnowledgeSource_OnKnowledgeAdded(object sender, KnowledgeSource.OnKnowledgeAddedEventArgs e)
    {
        int instantiatedUIs = 0;

        foreach (DialectKnowledge dialectKnowledge in e.knowledgeSourceSO.dialectKnowledgeLevelChanges) 
        {
            GameObject knowledgeAddedUIGameObject = Instantiate(knowledgeAddedUIPrefab.gameObject, transform.position + instantiationPositionOffset + offsetBetweenInstantiatedUIs * instantiatedUIs, transform.rotation);

            KnowledgeAddedUI knowledgeAddedUI = knowledgeAddedUIGameObject.GetComponentInChildren<KnowledgeAddedUI>();

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
