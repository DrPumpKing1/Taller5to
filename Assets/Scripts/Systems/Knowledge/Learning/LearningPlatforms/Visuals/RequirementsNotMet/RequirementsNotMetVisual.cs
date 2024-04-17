using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequirementsNotMetVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private LearningPlatform learningPlatform;

    [Header("Requirements Not Met Settings")]
    [SerializeField] private RequirementsNotMetVisualSettingsSO RequirementsNotMetVisualSettingsSO;

    private float cooldownTimer;

    private void OnEnable()
    {
        learningPlatform.OnKnowledgeRequirementsNotMet += LearningPlatform_OnKnowledgeRequirementsNotMet;
    }
    private void OnDisable()
    {
        learningPlatform.OnKnowledgeRequirementsNotMet -= LearningPlatform_OnKnowledgeRequirementsNotMet;
    }

    private void Update()
    {
        HandleVisualCooldown();
    }

    private void HandleVisualCooldown()
    {
        if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;
    }

    private void LearningPlatform_OnKnowledgeRequirementsNotMet(object sender, IRequiresKnowledge.OnKnowledgeRequirementsNotMetEventArgs e)
    {
        if (cooldownTimer > 0) return;

        GameObject requirementsNotMetUIGameObject = Instantiate(RequirementsNotMetVisualSettingsSO.requirementsNotMetUIPrefab.gameObject, transform.position + RequirementsNotMetVisualSettingsSO.instantiationPositionOffset, transform.rotation);
        
        RequirementsNotMetUI requirementsNotMetUI = requirementsNotMetUIGameObject.GetComponent<RequirementsNotMetUI>();

        if (!requirementsNotMetUI)
        {
            Debug.LogWarning("There's not a requirementsNotMetUI attached to instantiated prefab");
            return;
        }

        cooldownTimer = requirementsNotMetUI.LifeTime;
    }


}
