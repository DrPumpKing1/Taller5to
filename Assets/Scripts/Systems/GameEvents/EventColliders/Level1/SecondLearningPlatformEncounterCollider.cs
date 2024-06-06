using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SecondLearningPlatformEncounterCollider : EventCollider
{
    public static event EventHandler OnSecondLearningPlatformEncounter;

    protected override void TriggerCollider()
    {
        OnSecondLearningPlatformEncounter?.Invoke(this, EventArgs.Empty);
    }
}