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

    public static event EventHandler<OnWeakPointsEventArgs> OnWeakPointsEnableA;
    public static event EventHandler<OnWeakPointsEventArgs> OnWeakPointsEnableB;
    public static event EventHandler<OnWeakPointsEventArgs> OnWeakPointsDisableA;
    public static event EventHandler<OnWeakPointsEventArgs> OnWeakPointsDisableB;

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
        ShowcaseRoomStateHandler.OnShowcaseRoomPhaseChangeEnd += ShowcaseRoomStateHandler_OnShowcaseRoomPhaseChangeEnd;
        ShowcaseRoomPhaseHandler.OnPhaseCompleated += ShowcaseRoomPhaseHandler_OnPhaseCompleated;
        ShowcaseRoomStateHandler.OnShowcaseRoomPhaseChangeMidC += ShowcaseRoomStateHandler_OnShowcaseRoomPhaseChangeMidC;

        ShowcaseRoomPhaseHandler.OnLastPhaseCompleated += ShowcaseRoomPhaseHandler_OnLastPhaseCompleated;
    }


    private void OnDisable()
    {
        ShowcaseRoomStateHandler.OnShowcaseRoomPhaseChangeEnd -= ShowcaseRoomStateHandler_OnShowcaseRoomPhaseChangeEnd;
        ShowcaseRoomPhaseHandler.OnPhaseCompleated -= ShowcaseRoomPhaseHandler_OnPhaseCompleated;
        ShowcaseRoomStateHandler.OnShowcaseRoomPhaseChangeMidC -= ShowcaseRoomStateHandler_OnShowcaseRoomPhaseChangeMidC;

        ShowcaseRoomPhaseHandler.OnLastPhaseCompleated -= ShowcaseRoomPhaseHandler_OnLastPhaseCompleated;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        DisableAllWeakPoints();
        TotalEnableWeakPointsByPhase(FIRST_WEAK_POINTS_PHASE);
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
                OnWeakPointsEnableA?.Invoke(this, new OnWeakPointsEventArgs { weakPoints = phaseWeakPoints.weakPoints });
            }
        }
    }

    private void EnableWeakPointsByPhaseChangeCompleated(ShowcaseRoomPhase showcaseRoomPhase)
    {
        foreach (PhaseWeakPoints phaseWeakPoints in regularPhaseWeakPointsList)
        {
            if (phaseWeakPoints.showcaseRoomPhase == showcaseRoomPhase)
            {
                OnWeakPointsEnableB?.Invoke(this, new OnWeakPointsEventArgs { weakPoints = phaseWeakPoints.weakPoints });
            }
        }
    }

    private void DisableWeakPointsByPhaseCompleated(ShowcaseRoomPhase showcaseRoomPhase)
    {
        foreach (PhaseWeakPoints phaseWeakPoints in regularPhaseWeakPointsList)
        {
            if (phaseWeakPoints.showcaseRoomPhase == showcaseRoomPhase)
            {
                OnWeakPointsDisableA?.Invoke(this, new OnWeakPointsEventArgs { weakPoints = phaseWeakPoints.weakPoints });
            }
        }
    }

    private void DisableWeakPointsByPhaseChange(ShowcaseRoomPhase showcaseRoomPhase)
    {
        foreach (PhaseWeakPoints phaseWeakPoints in regularPhaseWeakPointsList)
        {
            if (phaseWeakPoints.showcaseRoomPhase == showcaseRoomPhase)
            {
                OnWeakPointsDisableB?.Invoke(this, new OnWeakPointsEventArgs { weakPoints = phaseWeakPoints.weakPoints });
            }
        }
    }
    #endregion
    private void TotalEnableWeakPointsByPhase(ShowcaseRoomPhase showcaseRoomPhase)
    {
        foreach (PhaseWeakPoints phaseWeakPoints in regularPhaseWeakPointsList)
        {
            if (phaseWeakPoints.showcaseRoomPhase == showcaseRoomPhase)
            {
                OnWeakPointsEnableA?.Invoke(this, new OnWeakPointsEventArgs { weakPoints = phaseWeakPoints.weakPoints });
                OnWeakPointsEnableB?.Invoke(this, new OnWeakPointsEventArgs { weakPoints = phaseWeakPoints.weakPoints });
            }
        }
    }

    private void DisableAllWeakPoints()
    {
        foreach (PhaseWeakPoints phaseWeakPoints in regularPhaseWeakPointsList)
        {
            OnWeakPointsDisableA?.Invoke(this, new OnWeakPointsEventArgs { weakPoints = phaseWeakPoints.weakPoints });
            OnWeakPointsDisableB?.Invoke(this, new OnWeakPointsEventArgs { weakPoints = phaseWeakPoints.weakPoints });
        }
    }

    #region ShowcaseRoomPhase&StateHandler Subscriptions
    private void ShowcaseRoomStateHandler_OnShowcaseRoomPhaseChangeEnd(object sender, ShowcaseRoomStateHandler.OnPhaseChangeEventArgs e)
    {
        EnableWeakPointsByPhaseChangeCompleated(e.nextPhase);
    }
    private void ShowcaseRoomPhaseHandler_OnPhaseCompleated(object sender, ShowcaseRoomPhaseHandler.OnPhaseEventArgs e)
    {
        DisableWeakPointsByPhaseCompleated(e.currentPhase);
    }
    private void ShowcaseRoomStateHandler_OnShowcaseRoomPhaseChangeMidC(object sender, ShowcaseRoomStateHandler.OnPhaseChangeEventArgs e)
    {
        DisableWeakPointsByPhaseChange(e.currentPhase);
        EnableWeakPointsByPhaseChange(e.nextPhase);
    }
    private void ShowcaseRoomPhaseHandler_OnLastPhaseCompleated(object sender, EventArgs e)
    {
        DisableAllWeakPoints();
    }
    #endregion
}
