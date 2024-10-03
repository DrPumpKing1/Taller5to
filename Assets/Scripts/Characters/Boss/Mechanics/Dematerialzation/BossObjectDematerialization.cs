using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossObjectDematerialization : MonoBehaviour
{
    public static BossObjectDematerialization Instance { get; private set; }

    public static event EventHandler OnBossAllObjectDematerialized;

    private void OnEnable()
    {
        BossStateHandler.OnBossPhaseChangeMidA += BossStateHandler_OnBossPhaseChangeMidA;
        BossStateHandler.OnBossDefeated += BossStateHandler_OnBossDefeated;
    }

    private void OnDisable()
    {
        BossStateHandler.OnBossPhaseChangeMidA -= BossStateHandler_OnBossPhaseChangeMidA;
        BossStateHandler.OnBossDefeated -= BossStateHandler_OnBossDefeated;
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
            Debug.LogWarning("There is more than one BossObjectDematerialization, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public void DematerializeInTargetPlatform(ProjectionPlatform projectionPlatform)
    {
        if (projectionPlatform.CurrentProjectedObject == null) return;

        ProjectableObjectDematerialization projectableObjectDematerialization = projectionPlatform.GetProjectableObjectDematerialization();

        if (projectableObjectDematerialization == null) return;

        projectableObjectDematerialization.ForceDematerializeObject(true);
    }

    public void DematerializeAllObjects()
    {
        ProjectionManager.Instance.ForceDematerializeAllObjects();
        OnBossAllObjectDematerialized?.Invoke(this, EventArgs.Empty);
    }

    #region BossStateHandler Subscriptions
    private void BossStateHandler_OnBossPhaseChangeMidA(object sender, BossStateHandler.OnPhaseChangeEventArgs e)
    {
        DematerializeAllObjects();
    }

    private void BossStateHandler_OnBossDefeated(object sender, EventArgs e)
    {
        DematerializeAllObjects();
    }
    #endregion
}
