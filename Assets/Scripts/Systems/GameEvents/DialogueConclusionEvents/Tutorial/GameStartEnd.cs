using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameStartEnd : DialogueConclusionEvent
{
    public static event EventHandler OnGameStartEnd;

    protected override void TriggerEvent()
    {
        OnGameStartEnd?.Invoke(this, EventArgs.Empty);
    }
}
