using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectCableInstruction : LogAcomplishedInstruction
{
    protected override void OnEnable()
    {
        base.OnEnable();
        FirstProjectionPlatformEncounterEnd.OnFirstProjectionPlatformEncounterEnd += FirstProjectionPlatformEncounterEnd_OnFirstProjectionPlatformEncounterEnd; ;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        FirstProjectionPlatformEncounterEnd.OnFirstProjectionPlatformEncounterEnd += FirstProjectionPlatformEncounterEnd_OnFirstProjectionPlatformEncounterEnd; ;
    }

    private void FirstProjectionPlatformEncounterEnd_OnFirstProjectionPlatformEncounterEnd(object sender, System.EventArgs e)
    {
        CheckShouldShow();
    }
}