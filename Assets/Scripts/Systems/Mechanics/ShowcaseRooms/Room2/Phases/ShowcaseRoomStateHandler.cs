using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowcaseRoomStateHandler : MonoBehaviour
{
    public static ShowcaseRoomStateHandler Instance { get; private set; }

    [Header("States")]
    [SerializeField] private State state;

    [Header("Settings")]
    [SerializeField] private float timePrePhaseChange;
    [SerializeField] private float timePostPhaseChange;

    [Header("Booleans")]
    [SerializeField] private bool showcaseRoomDefeated;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public enum State { PhaseChange, OnPhase, Defeated }

    public State ShowcaseRoomState => state;
    public bool ShowcaseRoomDefeated => showcaseRoomDefeated;

    private const float SHOWCASE_ROOM_PHASE_CHANGE_TIME_A = 0.5f;
    private const float SHOWCASE_ROOM_PHASE_CHANGE_TIME_B = 1f;

    public static event EventHandler<OnPhaseChangeEventArgs> OnShowcaseRoomPhaseChangeStart;
    public static event EventHandler<OnPhaseChangeEventArgs> OnShowcaseRoomPhaseChangeMidA;
    public static event EventHandler<OnPhaseChangeEventArgs> OnShowcaseRoomPhaseChangeMidB;
    public static event EventHandler<OnPhaseChangeEventArgs> OnShowcaseRoomPhaseChangeMidC;
    public static event EventHandler<OnPhaseChangeEventArgs> OnShowcaseRoomPhaseChangeEnd;
    public static event EventHandler OnShowcaseRoomAlmostDefeated;
    public static event EventHandler OnShowcaseRoomDefeated;

    public static event EventHandler<OnPhaseChangeEventArgs> OnShowcaseRoomPhaseChangePreMid;
    public static event EventHandler<OnPhaseChangeEventArgs> OnShowcaseRoomPhaseChangePostMid;

    public class OnPhaseChangeEventArgs : EventArgs
    {
        public ShowcaseRoomPhase currentPhase;
        public ShowcaseRoomPhase nextPhase;
    }

    private void OnEnable()
    {
        //BossPhaseHandler.OnPhaseCompleated += BossPhaseHandler_OnPhaseCompleated;
        //BossPhaseHandler.OnLastPhaseCompleated += BossPhaseHandler_OnLastPhaseCompleated;
        //BossPhaseHandler.OnAlmostDefeatedPhaseCompleated += BossPhaseHandler_OnAlmostDefeatedPhaseCompleated;
    }

    private void OnDisable()
    {
        //BossPhaseHandler.OnPhaseCompleated -= BossPhaseHandler_OnPhaseCompleated;
        //BossPhaseHandler.OnLastPhaseCompleated -= BossPhaseHandler_OnLastPhaseCompleated;
        //BossPhaseHandler.OnAlmostDefeatedPhaseCompleated -= BossPhaseHandler_OnAlmostDefeatedPhaseCompleated;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        SetShowcaseRoomDefeated(false);
        SetShowcaseRoomState(State.OnPhase);
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

    private void SetShowcaseRoomState(State state) => this.state = state;

    private IEnumerator ChangePhaseCoroutine(ShowcaseRoomPhase currentPhase, ShowcaseRoomPhase nextPhase)
    {
        SetShowcaseRoomState(State.PhaseChange);

        OnShowcaseRoomPhaseChangeStart?.Invoke(this, new OnPhaseChangeEventArgs { currentPhase = currentPhase, nextPhase = nextPhase });

        yield return new WaitForSeconds(timePrePhaseChange);

        OnShowcaseRoomPhaseChangePreMid?.Invoke(this, new OnPhaseChangeEventArgs { currentPhase = currentPhase, nextPhase = nextPhase });

        yield return new WaitForSeconds(SHOWCASE_ROOM_PHASE_CHANGE_TIME_A);

        OnShowcaseRoomPhaseChangeMidA?.Invoke(this, new OnPhaseChangeEventArgs { currentPhase = currentPhase, nextPhase = nextPhase });
        OnShowcaseRoomPhaseChangeMidB?.Invoke(this, new OnPhaseChangeEventArgs { currentPhase = currentPhase, nextPhase = nextPhase });
        OnShowcaseRoomPhaseChangeMidC?.Invoke(this, new OnPhaseChangeEventArgs { currentPhase = currentPhase, nextPhase = nextPhase });

        yield return new WaitForSeconds(SHOWCASE_ROOM_PHASE_CHANGE_TIME_B);

        OnShowcaseRoomPhaseChangePostMid?.Invoke(this, new OnPhaseChangeEventArgs { currentPhase = currentPhase, nextPhase = nextPhase });

        yield return new WaitForSeconds(timePostPhaseChange);

        OnShowcaseRoomPhaseChangeEnd?.Invoke(this, new OnPhaseChangeEventArgs { currentPhase = currentPhase, nextPhase = nextPhase });

        SetShowcaseRoomState(State.OnPhase);
    }

    private void DefeatBoss()
    {
        SetShowcaseRoomState(State.Defeated);
        SetShowcaseRoomDefeated(true);

        OnShowcaseRoomDefeated?.Invoke(this, EventArgs.Empty);

        if (debug) Debug.Log("Showcase Room Defeated");
    }


    private bool SetShowcaseRoomDefeated(bool defeated) => showcaseRoomDefeated = defeated;
}
