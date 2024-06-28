using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossSourceStun : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool enableSinceFirstPhase;
    [SerializeField] private int phaseNumberToEnable;
    [SerializeField] private float timeNotSunning;
    [SerializeField] private float timeStunning;

    [Header("Target Settings")]
    [SerializeField] private List<StuneableSource> stuneableSources;

    [Header("States")]
    [SerializeField] private State bossSourceStunState;

    [Header("Debug")]
    [SerializeField] private bool debug;
    [SerializeField] private float timer;
    [SerializeField] private StuneableSource currentSourceStunned;

    private enum State { Disabled, NotStunning, Stunning }

    public static event EventHandler<OnSourceStunnedEventArgs> OnSourceStunned;
    public static event EventHandler<OnSourceStunnedEventArgs> OnSourceUnStunned;

    public class OnSourceStunnedEventArgs : EventArgs
    {
        public ProjectionPlatform projectionPlatform;
        public float timeStunning;
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
        SetBossSourceStunState(State.Disabled);
    }

    private void Update()
    {
        HandleBossSourceStunState();
    }

    private void SetBossSourceStunState(State state) => bossSourceStunState = state;
    private void HandleBossSourceStunState()
    {
        switch (bossSourceStunState)
        {
            case State.Disabled:
                DisabledLogic();
                break;
            case State.NotStunning:
                NotTargetingLogic();
                break;
            case State.Stunning:
                TargetingLogic();
                break;
        }
    }
    private void DisabledLogic()
    {

    }
    private void NotTargetingLogic()
    {
        throw new NotImplementedException();
    }

    private void TargetingLogic()
    {
        throw new NotImplementedException();
    }

    private StuneableSource ChooseRandomStuneableSource(List<StuneableSource> stuneableSource)
    {
        if (stuneableSource.Count == 0) return null;

        int randomIndex = UnityEngine.Random.Range(0, stuneableSource.Count - 1);
        return stuneableSource[randomIndex];
    }

    private void ResetTimer() => timer = 0f;

    #region BossStateHandler Subscriptions
    private void BossStateHandler_OnBossActiveStart(object sender, EventArgs e)
    {
        ResetTimer();
        SetBossSourceStunState(State.Disabled);
    }
    private void BossStateHandler_OnBossActiveEnd(object sender, EventArgs e)
    {
        ResetTimer();
        if (enableSinceFirstPhase) SetBossSourceStunState(State.NotStunning);
    }

    private void BossStateHandler_OnBossPhaseChangeStart(object sender, BossStateHandler.OnPhaseChangeEventArgs e)
    {
        ResetTimer();
        SetBossSourceStunState(State.Disabled);
    }

    private void BossStateHandler_OnBossPhaseChangeEnd(object sender, BossStateHandler.OnPhaseChangeEventArgs e)
    {
        ResetTimer();
        if (e.phaseNumber >= phaseNumberToEnable) SetBossSourceStunState(State.NotStunning);
    }

    private void BossStateHandler_OnBossDefeated(object sender, EventArgs e)
    {
        ResetTimer();
        SetBossSourceStunState(State.Disabled);
    }
    private void BossStateHandler_OnPlayerDefeated(object sender, EventArgs e)
    {
        ResetTimer();
        SetBossSourceStunState(State.Disabled);
    }
    #endregion
}
