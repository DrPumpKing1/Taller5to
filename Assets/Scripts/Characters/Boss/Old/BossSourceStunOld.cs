using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossSourceStunOld : MonoBehaviour
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
    public static event EventHandler<OnSourceStunnedEventArgs> OnSourceStunnedEnd;

    public class OnSourceStunnedEventArgs : EventArgs
    {
        public StuneableSource stuneableSource;
        public float timeStunning;
    }

    private void OnEnable()
    {
        BossStateHandlerOld.OnBossActiveStart += BossStateHandler_OnBossActiveStart;
        BossStateHandlerOld.OnBossActiveEnd += BossStateHandler_OnBossActiveEnd;

        BossStateHandlerOld.OnBossPhaseChangeStart += BossStateHandler_OnBossPhaseChangeStart;
        BossStateHandlerOld.OnBossPhaseChangeEnd += BossStateHandler_OnBossPhaseChangeEnd;

        BossStateHandlerOld.OnBossDefeated += BossStateHandler_OnBossDefeated;
        BossStateHandlerOld.OnPlayerDefeated += BossStateHandler_OnPlayerDefeated;
    }


    private void OnDisable()
    {
        BossStateHandlerOld.OnBossActiveStart -= BossStateHandler_OnBossActiveStart;
        BossStateHandlerOld.OnBossActiveEnd -= BossStateHandler_OnBossActiveEnd;

        BossStateHandlerOld.OnBossPhaseChangeStart -= BossStateHandler_OnBossPhaseChangeStart;
        BossStateHandlerOld.OnBossPhaseChangeEnd -= BossStateHandler_OnBossPhaseChangeEnd;

        BossStateHandlerOld.OnBossDefeated -= BossStateHandler_OnBossDefeated;
        BossStateHandlerOld.OnPlayerDefeated -= BossStateHandler_OnPlayerDefeated;
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
                NotStunningLogic();
                break;
            case State.Stunning:
                StunningLogic();
                break;
        }
    }
    private void DisabledLogic()
    {
        if (currentSourceStunned)
        {
            EndStuneableSourceStun();
            return;
        }
    }

    private void NotStunningLogic()
    {
        if (timer >= timeNotSunning)
        {
            SetBossSourceStunState(State.Stunning);

            StunStuneableSource();
            ResetTimer();
            return;
        }

        timer += Time.deltaTime;
    }

    private void StunningLogic()
    {
        if (timer >= timeStunning)
        {
            SetBossSourceStunState(State.NotStunning);

            EndStuneableSourceStun();
            ResetTimer();
            return;
        }

        timer += Time.deltaTime;
    }

    private void StunStuneableSource()
    {
        StuneableSource stunebleSourceToStun = ChooseRandomStuneableSource(stuneableSources);

        if (!stunebleSourceToStun)
        {
            if (debug) Debug.Log("There are any stuneable sources to stun");
            SetBossSourceStunState(State.NotStunning);
            return;
        }

        stunebleSourceToStun.StunSource();

        currentSourceStunned = stunebleSourceToStun;
        if (debug) Debug.Log($"{stunebleSourceToStun} stunned");

        OnSourceStunned?.Invoke(this, new OnSourceStunnedEventArgs { stuneableSource = currentSourceStunned, timeStunning = timeStunning });
    }

    private void EndStuneableSourceStun()
    {
        if (!currentSourceStunned) return;

        currentSourceStunned.EndStun();
        OnSourceStunnedEnd?.Invoke(this, new OnSourceStunnedEventArgs { stuneableSource = currentSourceStunned, timeStunning = timeStunning });

        if (debug) Debug.Log("Source stun ended");

        currentSourceStunned = null;
    }

    private StuneableSource ChooseRandomStuneableSource(List<StuneableSource> stuneableSource)
    {
        if (stuneableSource.Count == 0) return null;

        int randomIndex = UnityEngine.Random.Range(0, stuneableSource.Count);
        return stuneableSource[randomIndex];
    }

    private void ResetTimer() => timer = 0f;

    #region BossStateHandler Subscriptions
    private void BossStateHandler_OnBossActiveStart(object sender, EventArgs e)
    {
        ResetTimer();
        SetBossSourceStunState(State.Disabled);
        EndStuneableSourceStun();
    }
    private void BossStateHandler_OnBossActiveEnd(object sender, EventArgs e)
    {
        ResetTimer();
        if (enableSinceFirstPhase) SetBossSourceStunState(State.NotStunning);
        EndStuneableSourceStun();
    }

    private void BossStateHandler_OnBossPhaseChangeStart(object sender, BossStateHandlerOld.OnPhaseChangeEventArgs e)
    {
        ResetTimer();
        SetBossSourceStunState(State.Disabled);
        EndStuneableSourceStun();
    }

    private void BossStateHandler_OnBossPhaseChangeEnd(object sender, BossStateHandlerOld.OnPhaseChangeEventArgs e)
    {
        ResetTimer();
        if (e.phaseNumber >= phaseNumberToEnable) SetBossSourceStunState(State.NotStunning);
        EndStuneableSourceStun();
    }

    private void BossStateHandler_OnBossDefeated(object sender, EventArgs e)
    {
        ResetTimer();
        SetBossSourceStunState(State.Disabled);
        EndStuneableSourceStun();
    }
    private void BossStateHandler_OnPlayerDefeated(object sender, EventArgs e)
    {
        ResetTimer();
        SetBossSourceStunState(State.Disabled);
        EndStuneableSourceStun();
    }
    #endregion
}
