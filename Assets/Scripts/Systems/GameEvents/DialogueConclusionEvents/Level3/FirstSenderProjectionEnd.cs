using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstSenderProjectionEnd : DialogueConclusionEvent
{
    public static event EventHandler OnFirstSenderProjectionEnd;

    protected override void TriggerEvent()
    {
        OnFirstSenderProjectionEnd?.Invoke(this, EventArgs.Empty);
    }
}
