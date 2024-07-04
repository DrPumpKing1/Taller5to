using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FourthLearningPlatformEncounterCollider : EventCollider
{
    public static event EventHandler OnFourthLearningPlatformEncounter;

    protected override void TriggerCollider()
    {
        OnFourthLearningPlatformEncounter?.Invoke(this, EventArgs.Empty);
    }
}

