using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BossSurge : MonoBehaviour
{
    public static BossSurge Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private List<PhaseActiveLayout> phaseActiveLayouts;

    public static event EventHandler<OnBossSurgeEventArgs> OnBossSurge;

    private PhaseActiveLayout currentActiveLayout;

    private const BossPhase STARTING_SURGE_PHASE = BossPhase.Phase0;

    public class OnBossSurgeEventArgs : EventArgs
    {
        public PhaseActiveLayout phaseActiveLayout;
    }

    [Serializable]
    public class PhaseActiveLayout
    {
        public BossPhase activePhase;
        public Transform layoutTransform;
        public Transform activeLayoutPosition;
        public Transform disabledLayoutPosition;
    }

    private void OnEnable()
    {
        BossStateHandler.OnBossPhaseChangeMidB += BossStateHandler_OnBossPhaseChangeMidB;
    }

    private void OnDisable()
    {
        BossStateHandler.OnBossPhaseChangeMidB -= BossStateHandler_OnBossPhaseChangeMidB;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        ClearCurrentActiveLayout();
        CheckSurge(STARTING_SURGE_PHASE, false);
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one BossSurge, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }


    private void Surge(PhaseActiveLayout targetPhaseActiveLayout, bool triggerEvents)
    {
        foreach (PhaseActiveLayout phaseActiveLayout in phaseActiveLayouts) //Move all nonActives underground;
        {
            if (phaseActiveLayout != targetPhaseActiveLayout) SetLayout(phaseActiveLayout, false);
        }

        SetCurrentActiveLayout(targetPhaseActiveLayout); 
        SetLayout(targetPhaseActiveLayout, true); //Move Active layout up
        if(triggerEvents) OnBossSurge?.Invoke(this, new OnBossSurgeEventArgs { phaseActiveLayout = targetPhaseActiveLayout });
    }

    private void CheckSurge(BossPhase phase, bool triggerEvents)
    {
        foreach(PhaseActiveLayout phaseActiveLayout in phaseActiveLayouts)
        {
            if (phaseActiveLayout.activePhase == phase)
            {
                Surge(phaseActiveLayout, triggerEvents);
                return;
            }
        }
    }

    private void SetLayout(PhaseActiveLayout phaseActiveLayout, bool active)
    {
        if (active) phaseActiveLayout.layoutTransform.position = phaseActiveLayout.activeLayoutPosition.position;
        else phaseActiveLayout.layoutTransform.position = phaseActiveLayout.disabledLayoutPosition.position;
    }

    private void SetCurrentActiveLayout(PhaseActiveLayout phaseActiveLayout) => currentActiveLayout = phaseActiveLayout;
    private void ClearCurrentActiveLayout() => currentActiveLayout = null;

    #region BossStateHandler Subscriptions
    private void BossStateHandler_OnBossPhaseChangeMidB(object sender, BossStateHandler.OnPhaseChangeEventArgs e)
    {
        CheckSurge(e.nextPhase, true);
    }
    #endregion
}
