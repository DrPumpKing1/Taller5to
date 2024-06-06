using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstKaerumSwitchEncounterCollider : EventCollider
{
    public static event EventHandler OnFirstKaerumSwitchEncounter;

    protected override void TriggerCollider()
    {
        OnFirstKaerumSwitchEncounter?.Invoke(this, EventArgs.Empty);
    }
}

