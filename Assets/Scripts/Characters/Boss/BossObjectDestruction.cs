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
    [SerializeField] private List<ProjectionPlatform> projectionPlatforms;

    [Header("States")]
    [SerializeField] private State bossObjectDestructionState;

    [Header("Debug")]
    [SerializeField] private bool debug;
    [SerializeField] private float timer;
    [SerializeField] private ProjectionPlatform currentTargetedProjectionPlatform;

    private enum State { Disabled, NotTargeting, Targeting }

    public static event EventHandler<OnProjectionPlatformTargetEventArgs> OnProjectionPlatformTarget;
    public static event EventHandler<OnProjectionPlatformTargetEventArgs> OnProjectionPlatformTargetRemoved;
    public static event EventHandler<OnProjectionPlatformTargetEventArgs> OnProjectionPlatformTargetDestoyed;

    public static event EventHandler OnBossDestroyedAllObjects;

    public class OnProjectionPlatformTargetEventArgs : EventArgs
    {
        public ProjectionPlatform projectionPlatform;
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
        InitializeVariables();
        SetBossObjectDestructionState(State.Disabled);
    }

    private void Update()
    {
        HandleBossObjectDestructionState();
    }
    private void InitializeVariables()
    {
        ResetTimer();
    }

    private void SetBossObjectDestructionState(State state) => this.bossObjectDestructionState = state;

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

    private void DisabledLogic() { }
    private void NotTargetingLogic() 
    { 
    
    }
    private void TargetingLogic()
    {

    }

    private void TargetProjectionPlatform()
    {
        ProjectionPlatform targetProjectionPlatform = ChooseRandomProjectionPlatform(GetProjectionPlatformsWithObject());

        if (!targetProjectionPlatform)
        {
            if (debug) Debug.Log("There are any projection platforms with objects to target");
            return;
        }

        currentTargetedProjectionPlatform = targetProjectionPlatform;
        OnProjectionPlatformTarget?.Invoke(this, new OnProjectionPlatformTargetEventArgs { projectionPlatform = currentTargetedProjectionPlatform });
    }

    private void ToRemoveMethod()
    {
        if (BossStateHandler.Instance.BossState != BossStateHandler.State.OnPhase) return;

        if (timer > 0) timer -= Time.deltaTime;

        if (timer <= 0)
        {
            DestroyObjectInPlatform();
            ResetTimer();
        }
    }

    private void DestroyObjectInPlatform()
    {
        if (!currentTargetedProjectionPlatform.HasObject())
        {
            OnProjectionPlatformTargetRemoved?.Invoke(this, new OnProjectionPlatformTargetEventArgs { projectionPlatform = currentTargetedProjectionPlatform });
            if (debug) Debug.Log("Current targeted platform does not have an object");

            currentTargetedProjectionPlatform = null;
            return;
        }

        currentTargetedProjectionPlatform.CurrentProjectedObject.DestroyProjectableObject();
        OnProjectionPlatformTargetDestoyed?.Invoke(this, new OnProjectionPlatformTargetEventArgs { projectionPlatform = currentTargetedProjectionPlatform });
        if (debug) Debug.Log("Object in current targeted platform destroyed");

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

    private ProjectionPlatform ChooseRandomProjectionPlatform(List<ProjectionPlatform> projectionPlatforms)
    {
        if (projectionPlatforms.Count == 0) return null;

        int randomIndex = UnityEngine.Random.Range(0, projectionPlatforms.Count - 1);
        return this.projectionPlatforms[randomIndex];
    }

    private void CheckPlayerLose()
    {
        if (ProjectionGemsManager.Instance.AvailableProjectionGems > 0) return;
        if (ProjectionManager.Instance.CurrentProjectedObjectsComponents.Count > 0) return;

        OnBossDestroyedAllObjects?.Invoke(this, EventArgs.Empty);
    }

    private void ResetTimer() => timer = 0f;

    #region BossStateHandler Subscriptions
    private void BossStateHandler_OnBossActiveStart(object sender, EventArgs e)
    {
        ResetTimer();
    }
    private void BossStateHandler_OnBossActiveEnd(object sender, EventArgs e)
    {
        ResetTimer();
        if(enableSinceFirstPhase) SetBossObjectDestructionState(State.NotTargeting);

    }
    private void BossStateHandler_OnBossPhaseChangeStart(object sender, BossStateHandler.OnPhaseChangeEventArgs e)
    {
        ResetTimer();
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
    }
    private void BossStateHandler_OnPlayerDefeated(object sender, EventArgs e)
    {
        ResetTimer();
        SetBossObjectDestructionState(State.Disabled);
    }
    #endregion
}
