using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstCableRotationEncounterEnd : DialogueConclusionEvent
{
    public static event EventHandler OnFirstCableRotationEncounterEnd;

    protected override void TriggerEvent()
    {
        OnFirstCableRotationEncounterEnd?.Invoke(this, EventArgs.Empty);
    }
}
