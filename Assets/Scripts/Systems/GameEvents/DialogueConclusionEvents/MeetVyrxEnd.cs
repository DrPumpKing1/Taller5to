using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MeetVyrxEnd : DialogueConclusionEvent
{
    public static event EventHandler OnMeetVyrxEnd;

    protected override void TriggerEvent()
    {
        OnMeetVyrxEnd?.Invoke(this, EventArgs.Empty);
    }
}
