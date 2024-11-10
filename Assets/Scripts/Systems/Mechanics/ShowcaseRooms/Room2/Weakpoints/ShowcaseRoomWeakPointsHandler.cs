using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowcaseRoomWeakPointsHandler : MonoBehaviour
{
    public static ShowcaseRoomWeakPointsHandler Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private List<PhaseWeakPoints> regularPhaseWeakPointsList;

    [Header("Debug")]
    [SerializeField] private bool debug;

    [Serializable]
    public class PhaseWeakPoints
    {
        public ShowcaseRoomPhase showcaseRoomPhase;
        public List<ShowcaseRoomWeakpoint> weakPoints;
        public bool allHit;
    }

    public static event EventHandler<OnWeakPointsEventArgs> OnWeakPointsEnable;
    public static event EventHandler<OnWeakPointsEventArgs> OnWeakPointsDisable;

    public static event EventHandler<OnPhaseWeakPointsHitEventArgs> OnPhaseWeakPointsHit;

    private const ShowcaseRoomPhase FIRST_WEAK_POINTS_PHASE = ShowcaseRoomPhase.Phase1;


    public class OnWeakPointsEventArgs : EventArgs
    {
        public List<ShowcaseRoomWeakpoint> weakPoints;
    }
    public class OnPhaseWeakPointsHitEventArgs : EventArgs
    {
        public PhaseWeakPoints phaseWeakPoints;
    }

    private void OnEnable()
    {
        ShowcaseRoomPhaseHandler.OnPhaseCompleated += ShowcaseRoomPhaseHandler_OnPhaseCompleated;
        ShowcaseRoomStateHandler.OnShowcaseRoomPhaseChangeEnd += ShowcaseRoomStateHandler_OnShowcaseRoomPhaseChangeEnd;

        ShowcaseRoomPhaseHandler.OnLastPhaseCompleated += ShowcaseRoomPhaseHandler_OnLastPhaseCompleated;
    }

    private void OnDisable()
    {
        ShowcaseRoomPhaseHandler.OnPhaseCompleated -= ShowcaseRoomPhaseHandler_OnPhaseCompleated;
        ShowcaseRoomStateHandler.OnShowcaseRoomPhaseChangeEnd -= ShowcaseRoomStateHandler_OnShowcaseRoomPhaseChangeEnd;

        ShowcaseRoomPhaseHandler.OnLastPhaseCompleated -= ShowcaseRoomPhaseHandler_OnLastPhaseCompleated;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        DisableAllWeakPoints();
        EnableWeakPointsByPhaseChange(FIRST_WEAK_POINTS_PHASE);
    }

    private void Update()
    {
        HandlePhaseWeakPointsList();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one ShowcaseRoomWeakPointsHandler, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void HandlePhaseWeakPointsList()
    {
        foreach (PhaseWeakPoints phaseWeakPoints in regularPhaseWeakPointsList)
        {
            HandlePhaseWeakPoints(phaseWeakPoints);
        }
    }

    private void HandlePhaseWeakPoints(PhaseWeakPoints phaseWeakPoints)
    {
        if (phaseWeakPoints.allHit) return;
        if (phaseWeakPoints.weakPoints.Count <= 0) return;

        foreach (ShowcaseRoomWeakpoint showcaseRoomWeakpoint in phaseWeakPoints.weakPoints)
        {
            if (!showcaseRoomWeakpoint.IsHit) return;
        }

        phaseWeakPoints.allHit = true;
        OnPhaseWeakPointsHit?.Invoke(this, new OnPhaseWeakPointsHitEventArgs { phaseWeakPoints = phaseWeakPoints });

        if (debug) Debug.Log($"AllWeakPointsHit {phaseWeakPoints.showcaseRoomPhase}");
    }

    #region PhaseChange
    private void EnableWeakPointsByPhaseChange(ShowcaseRoomPhase showcaseRoomPhase)
    {
        foreach (PhaseWeakPoints phaseWeakPoints in regularPhaseWeakPointsList)
        {
            if (phaseWeakPoints.showcaseRoomPhase == showcaseRoomPhase)
            {
                OnWeakPointsEnable?.Invoke(this, new OnWeakPointsEventArgs { weakPoints = phaseWeakPoints.weakPoints });
            }
        }
    }

    private void DisableWeakPointsByPhaseChange(ShowcaseRoomPhase showcaseRoomPhase)
    {
        foreach (PhaseWeakPoints phaseWeakPoints in regularPhaseWeakPointsList)
        {
            if (phaseWeakPoints.showcaseRoomPhase == showcaseRoomPhase)
            {
                OnWeakPointsDisable?.Invoke(this, new OnWeakPointsEventArgs { weakPoints = phaseWeakPoints.weakPoints });
            }
        }
    }
    #endregion

    private void DisableAllWeakPoints()
    {
        foreach (PhaseWeakPoints phaseWeakPoints in regularPhaseWeakPointsList)
        {
            OnWeakPointsDisable?.Invoke(this, new OnWeakPointsEventArgs { weakPoints = phaseWeakPoints.weakPoints });
        }
    }

    #region ShowcaseRoomPhase&StateHandler Subscriptions
    private void ShowcaseRoomPhaseHandler_OnPhaseCompleated(object sender, ShowcaseRoomPhaseHandler.OnPhaseEventArgs e)
    {
        DisableWeakPointsByPhaseChange(e.currentPhase);
    }
    private void ShowcaseRoomStateHandler_OnShowcaseRoomPhaseChangeEnd(object sender, ShowcaseRoomStateHandler.OnPhaseChangeEventArgs e)
    {
        EnableWeakPointsByPhaseChange(e.nextPhase);
    }
    private void ShowcaseRoomPhaseHandler_OnLastPhaseCompleated(object sender, EventArgs e)
    {
        DisableAllWeakPoints();
    }
    #endregion
}
