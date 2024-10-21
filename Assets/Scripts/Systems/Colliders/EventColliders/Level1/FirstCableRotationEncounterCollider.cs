using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstCableRotationEncounterCollider : ConditionalEventCollider
{
    protected override bool MeetsCondition()
    {
        return ProjectionGemsManager.Instance.AvailableProjectionGems >= 1;
    }
}