using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstShieldPieceGrabEnd : DialogueConclusionEvent
{
    public static event EventHandler OnFirstShieldPieceGrabEnd;

    protected override void TriggerEvent()
    {
        OnFirstShieldPieceGrabEnd?.Invoke(this, EventArgs.Empty);
    }
}
