using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstLearningPlatformEncounterEnd : DialogueConclusionEvent
{
    public static event EventHandler OnFirstLearningPlatformEncounterEnd;

    protected override void TriggerEvent()
    {
        OnFirstLearningPlatformEncounterEnd?.Invoke(this, EventArgs.Empty);
    }
}
