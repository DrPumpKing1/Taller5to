using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhaseHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int phaseNumber;
    [SerializeField] private int lastPhase;
    [SerializeField] private float invulnerabilityTimeAfterPhase;
   
    public static event EventHandler<OnPhaseChangeEventArgs> OnPhaseChange;
    public static event EventHandler OnLastPhaseEnded;

    public class OnPhaseChangeEventArgs : EventArgs
    {
        public int newPhase;
    }

    private void OnEnable()
    {
        BossKaerumOvercharge.OnBossOvercharge += BossKaerumOvercharge_OnBossOvercharge;
    }

    private void OnDisable()
    {
        BossKaerumOvercharge.OnBossOvercharge -= BossKaerumOvercharge_OnBossOvercharge;
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        phaseNumber = 1;
    }

    private void CheckChangePhase()
    {
        if(phaseNumber >= lastPhase)
        {
            OnLastPhaseEnded?.Invoke(this, EventArgs.Empty);
            return;
        }

        phaseNumber++;
        OnPhaseChange?.Invoke(this, new OnPhaseChangeEventArgs { newPhase = phaseNumber });
    }

    #region BossKaerumOvercharge Subscriptions
    private void BossKaerumOvercharge_OnBossOvercharge(object sender, EventArgs e)
    {
        CheckChangePhase();
    }
    #endregion
}
