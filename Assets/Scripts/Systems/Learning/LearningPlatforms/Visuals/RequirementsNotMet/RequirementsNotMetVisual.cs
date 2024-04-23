using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequirementsNotMetVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private LearningPlatform learningPlatform;

    [Header("Requirements Not Met Settings")]
    [SerializeField] private Transform requirementsNotMetUIPrefab;
    [SerializeField] private Vector3 instantiationPositionOffset;

    private float cooldownTimer;

    private void OnEnable()
    {
        learningPlatform.OnDialectKnowledgeRequirementsNotMet += LearningPlatform_OnKnowledgeRequirementsNotMet;
    }
    private void OnDisable()
    {
        learningPlatform.OnDialectKnowledgeRequirementsNotMet -= LearningPlatform_OnKnowledgeRequirementsNotMet;
    }

    private void Update()
    {
        HandleVisualCooldown();
    }

    private void HandleVisualCooldown()
    {
        if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;
    }

    private void LearningPlatform_OnKnowledgeRequirementsNotMet(object sender, IRequiresDialectKnowledge.OnDialectKnowledgeRequirementsNotMetEventArgs e)
    {
        if (cooldownTimer > 0) return;

        GameObject requirementsNotMetUIGameObject = Instantiate(requirementsNotMetUIPrefab.gameObject, transform.position + instantiationPositionOffset, transform.rotation);
        
        LearningRequirementsNotMetUI requirementsNotMetUI = requirementsNotMetUIGameObject.GetComponentInChildren<LearningRequirementsNotMetUI>();

        if (!requirementsNotMetUI)
        {
            Debug.LogWarning("There's not a RequirementsNotMetUI attached to instantiated prefab");
            return;
        }

        cooldownTimer = requirementsNotMetUI.LifeTime;
    }
}
