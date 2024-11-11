using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityBossWeakPoint : BossWeakPoint
{
    [Header("Components")]
    [SerializeField] private CableElectrode controlingCable;

    private const float POWERED_TIME_THRESHOLD = 0.25f;
    private const float NOT_POWERED_TIME_THRESHOLD = 0.25f;
    private float poweredTimer;
    private float notPoweredTimer;
    private bool previousPowered;

    protected override void Start()
    {
        base.Start();
        SetPreviouslypowered(false);
        ResetTimer();
    }

    private void HandleWeakPointPowerInverted()
    {
        if (!IsEnabled) return;
        if (!CheckPlayerClose()) return;

        if (!CableEnergyzed())
        {
            ResetTimer();
            SetIsHit(false, true);
            SetPreviouslypowered(false);
        }
        else
        {
            poweredTimer += Time.deltaTime;

            if (poweredTimer >= POWERED_TIME_THRESHOLD && !previousPowered)
            {
                SetIsHit(true, true);
                SetPreviouslypowered(true);
            }
        }
    }

    protected override void HandleWeakPointPower()
    {
        if (!IsEnabled) return;
        if (!CheckPlayerClose()) return;

        if (!CableEnergyzed())
        {
            notPoweredTimer += Time.deltaTime;

            if (notPoweredTimer >= NOT_POWERED_TIME_THRESHOLD && previousPowered)
            {
                SetIsHit(false, true);
                SetPreviouslypowered(false);
            }
        }
        else
        {
            if (!previousPowered)
            {
                SetIsHit(true, true);
            }

            SetPreviouslypowered(true);
            ResetTimer();
        }
    }

    private bool CableEnergyzed() => controlingCable.Power >= Electrode.ACTIVATION_THRESHOLD;
    private bool SetPreviouslypowered(bool powered) => previousPowered = powered;
    private void ResetTimer() => poweredTimer = 0f;
}
