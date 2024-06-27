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
    public static event EventHandler<OnCurrentHitsInPhaseChangedEventArgs> OnCurrentHitsInPhaseChanged;
    public static event EventHandler OnCurrentHitsInPhaseReset;

    public int CurrentHitsInPhase => currentHitsInPhase;

    public class OnBossHitEventArgs : EventArgs
    {
        public bool isInvulnerable;
    }

    public class OnCurrentHitsInPhaseChangedEventArgs: EventArgs
    {
        public int currentHitsInPhase;
        public int hitsPerPhase;
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
        if (BossPhaseHandler.Instance.isDefeated) return;

        OnBossHit?.Invoke(this, new OnBossHitEventArgs { isInvulnerable = BossPhaseHandler.Instance.isInvulnerable });

        if (!BossPhaseHandler.Instance.isInvulnerable)
        {
            currentHitsInPhase++;
            OnCurrentHitsInPhaseChanged?.Invoke(this, new OnCurrentHitsInPhaseChangedEventArgs { currentHitsInPhase = currentHitsInPhase, hitsPerPhase = hitsPerPhase });
        }

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

    private void ResetCurrentHitsInPhase()
    {
        currentHitsInPhase = 0;
        OnCurrentHitsInPhaseReset?.Invoke(this, EventArgs.Empty);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out SignalProjectile signalProjectile))
        {
            CheckHit();
        }
    }
}
