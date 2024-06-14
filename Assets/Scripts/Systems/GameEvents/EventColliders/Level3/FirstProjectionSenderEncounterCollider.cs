using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstProjectionSenderEncounterCollider : EventCollider
{
    public static event EventHandler OnFirstProjectionSenderEncounter;

    protected override void TriggerCollider()
    {
        OnFirstProjectionSenderEncounter?.Invoke(this, EventArgs.Empty);
    }
}
