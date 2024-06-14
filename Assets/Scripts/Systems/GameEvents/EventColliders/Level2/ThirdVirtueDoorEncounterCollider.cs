using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThirdVirtueDoorEncounterCollider : EventCollider
{
    public static event EventHandler OnThirdVirtueDoorEncounter;

    protected override void TriggerCollider()
    {
        OnThirdVirtueDoorEncounter?.Invoke(this, EventArgs.Empty);
    }
}