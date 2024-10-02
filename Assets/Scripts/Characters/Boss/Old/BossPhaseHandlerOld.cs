using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhaseHandlerOld : MonoBehaviour
{
    public static BossPhaseHandlerOld Instance;

    [Header("Phases")]
    [SerializeField] private int phaseNumber;
    [SerializeField] private int lastPhase;

    [Header("Booleans")]
    public bool isInvulnerable;
    public bool isDefeated;

    public static event EventHandler OnFirstPhaseStart;
    public static event EventHandler<OnPhaseChangeEventArgs> OnPhaseChange;
    public static event EventHandler OnLastPhaseEnded;

    public class OnPhaseChangeEventArgs : EventArgs
    {
        public int newPhase;
    }

    private void OnEnable()
    {
        BossStateHandlerOld.OnBossActiveStart += BossStateHandler_OnBossActiveStart;
        BossStateHandlerOld.OnBossActiveEnd += BossStateHandler_OnBossActiveEnd;
        BossStateHandlerOld.OnBossPhaseChangeStart += BossStateHandler_OnBossPhaseChangeStart;
        BossStateHandlerOld.OnBossPhaseChangeEnd += BossStateHandler_OnBossPhaseChangeEnd;

        BossStateHandlerOld.OnBossDefeated += BossStateHandler_OnBossDefeated;

        BossKaerumOverchargeOld.OnBossHit += BossKaerumOvercharge_OnBossHit;
        BossKaerumOverchargeOld.OnBossOvercharge += BossKaerumOvercharge_OnBossOvercharge;
    }


    private void OnDisable()
    {
        BossStateHandlerOld.OnBossActiveStart -= BossStateHandler_OnBossActiveStart;
        BossStateHandlerOld.OnBossActiveEnd -= BossStateHandler_OnBossActiveEnd;
        BossStateHandlerOld.OnBossPhaseChangeStart -= BossStateHandler_OnBossPhaseChangeStart;
        BossStateHandlerOld.OnBossPhaseChangeEnd -= BossStateHandler_OnBossPhaseChangeEnd;

        BossStateHandlerOld.OnBossDefeated -= BossStateHandler_OnBossDefeated;

        BossKaerumOverchargeOld.OnBossHit -= BossKaerumOvercharge_OnBossHit;
        BossKaerumOverchargeOld.OnBossOvercharge -= BossKaerumOvercharge_OnBossOvercharge;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        InitializeVariables();
    }
    
    private void InitializeVariables()
    {
        SetInvulverability(true);
        SetDefeated(false);
        phaseNumber = 0;
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

    private void CheckFirstPhaseStart()
    {
        if (phaseNumber != 0) return;

        OnFirstPhaseStart?.Invoke(this, EventArgs.Empty);
        phaseNumber++;
    }

    private void CheckChangePhase()
    {
        if(phaseNumber >= lastPhase)
        {
            OnLastPhaseEnded?.Invoke(this, EventArgs.Empty);
            return;
        }

        phaseNumber++;
        OnPhaseChange?.Invoke(this, new OnPhaseChangeEventArgs { newPhase = phaseNumber });
    }

    private void SetInvulverability(bool invulnerable) => isInvulnerable = invulnerable;
    private void SetDefeated(bool defeated) => isDefeated = defeated;

    #region BossStateHandler Subscriptions
    private void BossStateHandler_OnBossActiveStart(object sender, EventArgs e) => SetInvulverability(true);
    private void BossStateHandler_OnBossActiveEnd(object sender, EventArgs e) => SetInvulverability(false);
    private void BossStateHandler_OnBossPhaseChangeStart(object sender, EventArgs e) => SetInvulverability(true);
    private void BossStateHandler_OnBossPhaseChangeEnd(object sender, EventArgs e) => SetInvulverability(false);
    private void BossStateHandler_OnBossDefeated(object sender, EventArgs e) => SetDefeated(true);
    #endregion

    #region BossKaerumOvercharge Subscriptions
    private void BossKaerumOvercharge_OnBossHit(object sender, BossKaerumOverchargeOld.OnBossHitEventArgs e)
    {
        CheckFirstPhaseStart();
    }

    private void BossKaerumOvercharge_OnBossOvercharge(object sender, EventArgs e)
    {
        CheckChangePhase();
    }
    #endregion
}
