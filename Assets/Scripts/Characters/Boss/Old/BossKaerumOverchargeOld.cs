using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossKaerumOverchargeOld : MonoBehaviour
{
    public static BossKaerumOverchargeOld Instance { get; private set; }

    [Header("ChargeSettings")]
    [SerializeField,Range(5f,100f)] private float chargeLimitPerPhase;
    [SerializeField,Range(1f,2f)] private float chargePerProjectile;
    [Space]
    [SerializeField] private float currentChargeInPhase;

    [Header("PassiveDischargeSettings")]
    [SerializeField, Range(5f, 30f)] private float timeForPassiveDischarge;
    [SerializeField, Range(0.1f, 3f)] private float passiveDischargePerSecond;

    [Header("Debug")]
    [SerializeField] private float timeNotHit;

    public static event EventHandler OnBossOvercharge;
    public static event EventHandler<OnBossHitEventArgs> OnBossHit;
    public static event EventHandler<OnCurrentChargeInPhaseChangedEventArgs> OnCurrentChargeInPhaseChanged;
    public static event EventHandler OnCurrentChargeInPhaseReset;

    public float CurrentChargeInPhase => currentChargeInPhase;
    public float ChargePerProjetile => chargePerProjectile;

    public class OnBossHitEventArgs : EventArgs
    {
        public bool isInvulnerable;
    }

    public class OnCurrentChargeInPhaseChangedEventArgs: EventArgs
    {
        public float currentChargeInPhase;
        public float chargeLimitPerPhase;
    }

    private void OnEnable()
    {
        BossStateHandlerOld.OnBossActiveStart += BossStateHandler_OnBossActiveStart;
        BossStateHandlerOld.OnBossPhaseChangeStart += BossStateHandler_OnBossPhaseChangeStart;
        BossStateHandlerOld.OnBossDefeated += BossStateHandler_OnBossDefeated;
        BossStateHandlerOld.OnPlayerDefeated += BossStateHandler_OnPlayerDefeated;
    }

    private void OnDisable()
    {
        BossStateHandlerOld.OnBossActiveStart -= BossStateHandler_OnBossActiveStart;
        BossStateHandlerOld.OnBossPhaseChangeStart -= BossStateHandler_OnBossPhaseChangeStart;
        BossStateHandlerOld.OnBossDefeated -= BossStateHandler_OnBossDefeated;
        BossStateHandlerOld.OnPlayerDefeated -= BossStateHandler_OnPlayerDefeated;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        InitializeVariables();
        ResetTimer();
    }

    private void Update()
    {
        HandlePassiveDischarge();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one BossKaerumOvercharge instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeVariables()
    {
        currentChargeInPhase = 0;
    }

    private void CheckHit()
    {
        if (BossPhaseHandlerOld.Instance.isDefeated) return;

        OnBossHit?.Invoke(this, new OnBossHitEventArgs { isInvulnerable = BossPhaseHandlerOld.Instance.isInvulnerable });

        if (!BossPhaseHandlerOld.Instance.isInvulnerable)
        {
            currentChargeInPhase+= chargePerProjectile;
            OnCurrentChargeInPhaseChanged?.Invoke(this, new OnCurrentChargeInPhaseChangedEventArgs { currentChargeInPhase = currentChargeInPhase, chargeLimitPerPhase = chargeLimitPerPhase });
        }

        CheckOvercharge();
    }

    private void CheckOvercharge()
    {
        if (currentChargeInPhase >= chargeLimitPerPhase)
        {
            OnBossOvercharge?.Invoke(this, EventArgs.Empty);
            ResetCurrentHitsInPhase();
        }
    }

    private void ResetCurrentHitsInPhase()
    {
        currentChargeInPhase = 0;
        OnCurrentChargeInPhaseReset?.Invoke(this, EventArgs.Empty);
    }

    private void HandlePassiveDischarge()
    {
        if (BossStateHandlerOld.Instance.BossState != BossStateHandlerOld.State.OnPhase) return;

        timeNotHit += Time.deltaTime;

        if (currentChargeInPhase <= 0) return;
        if (timeNotHit < timeForPassiveDischarge) return;

        currentChargeInPhase = currentChargeInPhase - passiveDischargePerSecond * Time.deltaTime <= 0 ? 0f : currentChargeInPhase - passiveDischargePerSecond * Time.deltaTime;

        OnCurrentChargeInPhaseChanged?.Invoke(this, new OnCurrentChargeInPhaseChangedEventArgs { currentChargeInPhase = currentChargeInPhase, chargeLimitPerPhase = chargeLimitPerPhase });
    }

    private void ResetTimer() => timeNotHit = 0f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out SignalProjectile signalProjectile))
        {
            CheckHit();
            ResetTimer();
        }
    }

    #region BossStateHandler Subscriptions
    private void BossStateHandler_OnBossActiveStart(object sender, EventArgs e)
    {
        ResetTimer();
    }
    private void BossStateHandler_OnBossPhaseChangeStart(object sender, BossStateHandlerOld.OnPhaseChangeEventArgs e)
    {
        ResetTimer();
    }

    private void BossStateHandler_OnPlayerDefeated(object sender, EventArgs e)
    {
        ResetTimer();
    }

    private void BossStateHandler_OnBossDefeated(object sender, EventArgs e)
    {
        ResetTimer();
    }
    #endregion
}
