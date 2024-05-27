using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningInstruction : Instruction
{
    [Header("Components")]
    [SerializeField] private LearningPlatformLearn learningPlatformLearn;

    [Header("Settings")]
    [SerializeField] private bool triggerOnAnyLearning;

    private void OnEnable()
    {
        LearningPlatformLearn.OnAnyObjectLearned += LearningPlatformLearnCrafting_OnAnyObjectLearned; ;
    }
    private void OnDisable()
    {
        LearningPlatformLearn.OnAnyObjectLearned -= LearningPlatformLearnCrafting_OnAnyObjectLearned; 
    }

    #region LerningPlatformLearn Subscriptions
    private void LearningPlatformLearnCrafting_OnAnyObjectLearned(object sender, LearningPlatformLearn.OnAnyObjectLearnedEventArgs e)
    {
        if (!triggerOnAnyLearning && e.learningPlatformLearn != learningPlatformLearn) return;
        HandleInstructionTrigger();
    }
    #endregion
}

