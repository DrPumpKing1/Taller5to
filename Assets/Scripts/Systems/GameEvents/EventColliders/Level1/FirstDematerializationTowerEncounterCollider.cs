using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstDematerializationTowerEncounterCollider : EventCollider
{
    public static event EventHandler OnFirstDematerializationTowenEncounter;

    protected override void TriggerCollider()
    {
        OnFirstDematerializationTowenEncounter?.Invoke(this, EventArgs.Empty);
    }
}