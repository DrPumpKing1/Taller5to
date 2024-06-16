using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectSenderInstruction : LogAcomplishedInstruction
{
    protected override void OnEnable()
    {
        base.OnEnable();
        FirstSenderProjectionEncounterEnd.OnFirstSenderProjectionEncounterEnd += FirstSenderProjectionEncounterEnd_OnFirstSenderProjectionEncounterEnd;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        FirstSenderProjectionEncounterEnd.OnFirstSenderProjectionEncounterEnd -= FirstSenderProjectionEncounterEnd_OnFirstSenderProjectionEncounterEnd;

    }

    private void FirstSenderProjectionEncounterEnd_OnFirstSenderProjectionEncounterEnd(object sender, System.EventArgs e)
    {
        CheckShouldShow();
    }
}