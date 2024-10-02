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
    [SerializeField] private float timePrePhaseChange;
    [SerializeField] private float timePostPhaseChange;

    [Header("Booleans")]
    [SerializeField] private bool bossDefeated;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public enum State {PhaseChange, OnPhase, BossDefeated, PlayerDefeated }
    public State BossState => state;
    public bool BossDefeated => bossDefeated;

    public static event EventHandler<OnPhaseChangeEventArgs> OnBossPhaseChangeStart;
    public static event EventHandler<OnPhaseChangeEventArgs> OnBossPhaseChangeMid;
    public static event EventHandler<OnPhaseChangeEventArgs> OnBossPhaseChangeEnd;
    public static event EventHandler OnBossDefeated;

    public class OnPhaseChangeEventArgs : EventArgs
    {
        public BossPhase currentPhase;
        public BossPhase nextPhase;
    }

    private void OnEnable()
    {
        BossPhaseHandler.OnPhaseCompleated += BossPhaseHandler_OnPhaseCompleated;
        BossPhaseHandler.OnLastPhaseCompleated += BossPhaseHandler_OnLastPhaseCompleated;
    }

    private void OnDisable()
    {
        BossPhaseHandler.OnPhaseCompleated -= BossPhaseHandler_OnPhaseCompleated;
        BossPhaseHandler.OnLastPhaseCompleated -= BossPhaseHandler_OnLastPhaseCompleated;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        SetBossDefeated(false);
        SetBossState(State.OnPhase);
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

    private void SetBossState(State state) => this.state = state;

    private IEnumerator ChangePhaseCoroutine(BossPhase currentPhase, BossPhase nextPhase)
    {
        SetBossState(State.PhaseChange);

        OnBossPhaseChangeStart?.Invoke(this, new OnPhaseChangeEventArgs { currentPhase = currentPhase, nextPhase = nextPhase });

        yield return new WaitForSeconds(timePrePhaseChange);

        OnBossPhaseChangeMid?.Invoke(this, new OnPhaseChangeEventArgs { currentPhase = currentPhase, nextPhase = nextPhase });

        yield return new WaitForSeconds(timePostPhaseChange);

        OnBossPhaseChangeEnd?.Invoke(this, new OnPhaseChangeEventArgs { currentPhase = currentPhase, nextPhase = nextPhase });

        SetBossState(State.OnPhase);
    }

    private void DefeatBoss()
    {
        SetBossState(State.BossDefeated);
        SetBossDefeated(true);

        OnBossDefeated?.Invoke(this, EventArgs.Empty);

        if (debug) Debug.Log("Boss Defeated");
    }

    private bool SetBossDefeated(bool defeated) => bossDefeated = defeated;

    #region BossPhaseHandler Subscriptions
    private void BossPhaseHandler_OnPhaseCompleated(object sender, BossPhaseHandler.OnPhaseEventArgs e)
    {
        if (state != State.OnPhase) return;
        StartCoroutine(ChangePhaseCoroutine(e.currentPhase,e.nextPhase));
    }

    private void BossPhaseHandler_OnLastPhaseCompleated(object sender, EventArgs e)
    {
        DefeatBoss();
    }
    #endregion

}
