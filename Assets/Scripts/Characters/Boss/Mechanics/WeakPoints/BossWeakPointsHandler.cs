using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Analytics;

public class BossWeakPointsHandler : MonoBehaviour
{
    public static BossWeakPointsHandler Instance {  get; private set; }

    [Header("Settings")]
    [SerializeField] private List<PhaseWeakPoints> regularPhaseWeakPointsList;
    [SerializeField] private List<PhaseWeakPoints> beamPhaseWeakPointsList;

    [Header("Debug")]
    [SerializeField] private bool debug;

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
    private const BossPhase ALMOST_DEFEATED_WEAK_POINTS_PHASE = BossPhase.AlmostDefeated;

    private const float TIME_TO_ENABLE_ALMOST_DEFEATED_WEAK_POINTS = 2f;

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
        BossStateHandler.OnBossPhaseChangeEnd += BossStateHandler_OnBossPhaseChangeEnd;

        BossPhaseHandler.OnLastPhaseCompleated += BossPhaseHandler_OnLastPhaseCompleated;
        BossStateHandler.OnBossDefeated += BossStateHandler_OnBossDefeated;

        BossBeam.OnBeamStart += BossBeam_OnBeamStart;
        BossBeam.OnBeamEnd += BossBeam_OnBeamEnd;

    }

    private void OnDisable()
    {
        BossPhaseHandler.OnPhaseCompleated -= BossPhaseHandler_OnPhaseCompleated;
        BossStateHandler.OnBossPhaseChangeEnd -= BossStateHandler_OnBossPhaseChangeEnd;

        BossPhaseHandler.OnLastPhaseCompleated -= BossPhaseHandler_OnLastPhaseCompleated;
        BossStateHandler.OnBossDefeated -= BossStateHandler_OnBossDefeated;

        BossBeam.OnBeamStart -= BossBeam_OnBeamStart;
        BossBeam.OnBeamEnd -= BossBeam_OnBeamEnd;
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
            Debug.LogWarning("There is more than one BossWeakPointsHandler, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void HandlePhaseWeakPointsList()
    {
        foreach (PhaseWeakPoints phaseWeakPoints in regularPhaseWeakPointsList)
        {
            HandlePhaseWeakPoints(phaseWeakPoints);
        }

        foreach (PhaseWeakPoints phaseWeakPoints in beamPhaseWeakPointsList)
        {
            HandlePhaseWeakPoints(phaseWeakPoints);
        }
    }

    private void HandlePhaseWeakPoints(PhaseWeakPoints phaseWeakPoints)
    {
        if (phaseWeakPoints.allHit) return;

        foreach (BossWeakPoint bossWeakPoint in phaseWeakPoints.weakPoints)
        {
            if (!bossWeakPoint.IsHit) return;
        }

        phaseWeakPoints.allHit = true;
        OnPhaseWeakPointsHit?.Invoke(this, new OnPhaseWeakPointsHitEventArgs { phaseWeakPoints = phaseWeakPoints });

        if(debug) Debug.Log($"AllWeakPointsHit {phaseWeakPoints.bossPhase}");
    }

    private IEnumerator EnableAlmostDefeatedWeakPointsCoroutine()
    {
        yield return new WaitForSeconds(TIME_TO_ENABLE_ALMOST_DEFEATED_WEAK_POINTS);
        EnableWeakPointsByPhaseChange(ALMOST_DEFEATED_WEAK_POINTS_PHASE);
    }

    #region PhaseChange
    private void EnableWeakPointsByPhaseChange(BossPhase bossPhase)
    {
        foreach (PhaseWeakPoints phaseWeakPoints in regularPhaseWeakPointsList)
        {
            if (phaseWeakPoints.bossPhase == bossPhase)
            {
                OnWeakPointsEnable?.Invoke(this, new OnWeakPointsEventArgs { weakPoints = phaseWeakPoints.weakPoints });
            }
        }
    }

    private void DisableWeakPointsByPhaseChange(BossPhase bossPhase)
    {
        foreach (PhaseWeakPoints phaseWeakPoints in regularPhaseWeakPointsList)
        {
            if (phaseWeakPoints.bossPhase == bossPhase)
            {
                OnWeakPointsDisable?.Invoke(this, new OnWeakPointsEventArgs { weakPoints = phaseWeakPoints.weakPoints });
            }
        }

        foreach (PhaseWeakPoints phaseWeakPoints in beamPhaseWeakPointsList)
        {
            if (phaseWeakPoints.bossPhase == bossPhase)
            {
                OnWeakPointsDisable?.Invoke(this, new OnWeakPointsEventArgs { weakPoints = phaseWeakPoints.weakPoints });
            }
        }
    }
    #endregion

    #region BossBeam
    private void EnableWeakPointsByBeamStart(BossPhase bossPhase)
    {
        foreach (PhaseWeakPoints phaseWeakPoints in beamPhaseWeakPointsList)
        {
            if (phaseWeakPoints.bossPhase == bossPhase)
            {
                OnWeakPointsEnable?.Invoke(this, new OnWeakPointsEventArgs { weakPoints = phaseWeakPoints.weakPoints });
            }
        }
    }

    private void DisableWeakPointsByBeamEnd(BossPhase bossPhase)
    {
        foreach (PhaseWeakPoints phaseWeakPoints in beamPhaseWeakPointsList)
        {
            if (phaseWeakPoints.bossPhase == bossPhase)
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

        foreach (PhaseWeakPoints phaseWeakPoints in beamPhaseWeakPointsList)
        {
            OnWeakPointsDisable?.Invoke(this, new OnWeakPointsEventArgs { weakPoints = phaseWeakPoints.weakPoints });
        }
    }

    #region BossPhase&StateHandler Subscriptions
    private void BossPhaseHandler_OnPhaseCompleated(object sender, BossPhaseHandler.OnPhaseEventArgs e)
    {
        DisableWeakPointsByPhaseChange(e.currentPhase);
    }

    private void BossStateHandler_OnBossPhaseChangeEnd(object sender, BossStateHandler.OnPhaseChangeEventArgs e)
    {
        EnableWeakPointsByPhaseChange(e.nextPhase);
    }
    private void BossPhaseHandler_OnLastPhaseCompleated(object sender, EventArgs e)
    {
        DisableAllWeakPoints();
        StartCoroutine(EnableAlmostDefeatedWeakPointsCoroutine());
    }

    private void BossStateHandler_OnBossDefeated(object sender, EventArgs e)
    {
        DisableAllWeakPoints();
    }
    #endregion

    #region BossBeam Subscriptions
    private void BossBeam_OnBeamStart(object sender, BossBeam.OnBeamEventArgs e)
    {
        EnableWeakPointsByBeamStart(e.bossPhase);
    }

    private void BossBeam_OnBeamEnd(object sender, BossBeam.OnBeamEventArgs e)
    {
        DisableWeakPointsByBeamEnd(e.bossPhase);

    }
    #endregion
}
