using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstInscriptionPoweringEnd : DialogueConclusionEvent
{
    public static event EventHandler OnFirstInscriptionPoweringEnd;

    protected override void TriggerEvent()
    {
        OnFirstInscriptionPoweringEnd?.Invoke(this, EventArgs.Empty);
    }
}
