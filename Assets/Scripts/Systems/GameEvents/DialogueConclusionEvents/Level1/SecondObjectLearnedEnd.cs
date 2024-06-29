using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SecondObjectLearnedEnd : DialogueConclusionEvent
{
    public static event EventHandler OnSecondObjectLearnedEnd;

    protected override void TriggerEvent()
    {
        OnSecondObjectLearnedEnd?.Invoke(this, EventArgs.Empty);
    }
}
