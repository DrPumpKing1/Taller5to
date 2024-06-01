using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstObjectLearnedEnd : DialogueConclusionEvent
{
    public static event EventHandler OnFirstObjectLearnedEnd;

    protected override void TriggerEvent()
    {
        OnFirstObjectLearnedEnd?.Invoke(this, EventArgs.Empty);
    }
}

