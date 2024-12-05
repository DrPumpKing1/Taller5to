using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstWeakpointsEncounterCollider : ConditionalEventCollider
{
    protected override bool MeetsCondition()
    {
        return ShowcaseRoomPhaseHandler.Instance.CurrentPhase == ShowcaseRoomPhase.Phase1;
    }
}
