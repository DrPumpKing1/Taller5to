using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningSFXManager : CustomSFXManager
{
    protected override void OnEnable()
    {
        base.OnEnable();
        LearningPlatformLearn.OnStartLearning += LearningPlatformLearn_OnStartLearning;
        LearningPlatformLearn.OnEndLearning += LearningPlatformLearn_OnEndLearning;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        LearningPlatformLearn.OnStartLearning -= LearningPlatformLearn_OnStartLearning;
        LearningPlatformLearn.OnEndLearning -= LearningPlatformLearn_OnEndLearning;
    }
    private void LearningPlatformLearn_OnStartLearning(object sender, LearningPlatformLearn.OnLearningEventArgs e)
    {
        ReplaceAudioClip(SFXPoolSO.startLearning);
    }

    private void LearningPlatformLearn_OnEndLearning(object sender, LearningPlatformLearn.OnLearningEventArgs e)
    {
        StopAudioSource();
    }

}
