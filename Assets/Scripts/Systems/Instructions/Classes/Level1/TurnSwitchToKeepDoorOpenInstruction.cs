using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSwitchToKeepDoorOpenInstruction : LogAcomplishedInstruction
{
    protected override void OnEnable()
    {
        base.OnEnable();
        FirstBacktrackSwitchEncounterEnd.OnFirstBacktrackSwitchEncounterEnd += FirstBacktrackSwitchEncounterEnd_OnFirstBacktrackSwitchEncounterEnd; ;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        FirstBacktrackSwitchEncounterEnd.OnFirstBacktrackSwitchEncounterEnd -= FirstBacktrackSwitchEncounterEnd_OnFirstBacktrackSwitchEncounterEnd; ;
    }

    private void FirstBacktrackSwitchEncounterEnd_OnFirstBacktrackSwitchEncounterEnd(object sender, System.EventArgs e)
    {
        CheckShouldShow();
    }
}