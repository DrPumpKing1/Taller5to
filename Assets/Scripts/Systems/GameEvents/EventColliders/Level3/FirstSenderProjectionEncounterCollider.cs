using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstSenderProjectionEncounterCollider : EventCollider
{
    public static event EventHandler OnFirstSenderProjectionEncounter;

    protected override void TriggerCollider()
    {
        OnFirstSenderProjectionEncounter?.Invoke(this, EventArgs.Empty);
    }
}
