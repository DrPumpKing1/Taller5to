using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SecondKaerumEncounterCollider : EventCollider
{
    public static event EventHandler OnSecondKaerumEncounter;

    protected override void TriggerCollider()
    {
        OnSecondKaerumEncounter?.Invoke(this, EventArgs.Empty);
    }
}
