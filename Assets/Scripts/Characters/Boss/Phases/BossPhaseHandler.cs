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

    public static event EventHandler<OnPhaseChangeEventArgs> OnPhaseChange;
    public static event EventHandler OnLastPhaseEnded;

    public class OnPhaseChangeEventArgs : EventArgs
    {
        public BossPhase nextPhase;
    }

    private void OnEnable()
    {
        BossStateHandler.OnBossDefeated += BossStateHandler_OnBossDefeated;
    }

    private void OnDisable()
    {
        BossStateHandler.OnBossDefeated -= BossStateHandler_OnBossDefeated;
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

    private void CheckChangePhase()
    {
        if (currentPhase == LAST_PHASE)
        {
            OnLastPhaseEnded?.Invoke(this, EventArgs.Empty);
            return;
        }

        BossPhase nextPhase = GetNextPhase(currentPhase);
        SetCurrentPhase(nextPhase);
        OnPhaseChange?.Invoke(this, new OnPhaseChangeEventArgs { nextPhase = nextPhase });
    }

    private void SetDefeated(bool defeated) => isDefeated = defeated;


    #region BossStateHandler Subscriptions
    private void BossStateHandler_OnBossDefeated(object sender, EventArgs e) => SetDefeated(true);
    #endregion

}
