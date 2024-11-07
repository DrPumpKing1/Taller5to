using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningPlatformProjectionGemHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private LearningPlatformLearn learningPlatformLearn;
    [SerializeField] private Transform model;

    private void OnEnable()
    {
        learningPlatformLearn.OnObjectLearned += LearningPlatformLearn_OnObjectLearned;
        learningPlatformLearn.OnObjectAlreadyLearned += LearningPlatformLearn_OnObjectAlreadyLearned;
    }

    private void OnDisable()
    {
        learningPlatformLearn.OnObjectLearned -= LearningPlatformLearn_OnObjectLearned;
        learningPlatformLearn.OnObjectAlreadyLearned -= LearningPlatformLearn_OnObjectAlreadyLearned;
    }

    private void DisableModel() => model.gameObject.SetActive(false);

    private void LearningPlatformLearn_OnObjectLearned(object sender, LearningPlatformLearn.OnObjectLearnedEventArgs e)
    {
        DisableModel();
    }
    private void LearningPlatformLearn_OnObjectAlreadyLearned(object sender, LearningPlatformLearn.OnObjectLearnedEventArgs e)
    {
        DisableModel();
    }

}
