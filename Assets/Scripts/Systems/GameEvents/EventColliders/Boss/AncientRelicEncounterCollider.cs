using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AncientRelicEncounterCollider : EventCollider
{
    public static event EventHandler OnAncientRelicEncounter;

    protected override void TriggerCollider()
    {
        OnAncientRelicEncounter?.Invoke(this, EventArgs.Empty);
    }
}

