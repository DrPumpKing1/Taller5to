using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstVirtueDoorEncounterCollider : EventCollider
{
    public static event EventHandler OnFirstVirtueDoorEncounter;

    protected override void TriggerCollider()
    {
        OnFirstVirtueDoorEncounter?.Invoke(this, EventArgs.Empty);
    }
}

