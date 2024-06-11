using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConditionalEventCollider : EventCollider
{
    protected override void HandleColliderTrigger()
    {
        if (!MeetsCondition()) return;
        base.HandleColliderTrigger();
    }

    protected abstract bool MeetsCondition();
}
