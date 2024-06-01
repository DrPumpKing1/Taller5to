using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MeetVyrxCollider : EventCollider
{
    public static event EventHandler OnMeetVyrx;

    protected override void TriggerCollider()
    {
        OnMeetVyrx?.Invoke(this, EventArgs.Empty);
    }
}
