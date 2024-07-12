using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PetLearningVFXHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private VisualEffect learningVisualEffect;

    private Transform currentAttentionTransform = null;

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
        InitializeLearningVFX();
    }

    private void Update()
    {
        HandleLearningVisualEffectRotation();
    }

    private void HandleLearningVisualEffectRotation()
    {
        if (!currentAttentionTransform) return;

        Vector3 direction = (currentAttentionTransform.position - learningVisualEffect.transform.position).normalized;
        learningVisualEffect.transform.rotation = Quaternion.LookRotation(direction);
    }

    private void InitializeLearningVFX()
    {
        learningVisualEffect.gameObject.SetActive(true);
        learningVisualEffect.Stop();
    }

    private void LearningPlatformLearn_OnStartLearning(object sender, LearningPlatformLearn.OnLearningEventArgs e)
    {
        learningVisualEffect.Play();
        currentAttentionTransform = e.attentionTransform;
    }

    private void LearningPlatformLearn_OnEndLearning(object sender, LearningPlatformLearn.OnLearningEventArgs e)
    {
        learningVisualEffect.Stop();
        currentAttentionTransform = null;
    }
}
