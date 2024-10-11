using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChamberOfCulturalPassagesCollider : ConditionalEventCollider
{
    protected override bool MeetsCondition() => CheckpointManager.Instance.CurrentCheckpointID <= 2;

}
