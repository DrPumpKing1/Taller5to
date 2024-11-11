using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityBossWeakPoint : BossWeakPoint
{
    [Header("Components")]
    [SerializeField] private CableElectrode controlingCable;

    private const float POWERED_TIME_THRESHOLD = 0.25f;
    private float poweredTimer;
    private bool previousPowered;

    protected override void Start()
    {
        base.Start();
        SetPreviouslypowered(false);
        ResetTimer();
    }

    protected override void HandleWeakPointPower()
    {
        if (!IsEnabled) return;
        if(!CheckPlayerClose()) return;

        if (!CableEnergyzed())
        {
            ResetTimer();
            SetIsHit(false,true);
            SetPreviouslypowered(false);
        }
        else
        {
            poweredTimer += Time.deltaTime;

            if (poweredTimer >= POWERED_TIME_THRESHOLD && !previousPowered)
            {
                SetIsHit(true,true);
                SetPreviouslypowered(true);
            }
        }
    }

    private bool CableEnergyzed() => controlingCable.Power >= Electrode.ACTIVATION_THRESHOLD;
    private bool SetPreviouslypowered(bool powered) => previousPowered = powered;
    private void ResetTimer() => poweredTimer = 0f;
}
