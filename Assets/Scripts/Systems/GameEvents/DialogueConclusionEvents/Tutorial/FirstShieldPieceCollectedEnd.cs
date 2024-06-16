using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstShieldPieceCollectedEnd : DialogueConclusionEvent
{
    public static event EventHandler OnFirstShieldPieceCollectedEnd;

    protected override void TriggerEvent()
    {
        OnFirstShieldPieceCollectedEnd?.Invoke(this, EventArgs.Empty);
    }
}
