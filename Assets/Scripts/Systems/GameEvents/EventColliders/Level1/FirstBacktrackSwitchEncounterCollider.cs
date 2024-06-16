using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstBacktrackSwitchEncounterCollider : EventCollider
{
    public static event EventHandler OnFirstBacktrackSwitchEncounter;

    protected override void TriggerCollider()
    {
        OnFirstBacktrackSwitchEncounter?.Invoke(this, EventArgs.Empty);
    }
}