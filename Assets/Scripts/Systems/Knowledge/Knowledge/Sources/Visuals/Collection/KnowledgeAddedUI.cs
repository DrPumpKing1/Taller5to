using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KnowledgeAddedUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI knowledgeAddedText;

    [Header("Settings")]
    [SerializeField] private float lifeTime;

    private void Awake()
    {
        DestroyAfterLifetime();
    }

    public void SetKnowledgeAddedText(DialectKnowledge dialectKnowledgeAdded)
    {
        string levelsText = "Nivel";
        if (dialectKnowledgeAdded.level > 1) levelsText = "Niveles";

        knowledgeAddedText.text = $"+{dialectKnowledgeAdded.level} {levelsText} de Dialecto {dialectKnowledgeAdded.dialect}";
    }

    private void DestroyAfterLifetime() => Destroy(transform.parent.gameObject, lifeTime);
}
