using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateHandler : MonoBehaviour
{
    public static BossStateHandler Instance;

    [Header("States")]
    [SerializeField] private State state;

    [Header("Settings")]
    [SerializeField] private float timePreChangingPhase;
    [SerializeField] private float timePostChangingPhase;

    [Header("Booleans")]
    [SerializeField] private bool bossDefeated;
    [SerializeField] private bool playerDefeated;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public enum State { Rest, PhaseChange, OnPhase, BossDefeated, PlayerDefeated }
    public State BossState => state;
    public bool BossDefeated => bossDefeated;
    public bool PlayerDefeated => playerDefeated;

    public static event EventHandler<OnPhaseChangeEventArgs> OnBossPhaseChangeStart;
    public static event EventHandler<OnPhaseChangeEventArgs> OnBossPhaseChangeMid;
    public static event EventHandler<OnPhaseChangeEventArgs> OnBossPhaseChangeEnd;
    public static event EventHandler OnBossDefeated;
    public static event EventHandler OnPlayerDefeated;

    public class OnPhaseChangeEventArgs : EventArgs
    {
        public BossPhase nextPhase;
    }

    private void Awake()
    {
        SetSingleton();
        InitializeVariables();
    }

    private void Start()
    {
        SetBossState(State.Rest);
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one BossStateHandler instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeVariables()
    {
        bossDefeated = false;
        playerDefeated = false;
    }

    private void SetBossState(State state) => this.state = state;

    private IEnumerator ChangePhaseCoroutine(BossPhase nextPhase)
    {
        SetBossState(State.PhaseChange);

        OnBossPhaseChangeStart?.Invoke(this, new OnPhaseChangeEventArgs { nextPhase = nextPhase });

        yield return new WaitForSeconds(timePreChangingPhase);

        OnBossPhaseChangeMid?.Invoke(this, new OnPhaseChangeEventArgs { nextPhase = nextPhase });

        yield return new WaitForSeconds(timePostChangingPhase);

        OnBossPhaseChangeEnd?.Invoke(this, new OnPhaseChangeEventArgs { nextPhase = nextPhase });

        SetBossState(State.OnPhase);
    }

    private void DefeatBoss()
    {
        SetBossState(State.BossDefeated);
        bossDefeated = true;

        OnBossDefeated?.Invoke(this, EventArgs.Empty);

        if (debug) Debug.Log("Boss Defeated");
    }

    private void DefeatPlayer()
    {
        SetBossState(State.PlayerDefeated);
        playerDefeated = true;

        OnPlayerDefeated?.Invoke(this, EventArgs.Empty);

        if (debug) Debug.Log("Player Defeated");
    }

}
