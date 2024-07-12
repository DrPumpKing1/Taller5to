using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PetLearningVFXHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private VisualEffect learningVisualEffect;

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

    private void InitializeLearningVFX()
    {
        learningVisualEffect.gameObject.SetActive(true);
        learningVisualEffect.Stop();
    }

    private void LearningPlatformLearn_OnStartLearning(object sender, System.EventArgs e)
    {
        learningVisualEffect.Play();
    }

    private void LearningPlatformLearn_OnEndLearning(object sender, System.EventArgs e)
    {
        learningVisualEffect.Stop();
    }
}
