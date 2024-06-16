using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerTheInscriptionInstruction : LogAcomplishedInstruction
{
    protected override void OnEnable()
    {
        base.OnEnable();
        FirstInscriptionEncounterEnd.OnFirstInscriptionEncounterEnd += FirstInscriptionEncounterEnd_OnFirstInscriptionEncounterEnd;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        FirstInscriptionEncounterEnd.OnFirstInscriptionEncounterEnd -= FirstInscriptionEncounterEnd_OnFirstInscriptionEncounterEnd;
    }

    private void FirstInscriptionEncounterEnd_OnFirstInscriptionEncounterEnd(object sender, System.EventArgs e)
    {
        CheckShouldShow();
    }
}
