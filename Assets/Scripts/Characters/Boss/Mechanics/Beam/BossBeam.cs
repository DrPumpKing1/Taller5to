using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using static BossBeam;
using UnityEngine.SocialPlatforms;

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
    public StunableProjectionPlatformProjection currentTargetedProjectionPlatform;

    public static event EventHandler<OnBeamEventArgs> OnBeamChargeStart;
    public static event EventHandler<OnBeamEventArgs> OnBeamChargeEnd;

    public static event EventHandler<OnBeamPlatformTargetEventArgs> OnBeamPlatformTargeted;
    public static event EventHandler<OnBeamPlatformTargetEventArgs> OnBeamPlatformTargetCleared;
    public static event EventHandler<OnBeamPlatformStunEventArgs> OnBeamPlatformStun;

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
        [Range(10f, 80f)] public float selectionRadius;
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

    public class OnBeamPlatformTargetEventArgs : EventArgs
    {
        public Transform beamSphere;
        public StunableProjectionPlatformProjection stunableProjectionPlatformProjection;
    }

    public class OnBeamPlatformStunEventArgs : EventArgs
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

        ProjectionPlatformProjection.OnAnyObjectProjectionSuccess += ProjectionPlatformProjection_OnAnyObjectProjectionSuccess;
        ProjectableObjectDematerialization.OnAnyObjectDematerialized += ProjectableObjectDematerialization_OnAnyObjectDematerialized;
    }

    private void OnDisable()
    {
        BossStateHandler.OnBossPhaseChangeStart -= BossStateHandler_OnBossPhaseChangeStart;
        BossStateHandler.OnBossPhaseChangeEnd -= BossStateHandler_OnBossPhaseChangeEnd;

        BossPhaseHandler.OnLastPhaseCompleated -= BossPhaseHandler_OnLastPhaseCompleated;
        BossStateHandler.OnBossDefeated -= BossStateHandler_OnBossDefeated;

        ProjectionPlatformProjection.OnAnyObjectProjectionSuccess -= ProjectionPlatformProjection_OnAnyObjectProjectionSuccess;
        ProjectableObjectDematerialization.OnAnyObjectDematerialized -= ProjectableObjectDematerialization_OnAnyObjectDematerialized;
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

        SetRandomBeamSphere();
        TargetProjectionPlatform();

        SetBeamState(State.Charging);

        OnBeamChargeStart?.Invoke(this, new OnBeamEventArgs { bossPhase = currentPhaseBeam.bossPhase });
    }

    private void OnChargeEnd()
    {
        ResetTimer();

        DematerializeInPlatform();
        StunProjectionPlatform();

        ClearCurrentTargetedProjectionPlatform();
        ClearCurrentBeamSphere();

        SetBeamState(State.OnCooldown);

        OnBeamChargeEnd?.Invoke(this, new OnBeamEventArgs { bossPhase = currentPhaseBeam.bossPhase });
    }

    private void OnCooldownEnd()
    {
        ResetTimer();

        SetRandomBeamSphere();
        TargetProjectionPlatform();

        SetBeamState(State.Charging);

        OnBeamChargeStart?.Invoke(this, new OnBeamEventArgs { bossPhase = currentPhaseBeam.bossPhase });
    }
    #endregion


    private void TargetProjectionPlatform()
    {
        List<StunableProjectionPlatformProjection> platformsInRange = GetStunableProjectionPlatformsInSphereRange(currentPhaseBeam.stunablePlatforms, currentBeamSphere, currentPhaseBeam.selectionRadius);

        List<StunableProjectionPlatformProjection> validPlatformsWithDrainer = GetStunableProjectionPlatformsWithSpecificObject(platformsInRange, DRAINER_ID);
        List<StunableProjectionPlatformProjection> validPlatformsWithObject = GetStunableProjectionPlatformsWithObject(platformsInRange);

        List<StunableProjectionPlatformProjection> platformsPool;

        if (validPlatformsWithDrainer.Count > 0) platformsPool = validPlatformsWithDrainer;
        else platformsPool = validPlatformsWithObject;

        if (platformsPool.Count <= 0) return;

        int randomIndex = UnityEngine.Random.Range(0, platformsPool.Count);
        StunableProjectionPlatformProjection targetPlatform = platformsPool[randomIndex];

        SetTargetedProjectionPlatform(targetPlatform);
    }

    private void CheckReTargetProjectionPlatform()
    {
        if (currentTargetedProjectionPlatform)
        {
            if (CheckValidStunableProjectionPlatform(currentTargetedProjectionPlatform, currentBeamSphere, currentPhaseBeam.selectionRadius)) return;
            ClearCurrentTargetedProjectionPlatform();
        }

        TargetProjectionPlatform();
    }

    private void DematerializeInPlatform()
    {
        if (!currentTargetedProjectionPlatform) return;

        BossObjectDematerialization.Instance.DematerializeInTargetPlatform(currentTargetedProjectionPlatform.ProjectionPlatform);
    }

    private void StunProjectionPlatform()
    {
        if (!currentTargetedProjectionPlatform) return;

        OnBeamPlatformStun?.Invoke(this, new OnBeamPlatformStunEventArgs { stunableProjectionPlatformProjection = currentTargetedProjectionPlatform, stunTime = currentPhaseBeam.stunTime });
    }

    private List<StunableProjectionPlatformProjection> GetStunableProjectionPlatformsInSphereRange(List<StunableProjectionPlatformProjection> pool, Transform sphereTransform, float range)
    {
        List<StunableProjectionPlatformProjection> stunableProjectionPlatformsInRange = new List<StunableProjectionPlatformProjection>();

        Vector3 supressedYComponentSpherePosition = GeneralMethods.SupressYComponent(sphereTransform.position);

        foreach (StunableProjectionPlatformProjection platform in pool)
        {
            Vector3 supressedYComponentPlatformPosition = GeneralMethods.SupressYComponent(platform.transform.position);
            if (Vector3.Distance(supressedYComponentPlatformPosition, supressedYComponentSpherePosition) > range) continue;          
            stunableProjectionPlatformsInRange.Add(platform);
        }

        return stunableProjectionPlatformsInRange;
    }

    private List<StunableProjectionPlatformProjection> GetStunableProjectionPlatformsWithObject(List<StunableProjectionPlatformProjection> pool)
    {
        List<StunableProjectionPlatformProjection> stunableProjectionPlatformsWithObject = new List<StunableProjectionPlatformProjection>();

        foreach (StunableProjectionPlatformProjection platform in pool)
        {
            if (!platform.ProjectionPlatform.HasObject()) continue;
            stunableProjectionPlatformsWithObject.Add(platform);
        }

        return stunableProjectionPlatformsWithObject;
    }

    private List<StunableProjectionPlatformProjection> GetStunableProjectionPlatformsWithSpecificObject(List<StunableProjectionPlatformProjection> pool, int objectID)
    {
        List<StunableProjectionPlatformProjection> stunableProjectionPlatformsWithSpecificObject = new List<StunableProjectionPlatformProjection>();

        foreach (StunableProjectionPlatformProjection platform in pool)
        {
            if (!platform.ProjectionPlatform.HasObject()) continue;
            if (platform.ProjectionPlatform.CurrentProjectedObjectSO.id != objectID) continue;
            stunableProjectionPlatformsWithSpecificObject.Add(platform);
        }

        return stunableProjectionPlatformsWithSpecificObject;
    }

    private bool CheckValidStunableProjectionPlatform(StunableProjectionPlatformProjection platform, Transform sphereTransform, float range)
    {
        List<StunableProjectionPlatformProjection> platformsInRange = GetStunableProjectionPlatformsInSphereRange(currentPhaseBeam.stunablePlatforms, currentBeamSphere, currentPhaseBeam.selectionRadius);

        List<StunableProjectionPlatformProjection> validPlatformsWithDrainer = GetStunableProjectionPlatformsWithSpecificObject(platformsInRange, DRAINER_ID);
        List<StunableProjectionPlatformProjection> validPlatformsWithObject = GetStunableProjectionPlatformsWithObject(platformsInRange);

        if (!platformsInRange.Contains(platform)) return false;
        if (!validPlatformsWithObject.Contains(platform)) return false;
        if (validPlatformsWithDrainer.Count > 0 && !validPlatformsWithDrainer.Contains(platform)) return false;
        
        return true;
    }

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
    private void SetTargetedProjectionPlatform(StunableProjectionPlatformProjection stunableProjectionPlatform)
    {
        currentTargetedProjectionPlatform = stunableProjectionPlatform;
        OnBeamPlatformTargeted?.Invoke(this,new OnBeamPlatformTargetEventArgs { stunableProjectionPlatformProjection= currentTargetedProjectionPlatform, beamSphere = currentBeamSphere });
    }
    private void ClearCurrentTargetedProjectionPlatform()
    {
        OnBeamPlatformTargetCleared?.Invoke(this, new OnBeamPlatformTargetEventArgs { stunableProjectionPlatformProjection = currentTargetedProjectionPlatform, beamSphere = currentBeamSphere });
        currentTargetedProjectionPlatform = null;
    }

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
        ClearCurrentTargetedProjectionPlatform();
        ClearCurrentBeamSphere();
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

    #region ProjectionPlatformProjection Subscriptions
    private void ProjectableObjectDematerialization_OnAnyObjectDematerialized(object sender, ProjectableObjectDematerialization.OnAnyObjectDematerializedEventArgs e)
    {
        if (state != State.Charging) return;

        CheckReTargetProjectionPlatform();
    }

    private void ProjectionPlatformProjection_OnAnyObjectProjectionSuccess(object sender, ProjectionPlatformProjection.OnAnyProjectionEventArgs e)
    {
        if (state != State.Charging) return;

        CheckReTargetProjectionPlatform();
    }
    #endregion
}
