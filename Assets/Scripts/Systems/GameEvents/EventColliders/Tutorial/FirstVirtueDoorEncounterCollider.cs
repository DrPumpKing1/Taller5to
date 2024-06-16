using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstVirtueDoorEncounterCollider : ConditionalEventCollider
{
    public static event EventHandler OnFirstVirtueDoorEncounter;

    protected override bool MeetsCondition()
    {
        return !ShieldPiecesManager.Instance.HasCompletedShield(Dialect.Zurryth);
    }

    protected override void TriggerCollider()
    {
        OnFirstVirtueDoorEncounter?.Invoke(this, EventArgs.Empty);
    }
}

