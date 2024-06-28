using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BossObjectDestruction : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool enableSinceFirstPhase;
    [SerializeField] private int phaseNumberToEnable;
    [SerializeField] private float timeNotTargeting;
    [SerializeField] private float timeTargeting;

    [Header("Target Settings")]
    [SerializeField] private bool stopTargetingWhenObjectDematerialized;
    [SerializeField] private bool priorizeSenders;
    [SerializeField] private List<ProjectionPlatform> projectionPlatforms;

    [Header("States")]
    [SerializeField] private State bossObjectDestructionState;

    [Header("Debug")]
    [SerializeField] private bool debug;
    [SerializeField] private float timer;
    [SerializeField] private int lockedProjectionGems;
    [SerializeField] private ProjectionPlatform currentTargetedProjectionPlatform;

    private enum State { Disabled, NotTargeting, Targeting }

    private const int SENDER_ID = 3;    

    public static event EventHandler<OnProjectionPlatformTargetEventArgs> OnProjectionPlatformTarget;
    public static event EventHandler<OnProjectionPlatformTargetEventArgs> OnProjectionPlatformTargetRemoved;
    public static event EventHandler<OnProjectionPlatformTargetEventArgs> OnProjectionPlatformTargetDestoyed;

    public static event EventHandler OnBossAllProjectionGemsLocked;

    public class OnProjectionPlatformTargetEventArgs : EventArgs
    {
        public ProjectionPlatform projectionPlatform;
        public ProjectableObjectSO projectableObjectSO;
        public float timeTargeting;
    }

    private void OnEnable()
    {
        BossStateHandler.OnBossActiveStart += BossStateHandler_OnBossActiveStart;
        BossStateHandler.OnBossActiveEnd += BossStateHandler_OnBossActiveEnd;

        BossStateHandler.OnBossPhaseChangeStart += BossStateHandler_OnBossPhaseChangeStart;
        BossStateHandler.OnBossPhaseChangeEnd += BossStateHandler_OnBossPhaseChangeEnd;

        BossStateHandler.OnBossDefeated += BossStateHandler_OnBossDefeated;
        BossStateHandler.OnPlayerDefeated += BossStateHandler_OnPlayerDefeated;
    }


    private void OnDisable()
    {
        BossStateHandler.OnBossActiveStart -= BossStateHandler_OnBossActiveStart;

        BossStateHandler.OnBossPhaseChangeStart -= BossStateHandler_OnBossPhaseChangeStart;
        BossStateHandler.OnBossPhaseChangeEnd -= BossStateHandler_OnBossPhaseChangeEnd;

        BossStateHandler.OnBossDefeated -= BossStateHandler_OnBossDefeated;
        BossStateHandler.OnPlayerDefeated -= BossStateHandler_OnPlayerDefeated;
    }

    private void Start()
    {
        ResetTimer();
        InitializeVariables();
        SetBossObjectDestructionState(State.Disabled);
    }

    private void Update()
    {
        HandleBossObjectDestructionState();
    }
    private void InitializeVariables()
    {
        lockedProjectionGems = 0;
    }

    private void SetBossObjectDestructionState(State state) => bossObjectDestructionState = state;

    private void HandleBossObjectDestructionState()
    {
        switch (bossObjectDestructionState)
        {
            case State.Disabled:
                DisabledLogic();
                break;
            case State.NotTargeting:
                NotTargetingLogic();
                break;
            case State.Targeting:
                TargetingLogic();
                break;
        }
    }

    private void DisabledLogic() 
    {
        if (currentTargetedProjectionPlatform)
        {
            OnProjectionPlatformTargetRemoved?.Invoke(this, new OnProjectionPlatformTargetEventArgs { projectionPlatform = currentTargetedProjectionPlatform, projectableObjectSO = currentTargetedProjectionPlatform.CurrentProjectedObjectSO });
            if (debug) Debug.Log("Current target removed");

            currentTargetedProjectionPlatform = null;
            return;
        }
    }
    private void NotTargetingLogic() 
    {
        if(timer >= timeNotTargeting)
        {
            SetBossObjectDestructionState(State.Targeting);

            TargetProjectionPlatform();
            ResetTimer();
            return;
        }

        timer += Time.deltaTime;
    }
    private void TargetingLogic()
    {
        if(!currentTargetedProjectionPlatform.HasObject() && stopTargetingWhenObjectDematerialized)
        {
            SetBossObjectDestructionState(State.NotTargeting);
            ResetTimer();

            OnProjectionPlatformTargetRemoved?.Invoke(this, new OnProjectionPlatformTargetEventArgs { projectionPlatform = currentTargetedProjectionPlatform, projectableObjectSO = currentTargetedProjectionPlatform.CurrentProjectedObjectSO });
            return;
        }

        if (timer >= timeTargeting)
        {
            SetBossObjectDestructionState(State.NotTargeting);

            DestroyObjectInPlatform();
            ResetTimer();
            return;
        }

        timer += Time.deltaTime;
    }

    private void TargetProjectionPlatform()
    {
        ProjectionPlatform targetProjectionPlatform = ChooseRandomProjectionPlatform(GetProjectionPlatformsWithObject());

        if (priorizeSenders)
        {
            ProjectionPlatform targetProjectionPlatformWithSender = ChooseRandomProjectionPlatform(GetProjectionPlatformsWithSender());
            if (targetProjectionPlatformWithSender) targetProjectionPlatform = targetProjectionPlatformWithSender;
        }

        if (!targetProjectionPlatform)
        {
            if (debug) Debug.Log("There are any projection platforms with objects to target");
            SetBossObjectDestructionState(State.NotTargeting);
            return;
        }

        currentTargetedProjectionPlatform = targetProjectionPlatform;
        if (debug) Debug.Log($"{targetProjectionPlatform} targeted");

        OnProjectionPlatformTarget?.Invoke(this, new OnProjectionPlatformTargetEventArgs { projectionPlatform = currentTargetedProjectionPlatform, projectableObjectSO = currentTargetedProjectionPlatform.CurrentProjectedObjectSO, timeTargeting = timeTargeting});
    }

    private void DestroyObjectInPlatform()
    {
        if (!currentTargetedProjectionPlatform.HasObject())
        {
            OnProjectionPlatformTargetRemoved?.Invoke(this, new OnProjectionPlatformTargetEventArgs { projectionPlatform = currentTargetedProjectionPlatform, projectableObjectSO = currentTargetedProjectionPlatform.CurrentProjectedObjectSO });
            if (debug) Debug.Log("Current targeted platform does not have an object");

            currentTargetedProjectionPlatform = null;
            return;
        }

        OnProjectionPlatformTargetDestoyed?.Invoke(this, new OnProjectionPlatformTargetEventArgs { projectionPlatform = currentTargetedProjectionPlatform, projectableObjectSO = currentTargetedProjectionPlatform.CurrentProjectedObjectSO });

        lockedProjectionGems += currentTargetedProjectionPlatform.CurrentProjectedObjectSO.projectionGemsCost;

        currentTargetedProjectionPlatform.CurrentProjectedObject.DestroyProjectableObject();
        if (debug) Debug.Log("Object in current targeted platform destroyed");

        currentTargetedProjectionPlatform = null;
        CheckPlayerLose();
    }

    private List<ProjectionPlatform> GetProjectionPlatformsWithObject()
    {
        List<ProjectionPlatform> projectionPlatformsWithObject = new List<ProjectionPlatform>();

        foreach(ProjectionPlatform projectionPlatform in projectionPlatforms)
        {
            if (projectionPlatform.HasObject()) projectionPlatformsWithObject.Add(projectionPlatform);
        }

        return projectionPlatformsWithObject;
    }

    private List<ProjectionPlatform> GetProjectionPlatformsWithSender()
    {
        List<ProjectionPlatform> projectionPlatformsWithSender = new List<ProjectionPlatform>();

        foreach (ProjectionPlatform projectionPlatform in projectionPlatforms)
        {
            if (!projectionPlatform.HasObject()) continue;
            if (projectionPlatform.CurrentProjectedObjectSO.id != SENDER_ID) continue;
            projectionPlatformsWithSender.Add(projectionPlatform);
        }

        return projectionPlatformsWithSender;
    }

    private ProjectionPlatform ChooseRandomProjectionPlatform(List<ProjectionPlatform> projectionPlatforms)
    {
        if (projectionPlatforms.Count == 0) return null;

        int randomIndex = UnityEngine.Random.Range(0, projectionPlatforms.Count);
        return projectionPlatforms[randomIndex];
    }

    private void CheckPlayerLose()
    {
        if (lockedProjectionGems < ProjectionGemsManager.Instance.TotalProjectionGems) return;

        OnBossAllProjectionGemsLocked?.Invoke(this, EventArgs.Empty);
        if (debug) Debug.Log("Player Defeated");
    }

    private void CheckRefundLockedProjectionGems()
    {
        if (lockedProjectionGems > 0)
        {
            ProjectionGemsManager.Instance.RefundProjectionGems(lockedProjectionGems);
            if (debug) Debug.Log($"{lockedProjectionGems} projection gems refunded");
            lockedProjectionGems = 0;
            return;
        }

        if (debug) Debug.Log("No projection gems to refund");
    }

    private void ResetTimer() => timer = 0f;

    #region BossStateHandler Subscriptions
    private void BossStateHandler_OnBossActiveStart(object sender, EventArgs e)
    {
        ResetTimer();
        SetBossObjectDestructionState(State.Disabled);
    }
    private void BossStateHandler_OnBossActiveEnd(object sender, EventArgs e)
    {
        ResetTimer();
        if(enableSinceFirstPhase) SetBossObjectDestructionState(State.NotTargeting);

    }
    private void BossStateHandler_OnBossPhaseChangeStart(object sender, BossStateHandler.OnPhaseChangeEventArgs e)
    {
        ResetTimer();
        SetBossObjectDestructionState(State.Disabled);

        CheckRefundLockedProjectionGems();
    }

    private void BossStateHandler_OnBossPhaseChangeEnd(object sender, BossStateHandler.OnPhaseChangeEventArgs e)
    {
        ResetTimer();
        if (e.phaseNumber >= phaseNumberToEnable) SetBossObjectDestructionState(State.NotTargeting);
    }

    private void BossStateHandler_OnBossDefeated(object sender, EventArgs e)
    {
        ResetTimer();
        SetBossObjectDestructionState(State.Disabled);

        CheckRefundLockedProjectionGems();
    }
    private void BossStateHandler_OnPlayerDefeated(object sender, EventArgs e)
    {
        ResetTimer();
        SetBossObjectDestructionState(State.Disabled);
    }
    #endregion
}
