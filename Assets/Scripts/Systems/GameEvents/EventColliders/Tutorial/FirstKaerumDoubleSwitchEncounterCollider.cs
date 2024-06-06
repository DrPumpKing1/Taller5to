using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstKaerumDoubleSwitchEncounterCollider : EventCollider
{
    public static event EventHandler OnFirstKaerumDoubleSwitchEncounter;

    protected override void TriggerCollider()
    {
        OnFirstKaerumDoubleSwitchEncounter?.Invoke(this, EventArgs.Empty);
    }
}

