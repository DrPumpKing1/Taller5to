using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowcaseRoomBeam : MonoBehaviour
{
    public static ShowcaseRoomBeam Instance { get; private set; }

    [Header("States")]
    [SerializeField] private State state;

    [Header("Settings")]
    [SerializeField] private Beam beam;
    [SerializeField] private List<Transform> beamSpheres;
    [SerializeField] private string logToEnable;
    [SerializeField] private string logToDisable;

    public enum State { Disabled, Activating, Charging, OnCooldown }
    public State BeamState => state;
    private float timer;

    private const int DRAINER_ID = 4;
    private Transform currentBeamSphere;
    private StunableProjectionPlatformProjection currentTargetedProjectionPlatform;

    public static event EventHandler<OnBeamEventArgs> OnBeamChargeStart;
    public static event EventHandler<OnBeamEventArgs> OnBeamChargeEnd;

    public static event EventHandler<OnBeamPlatformTargetEventArgs> OnBeamPlatformTargeted;
    public static event EventHandler<OnBeamPlatformTargetEventArgs> OnBeamPlatformTargetCleared;
    public static event EventHandler<OnBeamPlatformStunEventArgs> OnBeamPlatformStun;
    public static event EventHandler<OnBeamObjectDematerializationEventArgs> OnBeamObjectDematerialization;

    public static event EventHandler<OnBeamSphereEventArgs> OnBeamSphereSelected;
    public static event EventHandler<OnBeamSphereEventArgs> OnBeamSphereCleared;

    [Serializable]
    public class Beam
    {
        [Range(1f, 20f)] public float activationTime;
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
        public Beam beam;
        public Transform beamSphere;
    }

    public class OnBeamPlatformTargetEventArgs : EventArgs
    {
        public Transform beamSphere;
        public StunableProjectionPlatformProjection stunableProjectionPlatformProjection;
        public float chargeTime;
    }

    public class OnBeamPlatformStunEventArgs : EventArgs
    {
        public StunableProjectionPlatformProjection stunableProjectionPlatformProjection;
        public float stunTime;
    }

    public class OnBeamObjectDematerializationEventArgs : EventArgs
    {
        public StunableProjectionPlatformProjection stunableProjectionPlatformProjection;
    }

    private void OnEnable()
    {
        GameLogManager.OnLogAdd += GameLogManager_OnLogAdd;
        ProjectionPlatformProjection.OnAnyObjectProjectionSuccess += ProjectionPlatformProjection_OnAnyObjectProjectionSuccess;
        ProjectableObjectDematerialization.OnAnyObjectDematerialized += ProjectableObjectDematerialization_OnAnyObjectDematerialized;
    }


    private void OnDisable()
    {
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
        HandleShowcaseRoomBeamStates();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one ShowcaseRoomBeam, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void SetBeamState(State state) => this.state = state;

    private void HandleShowcaseRoomBeamStates()
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
        if (timer < beam.activationTime)
        {
            timer += Time.deltaTime;
            return;
        }

        OnActivationEnd();
    }

    private void ChargingLogic()
    {
        if (timer < beam.chargeTime)
        {
            timer += Time.deltaTime;
            return;
        }

        OnChargeEnd();
    }

    private void OnCooldownLogic()
    {
        if (timer < beam.cooldownTime)
        {
            timer += Time.deltaTime;
            return;
        }

        OnCooldownEnd();
    }

    private void CheckBeamEnable(string log)
    {
        if (log != logToEnable) return;

        ResetTimer();
        SetBeamState(State.Activating);
    }

    private void CheckBeamDisable(string log)
    {
        if (log != logToDisable) return;

        SuddenBeamDisable();
    }

    #region OnStateEnds
    private void OnActivationEnd()
    {
        ResetTimer();

        SetRandomBeamSphere();
        TargetProjectionPlatform();

        SetBeamState(State.Charging);

        OnBeamChargeStart?.Invoke(this, new OnBeamEventArgs { beam = beam, beamSphere = currentBeamSphere });
    }

    private void OnChargeEnd()
    {
        ResetTimer();

        DematerializeInPlatform();
        StunProjectionPlatform();

        ClearCurrentTargetedProjectionPlatform();

        Transform previousBeamSphere = currentBeamSphere;
        ClearCurrentBeamSphere();

        SetBeamState(State.OnCooldown);

        OnBeamChargeEnd?.Invoke(this, new OnBeamEventArgs { beam = beam, beamSphere = previousBeamSphere });
    }

    private void OnCooldownEnd()
    {
        ResetTimer();

        SetRandomBeamSphere();
        TargetProjectionPlatform();

        SetBeamState(State.Charging);

        OnBeamChargeStart?.Invoke(this, new OnBeamEventArgs { beam = beam, beamSphere = currentBeamSphere });
    }
    #endregion

    private void TargetProjectionPlatform()
    {
        List<StunableProjectionPlatformProjection> platformsInRange = GetStunableProjectionPlatformsInSphereRange(beam.stunablePlatforms, currentBeamSphere, beam.selectionRadius);

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
            if (CheckValidStunableProjectionPlatform(currentTargetedProjectionPlatform, currentBeamSphere, beam.selectionRadius)) return;
            ClearCurrentTargetedProjectionPlatform();
        }

        TargetProjectionPlatform();
    }

    private void DematerializeInPlatform()
    {
        if (!currentTargetedProjectionPlatform) return;

        BossObjectDematerialization.Instance.DematerializeInTargetPlatform(currentTargetedProjectionPlatform.ProjectionPlatform);

        OnBeamObjectDematerialization?.Invoke(this, new OnBeamObjectDematerializationEventArgs { stunableProjectionPlatformProjection = currentTargetedProjectionPlatform });
    }

    private void StunProjectionPlatform()
    {
        if (!currentTargetedProjectionPlatform) return;

        OnBeamPlatformStun?.Invoke(this, new OnBeamPlatformStunEventArgs { stunableProjectionPlatformProjection = currentTargetedProjectionPlatform, stunTime = beam.stunTime });
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
        List<StunableProjectionPlatformProjection> platformsInRange = GetStunableProjectionPlatformsInSphereRange(beam.stunablePlatforms, currentBeamSphere, beam.selectionRadius);

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

    private void SetTargetedProjectionPlatform(StunableProjectionPlatformProjection stunableProjectionPlatform)
    {
        currentTargetedProjectionPlatform = stunableProjectionPlatform;
        OnBeamPlatformTargeted?.Invoke(this, new OnBeamPlatformTargetEventArgs { stunableProjectionPlatformProjection = currentTargetedProjectionPlatform, beamSphere = currentBeamSphere, chargeTime = beam.chargeTime });
    }
    private void ClearCurrentTargetedProjectionPlatform()
    {
        OnBeamPlatformTargetCleared?.Invoke(this, new OnBeamPlatformTargetEventArgs { stunableProjectionPlatformProjection = currentTargetedProjectionPlatform, beamSphere = currentBeamSphere, chargeTime = 0f });
        currentTargetedProjectionPlatform = null;
    }

    private void SetTimer(float time) => timer = time;
    private void ResetTimer() => timer = 0f;

    private void SuddenBeamDisable()
    {
        if (state == State.Charging)
        {
            OnBeamChargeEnd?.Invoke(this, new OnBeamEventArgs { beam = beam, beamSphere = currentBeamSphere });
        }

        ResetTimer();
        ClearCurrentTargetedProjectionPlatform();
        ClearCurrentBeamSphere();
        SetBeamState(State.Disabled);
    }

    #region GameLog Subscriptions
    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        CheckBeamEnable(e.gameplayAction.log);
        CheckBeamDisable(e.gameplayAction.log);
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
