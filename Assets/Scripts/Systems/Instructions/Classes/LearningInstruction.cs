using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningInstruction : Instruction
{
    [Header("Components")]
    [SerializeField] private LearningPlatformLearnCrafting learningPlatformLearnCrafting;

    [Header("Settings")]
    [SerializeField] private bool triggerOnAnyLearning;

    private void OnEnable()
    {
        LearningPlatformLearnCrafting.OnAnyObjectLearned += LearningPlatformLearnCrafting_OnAnyObjectLearned; ;
    }
    private void OnDisable()
    {
        LearningPlatformLearnCrafting.OnAnyObjectLearned -= LearningPlatformLearnCrafting_OnAnyObjectLearned; 
    }

    #region LerningPlatformLearn Subscriptions
    private void LearningPlatformLearnCrafting_OnAnyObjectLearned(object sender, LearningPlatformLearnCrafting.OnAnyObjectLearnedEventArgs e)
    {
        if (!triggerOnAnyLearning && e.learningPlatformLearnCrafting != learningPlatformLearnCrafting) return;
        HandleInstructionTrigger();
    }
    #endregion
}

