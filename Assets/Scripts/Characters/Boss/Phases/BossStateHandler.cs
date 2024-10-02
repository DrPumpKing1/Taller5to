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

    [Header("Debug")]
    [SerializeField] private bool debug;

    public enum State { Rest, PhaseChange, OnPhase, BossDefeated, PlayerDefeated }
    public State BossState => state;
    public bool BossDefeated => bossDefeated;

    public static event EventHandler<OnPhaseChangeEventArgs> OnBossPhaseChangeStart;
    public static event EventHandler<OnPhaseChangeEventArgs> OnBossPhaseChangeMid;
    public static event EventHandler<OnPhaseChangeEventArgs> OnBossPhaseChangeEnd;
    public static event EventHandler OnBossDefeated;

    public class OnPhaseChangeEventArgs : EventArgs
    {
        public BossPhase nextPhase;
    }

    private void OnEnable()
    {
        BossPhaseHandler.OnPhaseChange += BossPhaseHandler_OnPhaseChange;
        BossPhaseHandler.OnLastPhaseEnded += BossPhaseHandler_OnLastPhaseEnded;
    }

    private void OnDisable()
    {
        BossPhaseHandler.OnPhaseChange -= BossPhaseHandler_OnPhaseChange;
        BossPhaseHandler.OnLastPhaseEnded -= BossPhaseHandler_OnLastPhaseEnded;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        SetBossDefeated(false);
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
        SetBossDefeated(true);

        OnBossDefeated?.Invoke(this, EventArgs.Empty);

        if (debug) Debug.Log("Boss Defeated");
    }

    private bool SetBossDefeated(bool defeated) => bossDefeated = defeated;

    #region BossPhaseHandler Subscriptions
    private void BossPhaseHandler_OnPhaseChange(object sender, BossPhaseHandler.OnPhaseChangeEventArgs e)
    {
        StartCoroutine(ChangePhaseCoroutine(e.nextPhase));
    }

    private void BossPhaseHandler_OnLastPhaseEnded(object sender, EventArgs e)
    {
        DefeatBoss();
    }
    #endregion

}
