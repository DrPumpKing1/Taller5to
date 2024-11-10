using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShowcaseRoomWeakpoint : ShowcaseRoomWeakpoint
{
    [Header("Projectile Weakpoint Specifics")]
    [SerializeField] private float poweredTimeThreshold = 5f;
    [SerializeField] private float poweredTimePerProjectile = 4.5f;

    private float poweredAccumulatedTimer;

    protected override void Start()
    {
        base.Start();
        ResetTimer();
    }

    protected override void HandleWeakPointPower()
    {
        if (!IsEnabled) return;
        if (!CheckPlayerClose()) return;

        if (poweredAccumulatedTimer >= poweredTimeThreshold)
        {
            SetIsHit(true);
        }
        else
        {
            SetIsHit(false);
        }

        if (poweredAccumulatedTimer > 0f) poweredAccumulatedTimer -= Time.deltaTime;
        if (poweredAccumulatedTimer < 0f) ResetTimer();
    }

    private void PowerWeakPoint()
    {
        if (!IsEnabled) return;
        poweredAccumulatedTimer += poweredTimePerProjectile;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out SignalProjectile signalProjectile))
        {
            PowerWeakPoint();
        }
    }

    private void ResetTimer() => poweredAccumulatedTimer = 0;
}
