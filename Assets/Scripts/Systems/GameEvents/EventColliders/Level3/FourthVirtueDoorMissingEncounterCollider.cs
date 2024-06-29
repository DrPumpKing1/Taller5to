using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FourthVirtueDoorMissingEncounterCollider : EventCollider
{
    public static event EventHandler OnFourthVirtueDoorMissingEncounter;

    protected override void TriggerCollider()
    {
        OnFourthVirtueDoorMissingEncounter?.Invoke(this, EventArgs.Empty);
    }
}