using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityBossWeakPoint : BossWeakPoint
{
    [Header("Components")]
    [SerializeField] private Electrode controllingNode;

    private const float POWERED_TIME_THRESHOLD = 0.25f;
    private float poweredTimer;
    private bool previousPowered;

    private void Start()
    {
        SetPreviouslypowered(false);
        ResetTimer();
    }

    protected override void HandleWeakPointPower()
    {
        if (!WeakPointEnabled) return;
        if (WeakPointHit) return;

        if (!NodeEnergyed())
        {
            ResetTimer();
            SetPreviouslypowered(false);
        }
        else
        {
            poweredTimer += Time.deltaTime;

            if (poweredTimer >= POWERED_TIME_THRESHOLD && !previousPowered)
            {
                HitWeakPoint();
                SetPreviouslypowered(true);
            }
        }
    }

    private bool NodeEnergyed() => controllingNode.Power >= Electrode.ACTIVATION_THRESHOLD;
    private bool SetPreviouslypowered(bool powered) => previousPowered = powered;
    private void ResetTimer() => poweredTimer = 0f;
}
