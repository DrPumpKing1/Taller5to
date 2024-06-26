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

    public enum State { Rest, Activating, PhaseChange, OnPhase, BossDefeated, PlayerDefeated}
    public State BossState => state;

    public static event EventHandler OnBossActiveStart;
    public static event EventHandler OnBossActiveEnd;

    public static event EventHandler OnBossPhaseChangeStart;
    public static event EventHandler OnBossPhaseChangeEnd;
    public static event EventHandler OnBossDefeated;
    public static event EventHandler OnPlayerDefeated;

    private void OnEnable()
    {
        BossPhaseHandler.OnFirstPhaseStart += BossPhaseHandler_OnFirstPhaseStart;
        BossPhaseHandler.OnPhaseChange += BossPhaseHandler_OnPhaseChange;
        BossPhaseHandler.OnLastPhaseEnded += BossPhaseHandler_OnLastPhaseEnded;

        BossObjectDestruction.OnBossDestroyAllProjectionPlatforms += BossPlatformDestruction_OnBossDestroyAllProjectionPlatforms;
    }

    private void OnDisable()
    {
        BossPhaseHandler.OnFirstPhaseStart -= BossPhaseHandler_OnFirstPhaseStart;
        BossPhaseHandler.OnPhaseChange -= BossPhaseHandler_OnPhaseChange;
        BossPhaseHandler.OnLastPhaseEnded -= BossPhaseHandler_OnLastPhaseEnded;
    }

    private void Awake()
    {
        SetSingleton();
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

    private void SetBossState(State state) => this.state = state;


    private IEnumerator ActivateBossCoroutine()
    {
        SetBossState(State.Activating);

        OnBossActiveStart?.Invoke(this, EventArgs.Empty);

        yield return new WaitForSeconds(timeActivating);

        OnBossActiveEnd?.Invoke(this, EventArgs.Empty);

        SetBossState(State.OnPhase);
    }

    private IEnumerator ChangePhaseCoroutine()
    {
        SetBossState(State.PhaseChange);

        OnBossPhaseChangeStart?.Invoke(this, EventArgs.Empty);

        yield return new WaitForSeconds(timeChangingPhase);

        OnBossPhaseChangeEnd?.Invoke(this, EventArgs.Empty);

        SetBossState(State.OnPhase);
    }

    private void DefeatBoss()
    {
        SetBossState(State.BossDefeated);
        OnBossDefeated?.Invoke(this, EventArgs.Empty);

        if (debug) Debug.Log("Boss Defeated");
    }

    private void DefeatPlayer()
    {
        SetBossState(State.PlayerDefeated);
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
        StartCoroutine(ChangePhaseCoroutine());
    }

    private void BossPhaseHandler_OnLastPhaseEnded(object sender, EventArgs e)
    {
        DefeatBoss();
    }
    #endregion

    #region BossPlatformDestruction Subriptions
    private void BossPlatformDestruction_OnBossDestroyAllProjectionPlatforms(object sender, EventArgs e)
    {
        DefeatPlayer();
    }
    #endregion
}
