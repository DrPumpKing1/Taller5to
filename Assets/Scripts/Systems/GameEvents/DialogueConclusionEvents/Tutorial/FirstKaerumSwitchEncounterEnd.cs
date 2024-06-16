using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstKaerumSwitchEncounterEnd : DialogueConclusionEvent
{
    public static event EventHandler OnFirstKaerumSwitchEncounterEnd;
    protected override void TriggerEvent()
    {
        OnFirstKaerumSwitchEncounterEnd?.Invoke(this, EventArgs.Empty);
    }
}
