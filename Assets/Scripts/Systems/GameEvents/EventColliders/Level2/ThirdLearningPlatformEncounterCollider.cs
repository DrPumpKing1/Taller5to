using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThirdLearningPlatformEncounterCollider : EventCollider
{
    public static event EventHandler OnThirdLearningPlatformEncounter;

    protected override void TriggerCollider()
    {
        OnThirdLearningPlatformEncounter?.Invoke(this, EventArgs.Empty);
    }
}