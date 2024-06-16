using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstKaerumRemoteSwitchEncounterEnd : DialogueConclusionEvent
{
    public static event EventHandler OnFirstKaerumRemoteSwitchEncounterEnd;

    protected override void TriggerEvent()
    {
        OnFirstKaerumRemoteSwitchEncounterEnd?.Invoke(this, EventArgs.Empty);
    }
}
