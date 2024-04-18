using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class LearningPlatformInfoPanelUI : MonoBehaviour
{
    [Header("Projectable Object UI Components")]
    [SerializeField] private TextMeshProUGUI projectableObjectName;
    [SerializeField] private Image projectableObjectImage;
    [SerializeField] private TextMeshProUGUI projectableObjectDescription;

    [Header("Dialect Requirements UI Components")]
    [SerializeField] private Transform dialectKnowledgeRequirementsUIsContainer;
    [SerializeField] private Transform dialectKnowledgeRequirementsTemplate;

    private void Start()
    {
        DisableDialectRequirementsUITemplate();
    }

    public void SetInfoPanelContents(ProjectableObjectSO projectableObjectToLearn, List<DialectKnowledge> dialectKnowledgeRequirements)
    {
        SetProjectableObjectContents(projectableObjectToLearn);
        SetDialectKnowledgeRequirementsContents(dialectKnowledgeRequirements);
    }

    private void SetProjectableObjectContents(ProjectableObjectSO projectableObjectToLearn)
    {
        projectableObjectName.text = projectableObjectToLearn.objectName;
        projectableObjectImage.sprite = projectableObjectToLearn.sprite;
        projectableObjectDescription.text = projectableObjectToLearn.description;
    }

    private void SetDialectKnowledgeRequirementsContents(List<DialectKnowledge> dialectKnowledgeRequirements)
    {
        foreach (DialectKnowledge dialectKnowledge in dialectKnowledgeRequirements)
        {
            Transform dialectRequirementsUIGameObject = Instantiate(dialectKnowledgeRequirementsTemplate, dialectKnowledgeRequirementsUIsContainer);
            dialectRequirementsUIGameObject.gameObject.SetActive(true);

            LearningPlatformKnowledgeRequirementsSingleUI knowledgeRequirementUI = dialectRequirementsUIGameObject.GetComponent<LearningPlatformKnowledgeRequirementsSingleUI>();

            if (!knowledgeRequirementUI)
            {
                Debug.LogWarning("There's not a LearningPlatformKnowledgeRequirementsSingleUI attached to instantiated prefab");
                continue;
            }

            knowledgeRequirementUI.SetDialectKnowledgeRequirementsContents(dialectKnowledge);
        }
    }

    private void DisableDialectRequirementsUITemplate() => dialectKnowledgeRequirementsTemplate.gameObject.SetActive(false);

    public void DestroyPanel() => Destroy(gameObject);
}
