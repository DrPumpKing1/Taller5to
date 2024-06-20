using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossKaerumOvercharge : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int hitsPerPhase;

    [SerializeField] private int currentHitsInPhase;



    public static event EventHandler OnBossOvercharge;
    public static event EventHandler<OnBossHitEventArgs> OnBossHit;

    public class OnBossHitEventArgs : EventArgs
    {
        public bool isInvulnerable;
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        currentHitsInPhase = 0;
    }

    private void CheckHit()
    {
        OnBossHit?.Invoke(this, new OnBossHitEventArgs { isInvulnerable = BossPhaseHandler.Instance.isInvulnerable });

        if (!BossPhaseHandler.Instance.isInvulnerable) currentHitsInPhase++;
        CheckOvercharge();
    }

    private void CheckOvercharge()
    {
        if (currentHitsInPhase >= hitsPerPhase)
        {
            OnBossOvercharge?.Invoke(this, EventArgs.Empty);
            ResetCurrentHitsInPhase();
        }
    }

    private void ResetCurrentHitsInPhase() => currentHitsInPhase = 0;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out SignalProjectile signalProjectile))
        {
            CheckHit();
        }
    }
}
