using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTheSwitchInstruction : LogAcomplishedInstruction
{
    protected override void OnEnable()
    {
        base.OnEnable();
        FirstKaerumRemoteSwitchEncounterEnd.OnFirstKaerumRemoteSwitchEncounterEnd += FirstKaerumRemoteSwitchEncounterEnd_OnFirstKaerumRemoteSwitchEncounterEnd;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        FirstKaerumRemoteSwitchEncounterEnd.OnFirstKaerumRemoteSwitchEncounterEnd -= FirstKaerumRemoteSwitchEncounterEnd_OnFirstKaerumRemoteSwitchEncounterEnd;
    }

    private void FirstKaerumRemoteSwitchEncounterEnd_OnFirstKaerumRemoteSwitchEncounterEnd(object sender, System.EventArgs e)
    {
        CheckShouldShow();
    }
}
