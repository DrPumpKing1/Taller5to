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

    [Header("Player Instant Position")]
    [SerializeField] private Transform playerInstantPositionTransform;
    [SerializeField] private Vector2 playerInstantDirection;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public enum State {PhaseChange, OnPhase, AlmostDefeated, Defeated }
    public State BossState => state;
    public bool BossDefeated => bossDefeated;

    private const float BOSS_PHASE_CHANGE_TIME_A = 0.5f;
    private const float BOSS_PHASE_CHANGE_TIME_B = 1f;

    public static event EventHandler<OnPhaseChangeEventArgs> OnBossPhaseChangeStart;
    public static event EventHandler<OnPhaseChangeEventArgs> OnBossPhaseChangeMidA;
    public static event EventHandler<OnPhaseChangeEventArgs> OnBossPhaseChangeMidB;
    public static event EventHandler<OnPhaseChangeEventArgs> OnBossPhaseChangeMidC;
    public static event EventHandler<OnPhaseChangeEventArgs> OnBossPhaseChangeEnd;
    public static event EventHandler OnBossAlmostDefeated;
    public static event EventHandler OnBossDefeated;

    public static event EventHandler<OnPhaseChangeEventArgs> OnBossPhaseChangePreMid;
    public static event EventHandler<OnPhaseChangeEventArgs> OnBossPhaseChangePostMid;

    public class OnPhaseChangeEventArgs : EventArgs
    {
        public BossPhase currentPhase;
        public BossPhase nextPhase;
    }

    private void OnEnable()
    {
        BossPhaseHandler.OnPhaseCompleated += BossPhaseHandler_OnPhaseCompleated;
        BossPhaseHandler.OnLastPhaseCompleated += BossPhaseHandler_OnLastPhaseCompleated;
        BossPhaseHandler.OnAlmostDefeatedPhaseCompleated += BossPhaseHandler_OnAlmostDefeatedPhaseCompleated;
    }

    private void OnDisable()
    {
        BossPhaseHandler.OnPhaseCompleated -= BossPhaseHandler_OnPhaseCompleated;
        BossPhaseHandler.OnLastPhaseCompleated -= BossPhaseHandler_OnLastPhaseCompleated;
        BossPhaseHandler.OnAlmostDefeatedPhaseCompleated -= BossPhaseHandler_OnAlmostDefeatedPhaseCompleated;
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

        OnBossPhaseChangePreMid?.Invoke(this, new OnPhaseChangeEventArgs { currentPhase = currentPhase, nextPhase = nextPhase });

        yield return new WaitForSeconds(BOSS_PHASE_CHANGE_TIME_A);

        InstantPositionPlayer();

        OnBossPhaseChangeMidA?.Invoke(this, new OnPhaseChangeEventArgs { currentPhase = currentPhase, nextPhase = nextPhase });
        OnBossPhaseChangeMidB?.Invoke(this, new OnPhaseChangeEventArgs { currentPhase = currentPhase, nextPhase = nextPhase });
        OnBossPhaseChangeMidC?.Invoke(this, new OnPhaseChangeEventArgs { currentPhase = currentPhase, nextPhase = nextPhase });

        yield return new WaitForSeconds(BOSS_PHASE_CHANGE_TIME_B);

        OnBossPhaseChangePostMid?.Invoke(this, new OnPhaseChangeEventArgs { currentPhase = currentPhase, nextPhase = nextPhase });

        yield return new WaitForSeconds(timePostPhaseChange);

        OnBossPhaseChangeEnd?.Invoke(this, new OnPhaseChangeEventArgs { currentPhase = currentPhase, nextPhase = nextPhase });

        SetBossState(State.OnPhase);
    }
    private void AlmostDefeatBoss()
    {
        SetBossState(State.AlmostDefeated);

        OnBossAlmostDefeated?.Invoke(this, EventArgs.Empty);

        if (debug) Debug.Log("Boss Almost Defeated");
    }

    private void DefeatBoss()
    {
        SetBossState(State.Defeated);
        SetBossDefeated(true);

        OnBossDefeated?.Invoke(this, EventArgs.Empty);

        if (debug) Debug.Log("Boss Defeated");
    }


    private bool SetBossDefeated(bool defeated) => bossDefeated = defeated;

    private void InstantPositionPlayer()
    {
        PlayerPositioningHandler.Instance.InstantPositionPlayer(playerInstantPositionTransform.position);
        PlayerDirectionHandler.Instance.InstantDirectionPlayer(playerInstantDirection);
    }

    #region BossPhaseHandler Subscriptions
    private void BossPhaseHandler_OnPhaseCompleated(object sender, BossPhaseHandler.OnPhaseEventArgs e)
    {
        if (state != State.OnPhase) return;
        StartCoroutine(ChangePhaseCoroutine(e.currentPhase,e.nextPhase));
    }

    private void BossPhaseHandler_OnLastPhaseCompleated(object sender, EventArgs e)
    {
        AlmostDefeatBoss();
    }

    private void BossPhaseHandler_OnAlmostDefeatedPhaseCompleated(object sender, EventArgs e)
    {
        DefeatBoss();
    }

    #endregion

}
