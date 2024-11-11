using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowcaseRoomObjectDematerialization : MonoBehaviour
{
    public static ShowcaseRoomObjectDematerialization Instance { get; private set; }

    public static event EventHandler OnShowcaseRoomAllObjectDematerialized;

    private void OnEnable()
    {
        ShowcaseRoomStateHandler.OnShowcaseRoomPhaseChangeMidA += ShowcaseRoomStateHandler_OnShowcaseRoomPhaseChangeMidA;
        ShowcaseRoomStateHandler.OnShowcaseRoomDefeated += ShowcaseRoomStateHandler_OnShowcaseRoomDefeated;
    }

    private void OnDisable()
    {
        ShowcaseRoomStateHandler.OnShowcaseRoomPhaseChangeMidA -= ShowcaseRoomStateHandler_OnShowcaseRoomPhaseChangeMidA;
        ShowcaseRoomStateHandler.OnShowcaseRoomDefeated -= ShowcaseRoomStateHandler_OnShowcaseRoomDefeated;
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
            Debug.LogWarning("There is more than one ShowcaseRoomObjectDematerialization, proceding to destroy duplicate");
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
        OnShowcaseRoomAllObjectDematerialized?.Invoke(this, EventArgs.Empty);
    }

    #region ShowcaseRoomStateHandler Subscriptions
    private void ShowcaseRoomStateHandler_OnShowcaseRoomPhaseChangeMidA(object sender, ShowcaseRoomStateHandler.OnPhaseChangeEventArgs e)
    {
        DematerializeAllObjects();
    }

    private void ShowcaseRoomStateHandler_OnShowcaseRoomDefeated(object sender, EventArgs e)
    {
        DematerializeAllObjects();
    }
    #endregion
}
