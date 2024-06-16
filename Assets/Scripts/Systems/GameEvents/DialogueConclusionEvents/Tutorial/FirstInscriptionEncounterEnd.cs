using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstInscriptionEncounterEnd : DialogueConclusionEvent
{
    public static event EventHandler OnFirstInscriptionEncounterEnd;

    protected override void TriggerEvent()
    {
        OnFirstInscriptionEncounterEnd?.Invoke(this, EventArgs.Empty);
    }
}
