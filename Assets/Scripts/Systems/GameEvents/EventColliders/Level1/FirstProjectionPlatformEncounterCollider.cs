using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstProjectionPlatformEncounterCollider : EventCollider
{
    public static event EventHandler OnFirstProjectionPlatformEncounter;

    protected override void TriggerCollider()
    {
        OnFirstProjectionPlatformEncounter?.Invoke(this, EventArgs.Empty);
    }
}
