using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerDefeatedEnd : DialogueConclusionEvent
{
    public static event EventHandler OnPlayerDefeatedEnd;

    protected override void TriggerEvent()
    {
        OnPlayerDefeatedEnd?.Invoke(this, EventArgs.Empty);
    }
}
