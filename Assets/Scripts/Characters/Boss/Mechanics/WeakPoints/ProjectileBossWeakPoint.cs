using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBossWeakPoint : BossWeakPoint
{
    private const float POWERED_TIME_THRESHOLD = 5f;
    private const float POWERED_TIME_PER_PROJECTILE = 4.5f;

    public float poweredAccumulatedTimer;

    private void Start()
    {
        ResetTimer();
    }

    protected override void HandleWeakPointPower()
    {
        if (!WeakPointEnabled) return;
        if (WeakPointHit) return;

        if(poweredAccumulatedTimer >= POWERED_TIME_THRESHOLD)
        {
            HitWeakPoint();
        }

        if (poweredAccumulatedTimer > 0f) poweredAccumulatedTimer -= Time.deltaTime;
        if (poweredAccumulatedTimer < 0f) ResetTimer();
    }

    private void PowerWeakPoint()
    {
        if (!WeakPointEnabled) return;
        poweredAccumulatedTimer += POWERED_TIME_PER_PROJECTILE;
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
