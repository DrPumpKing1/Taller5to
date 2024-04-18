using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LearningPlatformKnowledgeRequirementsSingleUI : MonoBehaviour
{
    [Header("Single UI Components")]
    [SerializeField] private TextMeshProUGUI dialectNameText;
    [SerializeField] private TextMeshProUGUI dialectLevelText;

    public void SetDialectKnowledgeRequirementsContents(DialectKnowledge dialectKnowledge)
    {
        dialectNameText.text = $"Dialecto {dialectKnowledge.dialect}";
        dialectLevelText.text = $"Nivel {dialectKnowledge.level}";
    }
}
