using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstCableRotationEncounterCollider : ConditionalEventCollider
{
    public static event EventHandler OnFirstCableRotationEncounter;

    protected override bool MeetsCondition()
    {
        return ProjectionGemsManager.Instance.AvailableProjectionGems >= 1;
    }

    protected override void TriggerCollider()
    {
        OnFirstCableRotationEncounter?.Invoke(this, EventArgs.Empty);
    }
}