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

    public static event EventHandler<OnWeakPointsEventArgs> OnWeakPointsEnableA;
    public static event EventHandler<OnWeakPointsEventArgs> OnWeakPointsEnableB;
    public static event EventHandler<OnWeakPointsEventArgs> OnWeakPointsDisableA;
    public static event EventHandler<OnWeakPointsEventArgs> OnWeakPointsDisableB;

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
        BossStateHandler.OnBossPhaseChangeMidC += BossStateHandler_OnBossPhaseChangeMidC;

        BossPhaseHandler.OnLastPhaseCompleated += BossPhaseHandler_OnLastPhaseCompleated;
        BossStateHandler.OnBossDefeated += BossStateHandler_OnBossDefeated;

        BossBeam.OnBeamChargeStart += BossBeam_OnBeamStart;
        BossBeam.OnBeamChargeEnd += BossBeam_OnBeamEnd;
    }

    private void OnDisable()
    {
        BossPhaseHandler.OnPhaseCompleated -= BossPhaseHandler_OnPhaseCompleated;
        BossStateHandler.OnBossPhaseChangeEnd -= BossStateHandler_OnBossPhaseChangeEnd;
        BossStateHandler.OnBossPhaseChangeMidC -= BossStateHandler_OnBossPhaseChangeMidC;

        BossPhaseHandler.OnLastPhaseCompleated -= BossPhaseHandler_OnLastPhaseCompleated;
        BossStateHandler.OnBossDefeated -= BossStateHandler_OnBossDefeated;

        BossBeam.OnBeamChargeStart -= BossBeam_OnBeamStart;
        BossBeam.OnBeamChargeEnd -= BossBeam_OnBeamEnd;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        DisableAllWeakPoints();
        TotalEnableWeakPointsByRegularPhase(FIRST_WEAK_POINTS_PHASE);
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
        if (phaseWeakPoints.weakPoints.Count <= 0) return;

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
        TotalEnableWeakPointsByRegularPhase(ALMOST_DEFEATED_WEAK_POINTS_PHASE);
    }

    #region PhaseChange
    private void EnableWeakPointsByPhaseChange(BossPhase bossPhase)
    {
        foreach (PhaseWeakPoints phaseWeakPoints in regularPhaseWeakPointsList)
        {
            if (phaseWeakPoints.bossPhase == bossPhase)
            {
                OnWeakPointsEnableA?.Invoke(this, new OnWeakPointsEventArgs { weakPoints = phaseWeakPoints.weakPoints });
            }
        }

        foreach (PhaseWeakPoints phaseWeakPoints in beamPhaseWeakPointsList)
        {
            if (phaseWeakPoints.bossPhase == bossPhase)
            {
                OnWeakPointsEnableA?.Invoke(this, new OnWeakPointsEventArgs { weakPoints = phaseWeakPoints.weakPoints });
            }
        }
    }

    private void EnableWeakPointsByPhaseChangeCompleated(BossPhase bossPhase)
    {
        foreach (PhaseWeakPoints phaseWeakPoints in regularPhaseWeakPointsList)
        {
            if (phaseWeakPoints.bossPhase == bossPhase)
            {
                OnWeakPointsEnableB?.Invoke(this, new OnWeakPointsEventArgs { weakPoints = phaseWeakPoints.weakPoints });
            }
        }
    }

    private void DisableWeakPointsByPhaseCompleated(BossPhase bossPhase)
    {
        foreach (PhaseWeakPoints phaseWeakPoints in regularPhaseWeakPointsList)
        {
            if (phaseWeakPoints.bossPhase == bossPhase)
            {
                OnWeakPointsDisableA?.Invoke(this, new OnWeakPointsEventArgs { weakPoints = phaseWeakPoints.weakPoints });
            }
        }

        foreach (PhaseWeakPoints phaseWeakPoints in beamPhaseWeakPointsList)
        {
            if (phaseWeakPoints.bossPhase == bossPhase)
            {
                OnWeakPointsDisableA?.Invoke(this, new OnWeakPointsEventArgs { weakPoints = phaseWeakPoints.weakPoints });
            }
        }
    }

    private void DisableWeakPointsByPhaseChange(BossPhase bossPhase)
    {
        foreach (PhaseWeakPoints phaseWeakPoints in regularPhaseWeakPointsList)
        {
            if (phaseWeakPoints.bossPhase == bossPhase)
            {
                OnWeakPointsDisableB?.Invoke(this, new OnWeakPointsEventArgs { weakPoints = phaseWeakPoints.weakPoints });
            }
        }

        foreach (PhaseWeakPoints phaseWeakPoints in beamPhaseWeakPointsList)
        {
            if (phaseWeakPoints.bossPhase == bossPhase)
            {
                OnWeakPointsDisableB?.Invoke(this, new OnWeakPointsEventArgs { weakPoints = phaseWeakPoints.weakPoints });
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
                OnWeakPointsEnableB?.Invoke(this, new OnWeakPointsEventArgs { weakPoints = phaseWeakPoints.weakPoints });
            }
        }
    }

    private void DisableWeakPointsByBeamEnd(BossPhase bossPhase)
    {
        foreach (PhaseWeakPoints phaseWeakPoints in beamPhaseWeakPointsList)
        {
            if (phaseWeakPoints.bossPhase == bossPhase)
            {
                OnWeakPointsDisableA?.Invoke(this, new OnWeakPointsEventArgs { weakPoints = phaseWeakPoints.weakPoints });
            }
        }
    }
    #endregion

    private void TotalEnableWeakPointsByRegularPhase(BossPhase bossPhase)
    {
        foreach (PhaseWeakPoints phaseWeakPoints in regularPhaseWeakPointsList)
        {
            if (phaseWeakPoints.bossPhase == bossPhase)
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
        }

        foreach (PhaseWeakPoints phaseWeakPoints in beamPhaseWeakPointsList)
        {
            OnWeakPointsDisableA?.Invoke(this, new OnWeakPointsEventArgs { weakPoints = phaseWeakPoints.weakPoints });
        }
    }

    #region BossPhase&StateHandler Subscriptions
    private void BossStateHandler_OnBossPhaseChangeEnd(object sender, BossStateHandler.OnPhaseChangeEventArgs e)
    {
        EnableWeakPointsByPhaseChangeCompleated(e.nextPhase);
    }

    private void BossPhaseHandler_OnPhaseCompleated(object sender, BossPhaseHandler.OnPhaseEventArgs e)
    {
        DisableWeakPointsByPhaseCompleated(e.currentPhase);
    }

    private void BossStateHandler_OnBossPhaseChangeMidC(object sender, BossStateHandler.OnPhaseChangeEventArgs e)
    {
        DisableWeakPointsByPhaseChange(e.currentPhase); //Visual
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
        EnableWeakPointsByBeamStart(e.phaseBeam.bossPhase);
    }

    private void BossBeam_OnBeamEnd(object sender, BossBeam.OnBeamEventArgs e)
    {
        DisableWeakPointsByBeamEnd(e.phaseBeam.bossPhase);
    }
    #endregion
}
