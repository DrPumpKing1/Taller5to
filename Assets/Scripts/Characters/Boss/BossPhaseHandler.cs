using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhaseHandler : MonoBehaviour
{
    public static BossPhaseHandler Instance;

    [Header("Phases")]
    [SerializeField] private int phaseNumber;
    [SerializeField] private int lastPhase;

    [Header("Invulnerability")]
    public bool isInvulnerable;

    public static event EventHandler OnFirstPhaseStart;
    public static event EventHandler<OnPhaseChangeEventArgs> OnPhaseChange;
    public static event EventHandler OnLastPhaseEnded;

    public class OnPhaseChangeEventArgs : EventArgs
    {
        public int newPhase;
    }

    private void OnEnable()
    {
        BossStateHandler.OnBossActiveStart += BossStateHandler_OnBossActiveStart;
        BossStateHandler.OnBossActiveEnd += BossStateHandler_OnBossActiveEnd;
        BossStateHandler.OnBossPhaseChangeStart += BossStateHandler_OnBossPhaseChangeStart;
        BossStateHandler.OnBossPhaseChangeEnd += BossStateHandler_OnBossPhaseChangeEnd;

        BossKaerumOvercharge.OnBossHit += BossKaerumOvercharge_OnBossHit;
        BossKaerumOvercharge.OnBossOvercharge += BossKaerumOvercharge_OnBossOvercharge;
    }

    private void OnDisable()
    {
        BossStateHandler.OnBossActiveStart -= BossStateHandler_OnBossActiveStart;
        BossStateHandler.OnBossActiveEnd -= BossStateHandler_OnBossActiveEnd;
        BossStateHandler.OnBossPhaseChangeStart -= BossStateHandler_OnBossPhaseChangeStart;
        BossStateHandler.OnBossPhaseChangeEnd -= BossStateHandler_OnBossPhaseChangeEnd;

        BossKaerumOvercharge.OnBossHit -= BossKaerumOvercharge_OnBossHit;
        BossKaerumOvercharge.OnBossOvercharge -= BossKaerumOvercharge_OnBossOvercharge;
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
        isInvulnerable = true;
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

    #region BossStateHandler Subscriptions
    private void BossStateHandler_OnBossActiveStart(object sender, EventArgs e) => SetInvulverability(true);
    private void BossStateHandler_OnBossActiveEnd(object sender, EventArgs e) => SetInvulverability(false);
    private void BossStateHandler_OnBossPhaseChangeStart(object sender, EventArgs e) => SetInvulverability(true);
    private void BossStateHandler_OnBossPhaseChangeEnd(object sender, EventArgs e) => SetInvulverability(false);
    #endregion

    #region BossKaerumOvercharge Subscriptions
    private void BossKaerumOvercharge_OnBossHit(object sender, BossKaerumOvercharge.OnBossHitEventArgs e)
    {
        CheckFirstPhaseStart();
    }

    private void BossKaerumOvercharge_OnBossOvercharge(object sender, EventArgs e)
    {
        CheckChangePhase();
    }
    #endregion
}
