using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FourthVirtueDoorEncounterCollider : EventCollider
{
    public static event EventHandler OnFourthVirtueDoorEncounter;

    protected override void TriggerCollider()
    {
        OnFourthVirtueDoorEncounter?.Invoke(this, EventArgs.Empty);
    }
}