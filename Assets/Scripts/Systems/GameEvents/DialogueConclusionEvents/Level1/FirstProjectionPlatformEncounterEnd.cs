using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstProjectionPlatformEncounterEnd : DialogueConclusionEvent
{
    public static event EventHandler OnFirstProjectionPlatformEncounterEnd;

    protected override void TriggerEvent()
    {
        OnFirstProjectionPlatformEncounterEnd?.Invoke(this, EventArgs.Empty);
    }
}
