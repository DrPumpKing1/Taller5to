using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstMagicBoxProjectionEnd : DialogueConclusionEvent
{
    public static event EventHandler OnFirstMagicBoxProjectionEnd;

    protected override void TriggerEvent()
    {
        OnFirstMagicBoxProjectionEnd?.Invoke(this, EventArgs.Empty);
    }
}
