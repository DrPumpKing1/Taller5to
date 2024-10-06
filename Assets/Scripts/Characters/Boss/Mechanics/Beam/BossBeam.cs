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

    private const int DRAINER_ID = 4;
    private PhaseBeam currentPhaseBeam;

    public static event EventHandler<OnBeamEventArgs> OnBeamStart;
    public static event EventHandler<OnBeamEventArgs> OnBeamEnd;

    public static event EventHandler<OnBeamStunEventArgs> OnBeamStun;


    [Serializable]
    public class PhaseBeam
    {
        public BossPhase bossPhase;
        [Range(1f,20f)] public float activationTime;
        [Range(1f, 20f)] public float chargeTime;
        [Range(1f, 20f)] public float cooldownTime;
        [Range(1f, 20f)] public float stunTime;
        [Range(10f, 40f)] public float selectionRadius;
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
        SetBeamState(State.Disabled);
    }

    private void Update()
    {     
        HandleBossBeamStates();
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
        ResetTimer();
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

    private void CheckBossBeamEnable(BossPhase bossPhase)
    {
        foreach(PhaseBeam phaseBeam in phaseBeams)
        {
            if(phaseBeam.bossPhase == bossPhase)
            {
                SetCurrentPhaseBeam(phaseBeam);
                SetBeamState(State.Activating);
                return;
            }
        }
    }

    private void SetCurrentPhaseBeam(PhaseBeam phaseBeam) => currentPhaseBeam = phaseBeam;
    private void ClearCurrentPhaseBeam() => currentPhaseBeam = null;
    private void SetTimer(float time) => timer = time;
    private void ResetTimer() => timer = 0f;

    #region BossStateHandler Subscriptions
    private void BossStateHandler_OnBossPhaseChangeEnd(object sender, BossStateHandler.OnPhaseChangeEventArgs e)
    {
        CheckBossBeamEnable(e.nextPhase);
    }
    #endregion
}
