using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PetLearningVFXHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private VisualEffect learningVFX;

    private Transform currentAttentionTransform = null;

    private const string DURATION_PROPERTY = "Duration";

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
        if (!currentAttentionTransform) return;

        Vector3 direction = (currentAttentionTransform.position - learningVFX.transform.position).normalized;
        learningVFX.transform.rotation = Quaternion.LookRotation(direction);
    }

    private void SetVFXDuration(float duration)
    {
        if (learningVFX.HasFloat(DURATION_PROPERTY))
        {
            learningVFX.SetFloat(DURATION_PROPERTY, duration);
        }
    }

    private void LearningPlatformLearn_OnStartLearning(object sender, LearningPlatformLearn.OnLearningEventArgs e)
    {
        SetVFXDuration(e.holdDuration);

        learningVFX.Play();
        currentAttentionTransform = e.attentionTransform;
    }

    private void LearningPlatformLearn_OnEndLearning(object sender, LearningPlatformLearn.OnLearningEventArgs e)
    {
        learningVFX.Stop();
        currentAttentionTransform = null;
    }
}
