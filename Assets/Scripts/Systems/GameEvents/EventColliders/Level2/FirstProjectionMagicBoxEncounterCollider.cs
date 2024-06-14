using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstProjectionMagicBoxEncounterCollider : EventCollider
{
    public static event EventHandler OnFirstProjectionMagicBoxEncounter;

    protected override void TriggerCollider()
    {
        OnFirstProjectionMagicBoxEncounter?.Invoke(this, EventArgs.Empty);
    }
}
