using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldClickToLearnObjectInstruction : LogAcomplishedInstruction
{
    protected override void OnEnable()
    {
        base.OnEnable();
        FirstLearningPlatformEncounterEnd.OnFirstLearningPlatformEncounterEnd += FirstLearningPlatformEncounterEnd_OnFirstLearningPlatformEncounterEnd; ;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        FirstLearningPlatformEncounterEnd.OnFirstLearningPlatformEncounterEnd -= FirstLearningPlatformEncounterEnd_OnFirstLearningPlatformEncounterEnd; ;
    }

    private void FirstLearningPlatformEncounterEnd_OnFirstLearningPlatformEncounterEnd(object sender, System.EventArgs e)
    {
        CheckShouldShow();
    }
}
