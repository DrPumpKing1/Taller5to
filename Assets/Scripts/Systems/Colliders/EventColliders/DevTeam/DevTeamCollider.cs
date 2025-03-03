using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevTeamCollider : ConditionalEventCollider
{
    [Header("Dev Settings")]
    [SerializeField] private bool devTeamEnabled;

    protected override bool MeetsCondition()
    {
        return (devTeamEnabled);
    }
}
