using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BossObjectDestruction : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float timeToDestroyPlatform;
    [SerializeField] private float graceTimeAfterPlatformDestruction;
    [SerializeField] private List<ProjectionPlatform> projectionPlatforms;
    [SerializeField] private bool randomizeProjectionPlatforms;
    [SerializeField] private bool resetTimerOnPhaseChange;
    [Space]
    [SerializeField] private float timer;

    [Header("States")]
    [SerializeField] private State bossObjectDestructionState;

    private enum State { Disabled, NotTargeting, Targeting }


    public static event EventHandler OnBossDestroyProjectionPlatform;
    public static event EventHandler OnBossDestroyAllProjectionPlatforms;

    private void OnEnable()
    {
        BossStateHandler.OnBossActiveStart += BossStateHandler_OnBossActiveStart;
        BossStateHandler.OnBossPhaseChangeStart += BossStateHandler_OnBossPhaseChangeStart;
        BossStateHandler.OnBossDefeated += BossStateHandler_OnBossDefeated;
        BossStateHandler.OnPlayerDefeated += BossStateHandler_OnPlayerDefeated;
    }

    private void OnDisable()
    {
        BossStateHandler.OnBossActiveStart -= BossStateHandler_OnBossActiveStart;
        BossStateHandler.OnBossPhaseChangeStart -= BossStateHandler_OnBossPhaseChangeStart;
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
        HandleProjectionPlatformDestruction();
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

    private void HandleProjectionPlatformDestruction()
    {
        if (BossStateHandler.Instance.BossState != BossStateHandler.State.OnPhase) return;

        if (timer > 0) timer -= Time.deltaTime;

        if(timer <= 0)
        {
            DestroyProjectableObject();
            ResetTimer();
        }
    }

    private void DestroyProjectableObject()
    {
        ProjectionPlatform projectionPlatformToDestroyObject;

        if (randomizeProjectionPlatforms) projectionPlatformToDestroyObject = ChooseRandomProjectionPlatform();
        else projectionPlatformToDestroyObject = ChooseFirstProjectionPlatform();

        projectionPlatforms.Remove(projectionPlatformToDestroyObject);
        projectionPlatformToDestroyObject.DestroyProjectionPlatform();

        OnBossDestroyProjectionPlatform?.Invoke(this, EventArgs.Empty);

        ResetTimer();

        CheckAllPlatformsDestroyed();
    }

    private ProjectionPlatform ChooseRandomProjectionPlatform()
    {
        int randomIndex = UnityEngine.Random.Range(0, projectionPlatforms.Count - 1);
        return projectionPlatforms[randomIndex];
    }

    private ProjectionPlatform ChooseFirstProjectionPlatform() => projectionPlatforms[0];

    private void CheckAllPlatformsDestroyed()
    {
        if (projectionPlatforms.Count != 0) return;

        OnBossDestroyAllProjectionPlatforms?.Invoke(this, EventArgs.Empty);
    }


    private void ResetTimer() => timer = timeToDestroyPlatform;

    #region BossStateHandler Subscriptions

    private void BossStateHandler_OnBossActiveStart(object sender, EventArgs e)
    {
        if (!resetTimerOnPhaseChange) return;
        ResetTimer();
    }

    private void BossStateHandler_OnBossPhaseChangeStart(object sender, EventArgs e)
    {
        if (!resetTimerOnPhaseChange) return;
        ResetTimer();
    }
    private void BossStateHandler_OnBossDefeated(object sender, EventArgs e)
    {
        ResetTimer();
    }
    private void BossStateHandler_OnPlayerDefeated(object sender, EventArgs e)
    {
        ResetTimer();
    }
    #endregion
}
