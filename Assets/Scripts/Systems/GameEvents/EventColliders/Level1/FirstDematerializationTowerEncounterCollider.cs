using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstDematerializationTowerEncounterCollider : EventCollider
{
    public static event EventHandler OnFirstDematerializationTowerEncounter;

    protected override void TriggerCollider()
    {
        OnFirstDematerializationTowerEncounter?.Invoke(this, EventArgs.Empty);
    }
}