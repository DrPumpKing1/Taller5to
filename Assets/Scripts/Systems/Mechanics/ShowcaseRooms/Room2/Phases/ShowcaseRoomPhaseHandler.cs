using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowcaseRoomPhaseHandler : MonoBehaviour
{
    public static ShowcaseRoomPhaseHandler Instance { get; private set; }

    [Header("Phases")]
    [SerializeField] ShowcaseRoomPhase currentPhase;

    [Header("Booleans")]
    [SerializeField] private bool isDefeated;

    private const ShowcaseRoomPhase FIRST_PHASE = ShowcaseRoomPhase.Phase1;
    private const ShowcaseRoomPhase LAST_PHASE = ShowcaseRoomPhase.Phase3;
    private const ShowcaseRoomPhase DEFEATED_PHASE = ShowcaseRoomPhase.Defeated;

    public static event EventHandler<OnPhaseEventArgs> OnPhaseCompleated;
    public static event EventHandler OnLastPhaseCompleated;

    private GameObject player;//
    private const string PLAYER_TAG = "Player";//
    private const float PLAYER_DISTANCE_TO_FORCE_PHASE_CHANGE = 30f;//

    public class OnPhaseEventArgs : EventArgs
    {
        public ShowcaseRoomPhase currentPhase;
        public ShowcaseRoomPhase nextPhase;
    }

    private void OnEnable()
    {
        ShowcaseRoomStateHandler.OnShowcaseRoomPhaseChangeMidA += ShowcaseRoomStateHandler_OnShowcaseRoomPhaseChangeMidA;
        ShowcaseRoomStateHandler.OnShowcaseRoomDefeated += ShowcaseRoomStateHandler_OnShowcaseRoomDefeated;

        ShowcaseRoomWeakPointsHandler.OnPhaseWeakPointsHit += ShowcaseRoomWeakPointsHandler_OnPhaseWeakPointsHit;
    }

    private void OnDisable()
    {
        ShowcaseRoomStateHandler.OnShowcaseRoomPhaseChangeMidA -= ShowcaseRoomStateHandler_OnShowcaseRoomPhaseChangeMidA;
        ShowcaseRoomStateHandler.OnShowcaseRoomDefeated -= ShowcaseRoomStateHandler_OnShowcaseRoomDefeated;

        ShowcaseRoomWeakPointsHandler.OnPhaseWeakPointsHit -= ShowcaseRoomWeakPointsHandler_OnPhaseWeakPointsHit;
    }

    private void Awake()
    {
        SetSingleton();
        player = GameObject.FindGameObjectWithTag(PLAYER_TAG); //
    }

    private void Start()
    {
        SetDefeated(false);
        SetCurrentPhase(FIRST_PHASE);
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one ShowcaseRoomPhaseHandler, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void SetCurrentPhase(ShowcaseRoomPhase phase) => currentPhase = phase;

    private ShowcaseRoomPhase GetNextPhase(ShowcaseRoomPhase currentPhase)
    {
        switch (currentPhase)
        {
            case ShowcaseRoomPhase.Phase1:
                return ShowcaseRoomPhase.Phase2;
            case ShowcaseRoomPhase.Phase2:
                return ShowcaseRoomPhase.Phase3;
            case ShowcaseRoomPhase.Phase3:
            default:
                return ShowcaseRoomPhase.Defeated;
        }
    }

    public void ForceChangeToNextPhase()
    {
        if (!CheckPlayerClose()) return;
        ChangeToNextPhase();
    }

    private void ChangeToNextPhase()
    {
        if (ShowcaseRoomStateHandler.Instance.ShowcaseRoomState == ShowcaseRoomStateHandler.State.PhaseChange) return;
        if (ShowcaseRoomStateHandler.Instance.ShowcaseRoomState == ShowcaseRoomStateHandler.State.Defeated) return;
        if (isDefeated) return;

        if (currentPhase == LAST_PHASE)
        {
            OnLastPhaseCompleated?.Invoke(this, EventArgs.Empty);
            SetCurrentPhase(DEFEATED_PHASE);
            return;
        }

        ShowcaseRoomPhase nextPhase = GetNextPhase(currentPhase);
        OnPhaseCompleated?.Invoke(this, new OnPhaseEventArgs { currentPhase = currentPhase, nextPhase = nextPhase });
    }

    private void SetDefeated(bool defeated) => isDefeated = defeated;

    //
    private bool CheckPlayerClose()
    {
        if (!player) return true;
        if (Vector3.Distance(transform.position, player.transform.position) <= PLAYER_DISTANCE_TO_FORCE_PHASE_CHANGE) return true;

        return false;
    }

    #region ShowcaseRoomStateHandler Subscriptions

    private void ShowcaseRoomStateHandler_OnShowcaseRoomPhaseChangeMidA(object sender, ShowcaseRoomStateHandler.OnPhaseChangeEventArgs e)
    {
        SetCurrentPhase(e.nextPhase);
    }

    private void ShowcaseRoomStateHandler_OnShowcaseRoomDefeated(object sender, EventArgs e) => SetDefeated(true);
    #endregion

    #region ShowcaseRoomWeakpointsHandler Subscriptions
    private void ShowcaseRoomWeakPointsHandler_OnPhaseWeakPointsHit(object sender, ShowcaseRoomWeakPointsHandler.OnPhaseWeakPointsHitEventArgs e)
    {
        if (currentPhase != e.phaseWeakPoints.showcaseRoomPhase) return;
        ChangeToNextPhase();
    }
    #endregion
}
