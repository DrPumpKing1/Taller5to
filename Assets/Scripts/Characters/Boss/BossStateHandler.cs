using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossStateHandler : MonoBehaviour
{
    public static BossStateHandler Instance;

    [Header("States")]
    [SerializeField] private State state;

    [Header("Settings")]
    [SerializeField] private float timeActivating;
    [SerializeField] private float timeChangingPhase;

    [Header("Debug")]
    [SerializeField] private bool debug;
    [SerializeField] private bool bossDefeated;
    [SerializeField] private bool playerDefeated;

    public enum State { Rest, Activating, PhaseChange, OnPhase, BossDefeated, PlayerDefeated}
    public State BossState => state;
    public bool BossDefeated => bossDefeated;
    public bool PlayerDefeated => playerDefeated;

    public static event EventHandler OnBossActiveStart;
    public static event EventHandler OnBossActiveEnd;

    public static event EventHandler<OnPhaseChangeEventArgs> OnBossPhaseChangeStart;
    public static event EventHandler<OnPhaseChangeEventArgs> OnBossPhaseChangeEnd;
    public static event EventHandler OnBossDefeated;
    public static event EventHandler OnPlayerDefeated;

    public class OnPhaseChangeEventArgs : EventArgs
    {
        public int phaseNumber;
    }

    private void OnEnable()
    {
        BossPhaseHandler.OnFirstPhaseStart += BossPhaseHandler_OnFirstPhaseStart;
        BossPhaseHandler.OnPhaseChange += BossPhaseHandler_OnPhaseChange;
        BossPhaseHandler.OnLastPhaseEnded += BossPhaseHandler_OnLastPhaseEnded;

        BossObjectDestruction.OnBossAllProjectionGemsLocked += BossPlatformDestruction_OnBossDestroyedAllObjects;
    }

    private void OnDisable()
    {
        BossPhaseHandler.OnFirstPhaseStart -= BossPhaseHandler_OnFirstPhaseStart;
        BossPhaseHandler.OnPhaseChange -= BossPhaseHandler_OnPhaseChange;
        BossPhaseHandler.OnLastPhaseEnded -= BossPhaseHandler_OnLastPhaseEnded;

        BossObjectDestruction.OnBossAllProjectionGemsLocked -= BossPlatformDestruction_OnBossDestroyedAllObjects;
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

    private IEnumerator ActivateBossCoroutine()
    {
        SetBossState(State.Activating);

        OnBossActiveStart?.Invoke(this, EventArgs.Empty);

        yield return new WaitForSeconds(timeActivating);

        OnBossActiveEnd?.Invoke(this, EventArgs.Empty);

        SetBossState(State.OnPhase);
    }

    private IEnumerator ChangePhaseCoroutine(int phaseNumber)
    {
        SetBossState(State.PhaseChange);

        OnBossPhaseChangeStart?.Invoke(this, new OnPhaseChangeEventArgs { phaseNumber = phaseNumber });

        yield return new WaitForSeconds(timeChangingPhase);

        OnBossPhaseChangeEnd?.Invoke(this, new OnPhaseChangeEventArgs { phaseNumber = phaseNumber });

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

    #region BossPhaseHandlerSubscriptions
    private void BossPhaseHandler_OnFirstPhaseStart(object sender, EventArgs e)
    {
        StartCoroutine(ActivateBossCoroutine());
    }
    private void BossPhaseHandler_OnPhaseChange(object sender, BossPhaseHandler.OnPhaseChangeEventArgs e)
    {
        StartCoroutine(ChangePhaseCoroutine(e.newPhase));
    }

    private void BossPhaseHandler_OnLastPhaseEnded(object sender, EventArgs e)
    {
        DefeatBoss();
    }
    #endregion

    #region BossPlatformDestruction Subcriptions
    private void BossPlatformDestruction_OnBossDestroyedAllObjects(object sender, EventArgs e)
    {
        DefeatPlayer();
    }
    #endregion
}
