using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhaseHandler : MonoBehaviour
{
    public static BossPhaseHandler Instance;

    [Header("Phases")]
    [SerializeField] BossPhase currentPhase;

    [Header("Booleans")]
    [SerializeField] private bool isDefeated;

    private const BossPhase FIRST_PHASE = BossPhase.Phase0;
    private const BossPhase LAST_PHASE = BossPhase.Phase3;
    private const BossPhase ALMOST_DEFEATED_PHASE = BossPhase.AlmostDefeated;
    private const BossPhase DEFEATED_PHASE = BossPhase.Defeated;

    public static event EventHandler<OnPhaseEventArgs> OnPhaseCompleated;
    public static event EventHandler OnLastPhaseCompleated;
    public static event EventHandler OnAlmostDefeatedPhaseCompleated;

    public class OnPhaseEventArgs : EventArgs
    {
        public BossPhase currentPhase;
        public BossPhase nextPhase;
    }

    private void OnEnable()
    {
        BossStateHandler.OnBossPhaseChangeMidA += BossStateHandler_OnBossPhaseChangeMidA;
        BossStateHandler.OnBossDefeated += BossStateHandler_OnBossDefeated;

        BossWeakPointsHandler.OnPhaseWeakPointsHit += BossWeakPointsHandler_OnPhaseWeakPointsHit;
    }

    private void OnDisable()
    {
        BossStateHandler.OnBossPhaseChangeMidA -= BossStateHandler_OnBossPhaseChangeMidA;
        BossStateHandler.OnBossDefeated -= BossStateHandler_OnBossDefeated;

        BossWeakPointsHandler.OnPhaseWeakPointsHit -= BossWeakPointsHandler_OnPhaseWeakPointsHit;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        SetDefeated(false);
        SetCurrentPhase(FIRST_PHASE);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ChangeToNextPhase();
        }
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one BossPhaseHandler, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void SetCurrentPhase(BossPhase phase) => currentPhase = phase;
    private BossPhase GetNextPhase(BossPhase currentPhase)
    {
        switch (currentPhase)
        {
            case BossPhase.Phase0:
                return BossPhase.Phase1;
            case BossPhase.Phase1:
                return BossPhase.Phase2;
            case BossPhase.Phase2:
                return BossPhase.Phase3;
            case BossPhase.Phase3:
            default:
                return BossPhase.Defeated;
        }
    }

    private void ChangeToNextPhase()
    {
        if (BossStateHandler.Instance.BossState == BossStateHandler.State.PhaseChange) return;
        if (BossStateHandler.Instance.BossState == BossStateHandler.State.Defeated) return;
        if (isDefeated) return;

        if(currentPhase == ALMOST_DEFEATED_PHASE)
        {
            OnAlmostDefeatedPhaseCompleated?.Invoke(this, EventArgs.Empty);
            SetCurrentPhase(DEFEATED_PHASE);
            return;
        }

        if (currentPhase == LAST_PHASE)
        {
            OnLastPhaseCompleated?.Invoke(this, EventArgs.Empty);
            SetCurrentPhase(ALMOST_DEFEATED_PHASE);
            return;
        }

        BossPhase nextPhase = GetNextPhase(currentPhase);
        OnPhaseCompleated?.Invoke(this, new OnPhaseEventArgs {currentPhase = currentPhase, nextPhase = nextPhase });
    }

    private void SetDefeated(bool defeated) => isDefeated = defeated;


    #region BossStateHandler Subscriptions
    private void BossStateHandler_OnBossPhaseChangeMidA(object sender, BossStateHandler.OnPhaseChangeEventArgs e)
    {
        SetCurrentPhase(e.nextPhase);
    }
    private void BossStateHandler_OnBossDefeated(object sender, EventArgs e) => SetDefeated(true);
    #endregion

    #region BossWeakPointsHandler Subscriptions
    private void BossWeakPointsHandler_OnPhaseWeakPointsHit(object sender, BossWeakPointsHandler.OnPhaseWeakPointsHitEventArgs e)
    {
        if (currentPhase != e.phaseWeakPoints.bossPhase) return;
        ChangeToNextPhase();
    }
    #endregion
}
