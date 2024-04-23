using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningSuccessVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private LearningPlatform learningPlatform;

    [Header("Learning Success Settings")]
    [SerializeField] private Transform learningSuccessUIPrefab;
    [SerializeField] private Vector3 instantiationPositionOffset;

    private void OnEnable()
    {
        learningPlatform.OnObjectLearned += LearningPlatform_OnObjectLearned;
    }
    private void OnDisable()
    {
        learningPlatform.OnObjectLearned -= LearningPlatform_OnObjectLearned;
    }

    private void LearningPlatform_OnObjectLearned(object sender, LearningPlatform.OnObjectLearnedEventArgs e)
    {
        GameObject learningSuccessUIGameObject = Instantiate(learningSuccessUIPrefab.gameObject, transform.position + instantiationPositionOffset, transform.rotation);

        LearningSuccessUI learningSuccessUI = learningSuccessUIGameObject.GetComponentInChildren<LearningSuccessUI>();

        if (!learningSuccessUI)
        {
            Debug.LogWarning("There's not a LearningSuccessUI attached to instantiated prefab");
            return;
        }

        learningSuccessUI.SetLearningSuccessText(learningPlatform.ProjectableObjectToLearn);
    }

}
