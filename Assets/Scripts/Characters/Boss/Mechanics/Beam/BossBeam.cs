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
        BossStateHandler.OnBossPhaseChangeStart += BossStateHandler_OnBossPhaseChangeStart;

        BossPhaseHandler.OnLastPhaseCompleated += BossPhaseHandler_OnLastPhaseCompleated;
        BossStateHandler.OnBossDefeated += BossStateHandler_OnBossDefeated;
    }

    private void OnDisable()
    {
        BossStateHandler.OnBossPhaseChangeStart -= BossStateHandler_OnBossPhaseChangeStart;
        BossStateHandler.OnBossPhaseChangeEnd -= BossStateHandler_OnBossPhaseChangeEnd;

        BossPhaseHandler.OnLastPhaseCompleated += BossPhaseHandler_OnLastPhaseCompleated;
        BossStateHandler.OnBossDefeated -= BossStateHandler_OnBossDefeated;
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
        if(timer < currentPhaseBeam.activationTime)
        {
            timer += Time.deltaTime;
            return;
        }

        ResetTimer();

        //ChooseStunableProjectionPlatform;
        //Fire Event

        SetBeamState(State.Charging);
    }

    private void ChargingLogic()
    {
        if (timer < currentPhaseBeam.chargeTime)
        {
            timer += Time.deltaTime;
            return;
        }

        ResetTimer();

        //DematerializeOnProjectionPlatform;
        //Fire Event

        SetBeamState(State.OnCooldown);
    }

    private void OnCooldownLogic()
    {
        if (timer < currentPhaseBeam.cooldownTime)
        {
            timer += Time.deltaTime;
            return;
        }

        ResetTimer();

        //ChooseStunableProjectionPlatform;
        //FireEvent

        SetBeamState(State.Charging);
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
        ResetTimer();

        foreach(PhaseBeam phaseBeam in phaseBeams)
        {
            if(phaseBeam.bossPhase == bossPhase)
            {
                SetCurrentPhaseBeam(phaseBeam);
                SetBeamState(State.Activating);
                return;
            }
        }

        ClearCurrentPhaseBeam();
        SetBeamState(State.Disabled);
    }

    private void SetCurrentPhaseBeam(PhaseBeam phaseBeam) => currentPhaseBeam = phaseBeam;
    private void ClearCurrentPhaseBeam() => currentPhaseBeam = null;
    private void SetTimer(float time) => timer = time;
    private void ResetTimer() => timer = 0f;

    private void SuddenBeamDisable()
    {
        if(state == State.Charging)
        {
            //EndCharge
        }

        if(state == State.OnCooldown)
        {
            //EndCooldown
        }

        ResetTimer();
        ClearCurrentPhaseBeam();
        SetBeamState(State.Disabled);
    }

    #region BossState&PhaseHandler Subscriptions
    private void BossStateHandler_OnBossPhaseChangeStart(object sender, BossStateHandler.OnPhaseChangeEventArgs e)
    {
        SuddenBeamDisable();
    }

    private void BossStateHandler_OnBossPhaseChangeEnd(object sender, BossStateHandler.OnPhaseChangeEventArgs e)
    {
        CheckBossBeamEnable(e.nextPhase);
    }

    private void BossPhaseHandler_OnLastPhaseCompleated(object sender, EventArgs e)
    {
        SuddenBeamDisable();
    }
    private void BossStateHandler_OnBossDefeated(object sender, EventArgs e)
    {
        SuddenBeamDisable();
    }

    #endregion
}
