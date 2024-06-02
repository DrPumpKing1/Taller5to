using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstLearningPlatformEncounterCollider : EventCollider
{
    public static event EventHandler OnFirstLearningPlatformEncounter;

    protected override void TriggerCollider()
    {
        OnFirstLearningPlatformEncounter?.Invoke(this, EventArgs.Empty);
    }
}

