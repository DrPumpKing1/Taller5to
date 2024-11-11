using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShowcaseRoomWeakpoint : ShowcaseRoomWeakpoint
{
    [Header("Projectile Weakpoint Specifics")]
    [SerializeField] private float poweredTimeThreshold = 1f;
    [SerializeField] private float poweredTimePerProjectile = 5f;
    [SerializeField] private float timerClamp = 6f;

    private float poweredAccumulatedTimer;
    private bool previousPowered;

    protected override void Start()
    {
        base.Start();
        SetPreviouslypowered(false);
        ResetTimer();
    }

    protected override void HandleWeakPointPower()
    {
        if (!CheckPlayerClose()) return;

        if (poweredAccumulatedTimer >= poweredTimeThreshold && IsEnabled)
        {
            if (!previousPowered)
            {
                SetIsHit(true, true);
            }

            previousPowered = true;
        }

        if (poweredAccumulatedTimer < poweredTimeThreshold)
        {
            if (previousPowered)
            {
                SetIsHit(false, true);
            }

            previousPowered = false;
        }

        if (poweredAccumulatedTimer > timerClamp) poweredAccumulatedTimer = timerClamp;
        if (poweredAccumulatedTimer > 0f) poweredAccumulatedTimer -= Time.deltaTime;
        if (poweredAccumulatedTimer < 0f) ResetTimer();
    }

    private void PowerWeakPoint()
    {
        if (!IsEnabled) return;
        poweredAccumulatedTimer += poweredTimePerProjectile;
    }

    private bool SetPreviouslypowered(bool powered) => previousPowered = powered;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out SignalProjectile signalProjectile))
        {
            PowerWeakPoint();
        }
    }

    private void ResetTimer() => poweredAccumulatedTimer = 0;
}
