using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstCableRotationEncounterCollider : EventCollider
{
    public static event EventHandler OnFirstCableRotationEncounter;

    protected override void TriggerCollider()
    {
        OnFirstCableRotationEncounter?.Invoke(this, EventArgs.Empty);
    }
}