using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ProjectionGemCollectedVFXHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private LearningPlatformLearn learningPlatformLearn;
    [SerializeField] private VisualEffect projectionGemCollectionVFX;

    private bool hasUnsubscribed;

    private void OnEnable()
    {
        learningPlatformLearn.OnObjectLearned += LearningPlatformLearn_OnObjectLearned;
    }

    private void OnDisable()
    {
        UnsubscribeFromLearningPlatformLearnEvents();
    }

    private void Start()
    {
        InitializeVariables();
        InitializeVFX();
    }

    private void InitializeVariables()
    {
        hasUnsubscribed = false;
    }

    private void InitializeVFX()
    {
        projectionGemCollectionVFX.gameObject.SetActive(true);
        projectionGemCollectionVFX.Stop();
    }

    private void StartVFX() => projectionGemCollectionVFX.Play();

    private void StopVFX() => projectionGemCollectionVFX.Stop();

    private void UnsubscribeFromLearningPlatformLearnEvents()
    {
        if (hasUnsubscribed) return;

        learningPlatformLearn.OnObjectLearned -= LearningPlatformLearn_OnObjectLearned;
        hasUnsubscribed = true;
    }


    #region ShieldPieceCollection Subscriptions
    private void LearningPlatformLearn_OnObjectLearned(object sender, LearningPlatformLearn.OnObjectLearnedEventArgs e)
    {
        StartVFX();
        UnsubscribeFromLearningPlatformLearnEvents();     
    }
    #endregion
}
