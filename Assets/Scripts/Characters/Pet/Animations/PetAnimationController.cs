using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetAnimationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    private const string LEARNING_BOOLEAN = "Learning";
    private const string POWERING_BOOLEAN = "Powering";

    private bool learning;
    private bool powering;

    private void OnEnable()
    {
        LearningPlatformLearn.OnStartLearning += LearningPlatformLearn_OnStartLearning;
        LearningPlatformLearn.OnEndLearning += LearningPlatformLearn_OnEndLearning;

        ProjectionPlatformProjection.OnAnyStartProjection += ProjectionPlatformProjection_OnAnyStartProjection;
        ProjectionPlatformProjection.OnAnyEndProjection += ProjectionPlatformProjection_OnAnyEndProjection;

        ProjectableObjectDematerialization.OnAnyStartDematerialization += ProjectableObjectDematerialization_OnAnyStartDematerialization;
        ProjectableObjectDematerialization.OnAnyEndDematerialization += ProjectableObjectDematerialization_OnAnyEndDematerialization;

        ProjectionResetObject.OnAnyStartProjectionResetObjectUse += ProjectionResetObject_OnAnyStartProjectionResetObjectUse;
        ProjectionResetObject.OnAnyEndProjectionResetObjectUse += ProjectionResetObject_OnAnyEndProjectionResetObjectUse;
    }
    private void OnDisable()
    {
        LearningPlatformLearn.OnStartLearning -= LearningPlatformLearn_OnStartLearning;
        LearningPlatformLearn.OnEndLearning -= LearningPlatformLearn_OnEndLearning;

        ProjectionPlatformProjection.OnAnyStartProjection -= ProjectionPlatformProjection_OnAnyStartProjection;
        ProjectionPlatformProjection.OnAnyEndProjection -= ProjectionPlatformProjection_OnAnyEndProjection;

        ProjectableObjectDematerialization.OnAnyStartDematerialization -= ProjectableObjectDematerialization_OnAnyStartDematerialization;
        ProjectableObjectDematerialization.OnAnyEndDematerialization -= ProjectableObjectDematerialization_OnAnyEndDematerialization;

        ProjectionResetObject.OnAnyStartProjectionResetObjectUse -= ProjectionResetObject_OnAnyStartProjectionResetObjectUse;
        ProjectionResetObject.OnAnyEndProjectionResetObjectUse -= ProjectionResetObject_OnAnyEndProjectionResetObjectUse;
    }

    private void Start()
    {
        SetPowering(false);
        SetLearning(false);
    }

    private void Update()
    {
        UpdateAnimatorBooleans();
    }

    private void UpdateAnimatorBooleans()
    {
        animator.SetBool(LEARNING_BOOLEAN, learning);
        animator.SetBool(POWERING_BOOLEAN, powering);
    }

    private void SetLearning(bool learning) => this.learning = learning;
    private void SetPowering(bool powering) => this.powering = powering;

    #region Learning
    private void LearningPlatformLearn_OnStartLearning(object sender, LearningPlatformLearn.OnLearningEventArgs e)
    {
        SetLearning(true);
    }

    private void LearningPlatformLearn_OnEndLearning(object sender, LearningPlatformLearn.OnLearningEventArgs e)
    {
        SetLearning(false);
    }
    #endregion


    #region Projecting
    private void ProjectionPlatformProjection_OnAnyStartProjection(object sender, ProjectionPlatformProjection.OnProjectionEventArgs e)
    {
        SetPowering(true);
    }
    private void ProjectionPlatformProjection_OnAnyEndProjection(object sender, ProjectionPlatformProjection.OnProjectionEventArgs e)
    {
        SetPowering(false);
    }
    #endregion

    #region Dematerializing
    private void ProjectableObjectDematerialization_OnAnyStartDematerialization(object sender, ProjectableObjectDematerialization.OnAnyObjectDematerializedEventArgs e)
    {
        SetPowering(true);
    }

    private void ProjectableObjectDematerialization_OnAnyEndDematerialization(object sender, ProjectableObjectDematerialization.OnAnyObjectDematerializedEventArgs e)
    {
        SetPowering(false);
    }
    #endregion

    #region ProjectionResetObject
    private void ProjectionResetObject_OnAnyStartProjectionResetObjectUse(object sender, System.EventArgs e)
    {
        SetPowering(true);
    }

    private void ProjectionResetObject_OnAnyEndProjectionResetObjectUse(object sender, System.EventArgs e)
    {
        SetPowering(false);
    }
    #endregion
}
