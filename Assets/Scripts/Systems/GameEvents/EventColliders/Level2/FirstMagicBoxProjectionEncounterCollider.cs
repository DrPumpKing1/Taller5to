using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstMagicBoxProjectionEncounterCollider : EventCollider
{
    public static event EventHandler OnFirstMagicBoxProjectionEncounter;

    protected override void TriggerCollider()
    {
        OnFirstMagicBoxProjectionEncounter?.Invoke(this, EventArgs.Empty);
    }
}
