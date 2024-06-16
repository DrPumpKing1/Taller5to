using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstBacktrackSwitchEncounterEnd : DialogueConclusionEvent
{
    public static event EventHandler OnFirstBacktrackSwitchEncounterEnd;

    protected override void TriggerEvent()
    {
        OnFirstBacktrackSwitchEncounterEnd?.Invoke(this, EventArgs.Empty);
    }
}
