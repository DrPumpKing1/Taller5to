using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningSuccessVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private LearningPlatformLearnCrafting learningPlatformLearn;

    [Header("Learning Success Settings")]
    [SerializeField] private Transform learningSuccessUIPrefab;
    [SerializeField] private Vector3 instantiationPositionOffset;

    private void OnEnable()
    {
        learningPlatformLearn.OnObjectLearned += LearningPlatform_OnObjectLearned;
    }
    private void OnDisable()
    {
        learningPlatformLearn.OnObjectLearned -= LearningPlatform_OnObjectLearned;
    }

    private void LearningPlatform_OnObjectLearned(object sender, LearningPlatformLearnCrafting.OnObjectLearnedEventArgs e)
    {
        Transform learningSuccessUITransform = Instantiate(learningSuccessUIPrefab, transform.position + instantiationPositionOffset, transform.rotation);

        LearningSuccessUI learningSuccessUI = learningSuccessUITransform.GetComponentInChildren<LearningSuccessUI>();

        if (!learningSuccessUI)
        {
            Debug.LogWarning("There's not a LearningSuccessUI attached to instantiated prefab");
            return;
        }

        learningSuccessUI.SetLearningSuccessText(e.objectLearned);
    }

}
