using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Analytics;
using static BossWeakPointsHandler;

public class BossWeakPointsHandler : MonoBehaviour
{
    public static BossWeakPointsHandler Instance {  get; private set; }

    [Header("Settings")]
    [SerializeField] private List<PhaseWeakPoints> phaseWeakPointsList;

    [Serializable]
    public class PhaseWeakPoints
    {
        public BossPhase bossPhase;
        public List<BossWeakPoint> weakPoints;
        public bool allHit;
    }

    public static event EventHandler<OnWeakPointsEventArgs> OnWeakPointsEnable;
    public static event EventHandler<OnWeakPointsEventArgs> OnWeakPointsDisable;

    public static event EventHandler<OnPhaseWeakPointsHitEventArgs> OnPhaseWeakPointsHit;

    private const BossPhase FIRST_WEAK_POINTS_PHASE = BossPhase.Phase0;

    public class OnWeakPointsEventArgs : EventArgs
    {
        public List<BossWeakPoint> weakPoints;
    }

    public class OnPhaseWeakPointsHitEventArgs : EventArgs
    {
        public PhaseWeakPoints phaseWeakPoints;
    }

    private void OnEnable()
    {
        BossPhaseHandler.OnPhaseCompleated += BossPhaseHandler_OnPhaseCompleated;
        BossStateHandler.OnBossPhaseChangeMid += BossStateHandler_OnBossPhaseChangeMid;

    }

    private void OnDisable()
    {
        BossPhaseHandler.OnPhaseCompleated -= BossPhaseHandler_OnPhaseCompleated;
        BossStateHandler.OnBossPhaseChangeMid -= BossStateHandler_OnBossPhaseChangeMid;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        DisableAllWeakPoints();
        EnableWeakPointsByPhase(FIRST_WEAK_POINTS_PHASE);
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
            Debug.LogWarning("There is more than one BossWeakPointsHandler, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void HandlePhaseWeakPointsList()
    {
        foreach (PhaseWeakPoints phaseWeakPoints in phaseWeakPointsList)
        {
            HandlePhaseWeakPoints(phaseWeakPoints);
        }
    }

    private void HandlePhaseWeakPoints(PhaseWeakPoints phaseWeakPoints)
    {
        if (phaseWeakPoints.allHit) return;

        foreach (BossWeakPoint bossWeakPoint in phaseWeakPoints.weakPoints)
        {
            if (!bossWeakPoint.IsHit) continue;
        }

        phaseWeakPoints.allHit = true;
        OnPhaseWeakPointsHit?.Invoke(this, new OnPhaseWeakPointsHitEventArgs { phaseWeakPoints = phaseWeakPoints });
    }

    private void EnableWeakPointsByPhase(BossPhase bossPhase)
    {
        foreach (PhaseWeakPoints phaseWeakPoints in phaseWeakPointsList)
        {
            if (phaseWeakPoints.bossPhase == bossPhase)
            {
                OnWeakPointsEnable?.Invoke(this, new OnWeakPointsEventArgs { weakPoints = phaseWeakPoints.weakPoints });
            }
        }
    }

    private void DisableWeakPointsByPhase(BossPhase bossPhase)
    {
        foreach(PhaseWeakPoints phaseWeakPoints in phaseWeakPointsList)
        {
            if(phaseWeakPoints.bossPhase == bossPhase)
            {
                OnWeakPointsDisable?.Invoke(this, new OnWeakPointsEventArgs { weakPoints = phaseWeakPoints.weakPoints });
            }
        }
    }

    private void DisableAllWeakPoints()
    {
        foreach (PhaseWeakPoints phaseWeakPoints in phaseWeakPointsList)
        {
            OnWeakPointsDisable?.Invoke(this, new OnWeakPointsEventArgs { weakPoints = phaseWeakPoints.weakPoints });
        }
    }

    #region BossPhase&StateHandler Subscriptions
    private void BossPhaseHandler_OnPhaseCompleated(object sender, BossPhaseHandler.OnPhaseEventArgs e)
    {
        DisableWeakPointsByPhase(e.currentPhase);
    }

    private void BossStateHandler_OnBossPhaseChangeMid(object sender, BossStateHandler.OnPhaseChangeEventArgs e)
    {
        EnableWeakPointsByPhase(e.nextPhase);
    }

    #endregion
}
