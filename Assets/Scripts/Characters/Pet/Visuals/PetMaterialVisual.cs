using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetMaterialVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Renderer _renderer;

    [Header("Settings")]
    [SerializeField] private Material vyrxMaterial;
    [SerializeField] private Material vyrxPowerMaterial;

    private void OnEnable()
    {
        ProjectionPlatformProjection.OnAnyStartProjection += ProjectionPlatformProjection_OnAnyStartProjection;
        ProjectionPlatformProjection.OnAnyEndProjection += ProjectionPlatformProjection_OnAnyEndProjection;

        ProjectableObjectDematerialization.OnAnyStartDematerialization += ProjectableObjectDematerialization_OnAnyStartDematerialization;
        ProjectableObjectDematerialization.OnAnyEndDematerialization += ProjectableObjectDematerialization_OnAnyEndDematerialization;

        ProjectionResetObject.OnAnyStartProjectionResetObjectUse += ProjectionResetObject_OnAnyStartProjectionResetObjectUse;
        ProjectionResetObject.OnAnyEndProjectionResetObjectUse += ProjectionResetObject_OnAnyEndProjectionResetObjectUse;

        LearningPlatformLearn.OnStartLearning += LearningPlatformLearn_OnStartLearning;
        LearningPlatformLearn.OnEndLearning += LearningPlatformLearn_OnEndLearning;
    }

    private void OnDisable()
    {
        ProjectionPlatformProjection.OnAnyStartProjection -= ProjectionPlatformProjection_OnAnyStartProjection;
        ProjectionPlatformProjection.OnAnyEndProjection -= ProjectionPlatformProjection_OnAnyEndProjection;

        ProjectableObjectDematerialization.OnAnyStartDematerialization -= ProjectableObjectDematerialization_OnAnyStartDematerialization;
        ProjectableObjectDematerialization.OnAnyEndDematerialization -= ProjectableObjectDematerialization_OnAnyEndDematerialization;

        ProjectionResetObject.OnAnyStartProjectionResetObjectUse -= ProjectionResetObject_OnAnyStartProjectionResetObjectUse;
        ProjectionResetObject.OnAnyEndProjectionResetObjectUse -= ProjectionResetObject_OnAnyEndProjectionResetObjectUse;

        LearningPlatformLearn.OnStartLearning -= LearningPlatformLearn_OnStartLearning;
        LearningPlatformLearn.OnEndLearning -= LearningPlatformLearn_OnEndLearning;
    }

    private void Awake()
    {
        SetNormalMaterial();
    }

    private void SetNormalMaterial() => GeneralRenderingMethods.SetRendererMaterial(_renderer, vyrxMaterial);
    private void SetPowerMaterial() => GeneralRenderingMethods.SetRendererMaterial(_renderer, vyrxPowerMaterial);


    #region ProjectionPlatformProjection Subscriptions
    private void ProjectionPlatformProjection_OnAnyStartProjection(object sender, ProjectionPlatformProjection.OnProjectionEventArgs e)
    {
        SetPowerMaterial();
    }

    private void ProjectionPlatformProjection_OnAnyEndProjection(object sender, ProjectionPlatformProjection.OnProjectionEventArgs e)
    {
        SetNormalMaterial();
    }
    #endregion

    #region ProjectableObjectDematerialization Subscriptions
    private void ProjectableObjectDematerialization_OnAnyStartDematerialization(object sender, ProjectableObjectDematerialization.OnAnyObjectDematerializedEventArgs e)
    {
        SetPowerMaterial();
    }
    private void ProjectableObjectDematerialization_OnAnyEndDematerialization(object sender, ProjectableObjectDematerialization.OnAnyObjectDematerializedEventArgs e)
    {
        SetNormalMaterial();
    }
    #endregion

    #region ProjectionResetObject Subscriptions
    private void ProjectionResetObject_OnAnyStartProjectionResetObjectUse(object sender, System.EventArgs e)
    {
        SetPowerMaterial();
    }
    private void ProjectionResetObject_OnAnyEndProjectionResetObjectUse(object sender, System.EventArgs e)
    {
        SetNormalMaterial();
    }
    #endregion

    #region LearningPlatformLearn Subscriptions
    private void LearningPlatformLearn_OnStartLearning(object sender, LearningPlatformLearn.OnLearningEventArgs e)
    {
        SetPowerMaterial();
    }
    private void LearningPlatformLearn_OnEndLearning(object sender, LearningPlatformLearn.OnLearningEventArgs e)
    {
        SetNormalMaterial();
    }
    #endregion
}
