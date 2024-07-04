using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossEncounterCollider : EventCollider
{
    public static event EventHandler OnBossEncounter;

    protected override void TriggerCollider()
    {
        OnBossEncounter?.Invoke(this, EventArgs.Empty);
    }
}

