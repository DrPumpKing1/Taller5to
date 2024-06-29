using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThirdObjectLearnedEnd : DialogueConclusionEvent
{
    public static event EventHandler OnThirdObjectLearnedEnd;

    protected override void TriggerEvent()
    {
        OnThirdObjectLearnedEnd?.Invoke(this, EventArgs.Empty);
    }
}
