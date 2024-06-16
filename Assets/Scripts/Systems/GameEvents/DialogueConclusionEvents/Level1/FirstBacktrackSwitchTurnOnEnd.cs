using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class FirstBacktrackSwitchTurnOnEnd : DialogueConclusionEvent
{
    public static event EventHandler OnFirstBacktrackSwitchTurnOnEnd;

    protected override void TriggerEvent()
    {
        OnFirstBacktrackSwitchTurnOnEnd?.Invoke(this, EventArgs.Empty);
    }
}
