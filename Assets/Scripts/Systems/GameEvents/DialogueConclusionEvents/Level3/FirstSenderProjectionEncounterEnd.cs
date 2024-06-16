using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstSenderProjectionEncounterEnd : DialogueConclusionEvent
{
    public static event EventHandler OnFirstSenderProjectionEncounterEnd;

    protected override void TriggerEvent()
    {
        OnFirstSenderProjectionEncounterEnd?.Invoke(this, EventArgs.Empty);
    }
}
