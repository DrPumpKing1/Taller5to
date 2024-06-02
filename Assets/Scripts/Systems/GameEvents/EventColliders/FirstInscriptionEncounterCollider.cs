using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstInscriptionEncounterCollider : EventCollider
{
    public static event EventHandler OnFirstInscriptionEncounter;

    protected override void TriggerCollider()
    {
        OnFirstInscriptionEncounter?.Invoke(this, EventArgs.Empty);
    }
}

