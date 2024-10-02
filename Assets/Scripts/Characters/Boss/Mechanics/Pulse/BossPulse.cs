using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossPulse : MonoBehaviour
{
    public static BossPulse Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private List<PhasePulse> phasePulses;

    public static event EventHandler<OnBossPulseEventArgs> OnBossPulse;

    public class OnBossPulseEventArgs : EventArgs
    {
        public List<ToggleableSource> onSources;
        public List<ToggleableSource> offSources;
    }

    [Serializable]
    public class PhasePulse
    {
        public BossPhase bossPhase;
        public List<ToggleableSource> onSources;
        public List<ToggleableSource> offSources;
    }

    private void OnEnable()
    {
        BossStateHandler.OnBossPhaseChangeMid += BossStateHandler_OnBossPhaseChangeMid;
    }

    private void OnDisable()
    {
        BossStateHandler.OnBossPhaseChangeMid -= BossStateHandler_OnBossPhaseChangeMid;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one BossPulse, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    
    private void CheckPulse(BossPhase phase)
    {
        foreach(PhasePulse phasePulse in phasePulses)
        {
            if(phasePulse.bossPhase == phase)
            {
                Pulse(phasePulse.onSources,phasePulse.offSources);
                return;
            }
        }
    }

    private void Pulse(List<ToggleableSource> onSources, List<ToggleableSource> offSources)
    {
        TurnSources(onSources, true);
        TurnSources(offSources, false);

        OnBossPulse?.Invoke(this, new OnBossPulseEventArgs { onSources = onSources, offSources = offSources});
    }

    private void TurnSources(List<ToggleableSource> sources, bool on)
    {
        foreach (ToggleableSource source in sources)
        {
            if (on) source.TurnOnSource();
            else source.TurnOffSource();
        }
    }


    #region BossPhaseHandler Subscriptions
    private void BossStateHandler_OnBossPhaseChangeMid(object sender, BossStateHandler.OnPhaseChangeEventArgs e)
    {
        CheckPulse(e.nextPhase);
    }

    #endregion

}
