using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstDematerializationTowerEncounterEnd : DialogueConclusionEvent
{
    public static event EventHandler OnFirstDematerializationTowerEncounterEnd;

    protected override void TriggerEvent()
    {
        OnFirstDematerializationTowerEncounterEnd?.Invoke(this, EventArgs.Empty);
    }
}
