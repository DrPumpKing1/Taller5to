using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstKaerumEncounterCollider : EventCollider
{
    public static event EventHandler OnFirstKaerumEncounter;

    protected override void TriggerCollider()
    {
        OnFirstKaerumEncounter?.Invoke(this, EventArgs.Empty);
    }
}
