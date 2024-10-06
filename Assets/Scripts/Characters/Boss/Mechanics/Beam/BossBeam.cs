using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class BossBeam : MonoBehaviour
{
    public static BossBeam Instance {  get; private set; }

    [Header("States")]
    [SerializeField] private State state;
    
    [Header("Settings")]
    [SerializeField] private List<PhaseBeam> phaseBeams;

    public enum State { Disabled,Activating, Charging, OnCooldown}

    public State BeamState => state;
    private float timer;

    public static event EventHandler<OnBeamEventArgs> OnBeamStart;
    public static event EventHandler<OnBeamEventArgs> OnBeamEnd;

    public static event EventHandler<OnBeamStunEventArgs> OnBeamStun;

    public StunableProjectionPlatformProjection test;

    [Serializable]
    public class PhaseBeam
    {
        public BossPhase bossPhase;
        public float activationTime;
        public float chargeTime;
        public float cooldownTime;
        public float stunTime;
        public float selectionRadius;
        public List<StunableProjectionPlatformProjection> stunablePlatforms;
    }

    public class OnBeamEventArgs : EventArgs
    {
        public BossPhase bossPhase;
    }

    public class OnBeamStunEventArgs : EventArgs
    {
        public StunableProjectionPlatformProjection stunableProjectionPlatformProjection;
        public float stunTime;
    }

    private void OnEnable()
    {
        BossStateHandler.OnBossPhaseChangeEnd += BossStateHandler_OnBossPhaseChangeEnd;
    }

    private void OnDisable()
    {
        BossStateHandler.OnBossPhaseChangeEnd -= BossStateHandler_OnBossPhaseChangeEnd;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        ResetTimer();
    }

    private void Update()
    {     
        HandleBossBeamStates();

        if (Input.GetKeyDown(KeyCode.L))
        {
            OnBeamStun?.Invoke(this, new OnBeamStunEventArgs { stunableProjectionPlatformProjection = test, stunTime = 3 });
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
            Debug.LogWarning("There is more than one BossBeam, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void SetBeamState(State state) => this.state = state;

    private void HandleBossBeamStates()
    {
        switch (state)
        {
            case State.Disabled:
                DisabledLogic();
                break;
            case State.Activating:
                ActivatingLogic();
                break;
            case State.Charging:
                ChargingLogic();
                break;
            case State.OnCooldown:
                OnCooldownLogic();
                break;

        }
    }

    private void DisabledLogic()
    {

    }

    private void ActivatingLogic()
    {

    }

    private void ChargingLogic()
    {

    }

    private void OnCooldownLogic()
    {

    }

    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            OnBeamStart?.Invoke(this, new OnBeamEventArgs { bossPhase = BossPhase.Phase3 });
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            OnBeamEnd?.Invoke(this, new OnBeamEventArgs { bossPhase = BossPhase.Phase3 });
        }
    }

    private void ResetTimer() => timer = 0f;

    #region BossStateHandler Subscriptions
    private void BossStateHandler_OnBossPhaseChangeEnd(object sender, BossStateHandler.OnPhaseChangeEventArgs e)
    {
        
    }
    #endregion
}
