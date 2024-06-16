using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DematerializeCableInstruction : LogAcomplishedInstruction
{
    protected override void OnEnable()
    {
        base.OnEnable();
        FirstBacktrackSwitchTurnOnEnd.OnFirstBacktrackSwitchTurnOnEnd += FirstBacktrackSwitchTurnOnEnd_OnFirstBacktrackSwitchTurnOnEnd;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        FirstBacktrackSwitchTurnOnEnd.OnFirstBacktrackSwitchTurnOnEnd -= FirstBacktrackSwitchTurnOnEnd_OnFirstBacktrackSwitchTurnOnEnd;
    }

    private void FirstBacktrackSwitchTurnOnEnd_OnFirstBacktrackSwitchTurnOnEnd(object sender, System.EventArgs e)
    {
        CheckShouldShow();
    }
}