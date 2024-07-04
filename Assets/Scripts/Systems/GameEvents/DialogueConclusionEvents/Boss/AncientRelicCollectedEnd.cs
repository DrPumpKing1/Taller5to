using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AncientRelicCollectedEnd : DialogueConclusionEvent
{
    public static event EventHandler OnAncientRelicCollectedEnd;

    protected override void TriggerEvent()
    {
        OnAncientRelicCollectedEnd?.Invoke(this, EventArgs.Empty);
    }
}
