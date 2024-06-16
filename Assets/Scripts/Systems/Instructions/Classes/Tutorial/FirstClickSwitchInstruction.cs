using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstClickSwitchInstruction : LogAcomplishedInstruction
{
    protected override void OnEnable()
    {
        base.OnEnable();
        FirstKaerumSwitchEncounterEnd.OnFirstKaerumSwitchEncounterEnd += FirstKaerumSwitchEncounterEnd_OnFirstKaerumSwitchEncounterEnd;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        FirstKaerumSwitchEncounterEnd.OnFirstKaerumSwitchEncounterEnd -= FirstKaerumSwitchEncounterEnd_OnFirstKaerumSwitchEncounterEnd;
    }

    private void FirstKaerumSwitchEncounterEnd_OnFirstKaerumSwitchEncounterEnd(object sender, System.EventArgs e)
    {
        CheckShouldShow();
    }
}
