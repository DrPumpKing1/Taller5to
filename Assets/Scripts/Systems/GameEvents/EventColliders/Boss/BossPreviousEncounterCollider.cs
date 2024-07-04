using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossPreviousEncounterCollider : EventCollider
{
    public static event EventHandler OnBossPreviousEncounter;

    protected override void TriggerCollider()
    {
        OnBossPreviousEncounter?.Invoke(this, EventArgs.Empty);
    }
}
