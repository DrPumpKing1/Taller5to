using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCableInstruction : LogAcomplishedInstruction
{
    protected override void OnEnable()
    {
        base.OnEnable();
        FirstCableRotationEncounterEnd.OnFirstCableRotationEncounterEnd += FirstCableRotationEncounterEnd_OnFirstCableRotationEncounterEnd;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        FirstCableRotationEncounterEnd.OnFirstCableRotationEncounterEnd -= FirstCableRotationEncounterEnd_OnFirstCableRotationEncounterEnd;
    }

    private void FirstCableRotationEncounterEnd_OnFirstCableRotationEncounterEnd(object sender, System.EventArgs e)
    {
        CheckShouldShow();
    }

}