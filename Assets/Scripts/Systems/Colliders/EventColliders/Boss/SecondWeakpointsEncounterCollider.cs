using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondWeakpointsEncounterCollider : ConditionalEventCollider
{
    protected override bool MeetsCondition()
    {
        return ShowcaseRoomPhaseHandler.Instance.CurrentPhase == ShowcaseRoomPhase.Phase2;
    }
}
