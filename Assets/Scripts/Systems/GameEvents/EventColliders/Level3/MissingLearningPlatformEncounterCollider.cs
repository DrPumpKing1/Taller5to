using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MissingLearningPlatformEncounterCollider : EventCollider
{
    public static event EventHandler OnMissingLearningPlatformEncounter;

    protected override void TriggerCollider()
    {
        OnMissingLearningPlatformEncounter?.Invoke(this, EventArgs.Empty);
    }
}
