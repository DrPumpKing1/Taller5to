using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectMagicBoxInstruction : LogAcomplishedInstruction
{
    protected override void OnEnable()
    {
        base.OnEnable();
        FirstMagicBoxProjectionEncounterEnd.OnFirstMagicBoxProjectionEncounterEnd += FirstMagicBoxProjectionEncounterEnd_OnFirstMagicBoxProjectionEncounterEnd; ;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        FirstMagicBoxProjectionEncounterEnd.OnFirstMagicBoxProjectionEncounterEnd += FirstMagicBoxProjectionEncounterEnd_OnFirstMagicBoxProjectionEncounterEnd; ;
    }

    private void FirstMagicBoxProjectionEncounterEnd_OnFirstMagicBoxProjectionEncounterEnd(object sender, System.EventArgs e)
    {
        CheckShouldShow();
    }
}