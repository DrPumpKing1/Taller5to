using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SecondVirtueDoorEncounterCollider : EventCollider
{
    public static event EventHandler OnSecondVirtueDoorEncounter;

    protected override void TriggerCollider()
    {
        OnSecondVirtueDoorEncounter?.Invoke(this, EventArgs.Empty);
    }
}