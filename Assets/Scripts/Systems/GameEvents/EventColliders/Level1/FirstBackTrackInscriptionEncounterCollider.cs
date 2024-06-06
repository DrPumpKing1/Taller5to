using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstBackTrackInscriptionEncounterCollider : EventCollider
{
    public static event EventHandler OnFirstBackTrackInscriptionEncounter;

    protected override void TriggerCollider()
    {
        OnFirstBackTrackInscriptionEncounter?.Invoke(this, EventArgs.Empty);
    }
}