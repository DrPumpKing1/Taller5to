using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConditionalEventCollider : EventCollider
{
    protected override void HandleColliderTrigger()
    {
        if (hasBeenTriggered) return;
        if (!MeetsCondition()) return;

        TriggerCollider();
        hasBeenTriggered = true;
    }

    protected abstract bool MeetsCondition();
}
