using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DematerializeObjectsInstruction : LogAcomplishedInstruction
{
    protected override void OnEnable()
    {
        base.OnEnable();
        FirstDematerializationTowerEncounterEnd.OnFirstDematerializationTowerEncounterEnd += FirstDematerializationTowerEncounterEnd_OnFirstDematerializationTowerEncounterEnd;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        FirstDematerializationTowerEncounterEnd.OnFirstDematerializationTowerEncounterEnd -= FirstDematerializationTowerEncounterEnd_OnFirstDematerializationTowerEncounterEnd;
    }

    private void FirstDematerializationTowerEncounterEnd_OnFirstDematerializationTowerEncounterEnd(object sender, System.EventArgs e)
    {
        CheckShouldShow();
    }
}