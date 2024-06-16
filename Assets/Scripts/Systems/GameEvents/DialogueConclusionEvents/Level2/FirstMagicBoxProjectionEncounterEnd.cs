using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstMagicBoxProjectionEncounterEnd : DialogueConclusionEvent
{
    public static event EventHandler OnFirstMagicBoxProjectionEncounterEnd;

    protected override void TriggerEvent()
    {
        OnFirstMagicBoxProjectionEncounterEnd?.Invoke(this, EventArgs.Empty);
    }
}
