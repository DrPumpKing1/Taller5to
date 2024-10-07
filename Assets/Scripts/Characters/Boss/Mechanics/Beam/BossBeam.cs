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
    [SerializeField] private List<Transform> beamSpheres;

    public enum State { Disabled,Activating, Charging, OnCooldown}
    public State BeamState => state;
    private float timer;

    private const int DRAINER_ID = 4;
    private PhaseBeam currentPhaseBeam;
    public Transform currentBeamSphere;

    public static event EventHandler<OnBeamEventArgs> OnBeamStart;
    public static event EventHandler<OnBeamEventArgs> OnBeamEnd;

    public static event EventHandler<OnBeamStunEventArgs> OnBeamStun;

    public static event EventHandler<OnBeamSphereEventArgs> OnBeamSphereSelected;
    public static event EventHandler<OnBeamSphereEventArgs> OnBeamSphereCleared;

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

    public class OnBeamSphereEventArgs : EventArgs
    {
        public Transform beamSphere;
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

        OnActivationEnd();
    }

    private void ChargingLogic()
    {
        if (timer < currentPhaseBeam.chargeTime)
        {
            timer += Time.deltaTime;
            return;
        }

        OnChargeEnd();
    }

    private void OnCooldownLogic()
    {
        if (timer < currentPhaseBeam.cooldownTime)
        {
            timer += Time.deltaTime;
            return;
        }

        OnCooldownEnd();
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

    #region OnStateEnds
    private void OnActivationEnd()
    {
        ResetTimer();

        //ChooseStunableProjectionPlatform;
        //Fire Event

        SetRandomBeamSphere();
        SetBeamState(State.Charging);
    }

    private void OnChargeEnd()
    {
        ResetTimer();

        //DematerializeOnProjectionPlatform;
        //Fire Event

        ClearCurrentBeamSphere();
        SetBeamState(State.OnCooldown);
    }

    private void OnCooldownEnd()
    {
        ResetTimer();

        //ChooseStunableProjectionPlatform;
        //FireEvent

        SetRandomBeamSphere();
        SetBeamState(State.Charging);
    }
    #endregion

    #region SelectBeamSphere
    private void SetRandomBeamSphere() => SetCurrentBeamSphere(SelectRandomBeamSphere());

    private Transform SelectRandomBeamSphere()
    {
        int randomIndex = UnityEngine.Random.Range(0, beamSpheres.Count);
        return beamSpheres[randomIndex];
    }

    private void SetCurrentBeamSphere(Transform beamSphere)
    {
        currentBeamSphere = beamSphere;
        OnBeamSphereSelected?.Invoke(this, new OnBeamSphereEventArgs { beamSphere = currentBeamSphere });
    }
    private void ClearCurrentBeamSphere() 
    {
        OnBeamSphereCleared?.Invoke(this, new OnBeamSphereEventArgs { beamSphere = currentBeamSphere });
        currentBeamSphere = null;      
    }
    #endregion
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
        ClearCurrentBeamSphere();
        SetBeamState(State.Disabled);
    }

    private List<StunableProjectionPlatformProjection> GetStunableProjectionPlatformsInSphereRange(List<StunableProjectionPlatformProjection> pool,Transform sphereTransform, float range)
    {
        List<StunableProjectionPlatformProjection> stunableProjectionPlatformsInRange = new List<StunableProjectionPlatformProjection>();

        Vector3 supressedYComponentSpherePosition = GeneralMethods.SupressYComponent(sphereTransform.position);

        foreach(StunableProjectionPlatformProjection platform in pool)
        {
            Vector3 supressedYComponentPlatformPosition = GeneralMethods.SupressYComponent(platform.transform.position);

            if(Vector3.Distance(supressedYComponentPlatformPosition, supressedYComponentSpherePosition) <= range) stunableProjectionPlatformsInRange.Add(platform);
        }

        return stunableProjectionPlatformsInRange;
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
