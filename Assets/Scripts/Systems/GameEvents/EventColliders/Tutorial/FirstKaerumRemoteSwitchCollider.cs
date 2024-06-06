using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstKaerumRemoteSwitchEncounterCollider : EventCollider
{
    public static event EventHandler OnFirstKaerumRemoteSwitchEncounter;

    protected override void TriggerCollider()
    {
        OnFirstKaerumRemoteSwitchEncounter?.Invoke(this, EventArgs.Empty);
    }
}

