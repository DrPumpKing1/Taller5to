using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PetLearningVFXHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private VisualEffect learningVFX;

    private const string DURATION_PROPERTY = "Duration";
    private const string SPHERE_WORLD_POSITION_PROPERTY = "SphereWorldPosition";

    private void OnEnable()
    {
        LearningPlatformLearn.OnStartLearning += LearningPlatformLearn_OnStartLearning;
        LearningPlatformLearn.OnEndLearning += LearningPlatformLearn_OnEndLearning;
    }

    private void OnDisable()
    {
        LearningPlatformLearn.OnStartLearning -= LearningPlatformLearn_OnStartLearning;
        LearningPlatformLearn.OnEndLearning -= LearningPlatformLearn_OnEndLearning;
    }

    private void Start()
    {
        InitializeVFX();
    }

    private void Update()
    {
        HandleVFXRotation();
    }
    private void InitializeVFX()
    {
        learningVFX.gameObject.SetActive(true);
        learningVFX.Stop();
    }

    private void HandleVFXRotation()
    {
        learningVFX.transform.rotation = Quaternion.identity;
    }

    private void SetVFXDuration(float duration)
    {
        if (learningVFX.HasFloat(DURATION_PROPERTY))
        {
            learningVFX.SetFloat(DURATION_PROPERTY, duration);
        }
    }

    private void SetVFXSphereWorldPosition(Vector3 position)
    {
        if (learningVFX.HasVector3(SPHERE_WORLD_POSITION_PROPERTY))
        {
            learningVFX.SetVector3(SPHERE_WORLD_POSITION_PROPERTY, position);
        }
    }

    private void StartVFX(float holdDuration, Vector3 projectionGemPosition)
    {
        SetVFXDuration(holdDuration);
        SetVFXSphereWorldPosition(projectionGemPosition);

        learningVFX.Play();
    }

    private void StopVFX() => learningVFX.Stop();

    private void LearningPlatformLearn_OnStartLearning(object sender, LearningPlatformLearn.OnLearningEventArgs e)
    {
        StartVFX(e.holdDuration, e.projectionGemCenter.position);
    }

    private void LearningPlatformLearn_OnEndLearning(object sender, LearningPlatformLearn.OnLearningEventArgs e)
    {
        StopVFX();
    }
}
