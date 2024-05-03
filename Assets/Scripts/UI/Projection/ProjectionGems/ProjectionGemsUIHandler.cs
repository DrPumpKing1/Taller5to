using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ProjectionGemsUIHandler : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI projectionGemsText;

    private void OnEnable()
    {
        ProjectionGemsManager.OnProjectionGemsManagerInitialized += ProjectionGemsManager_OnProjectionGemsManagerInitialized;       

        ProjectionGemsManager.OnProjectionGemsUsed += ProjectionGemsManager_OnProjectionGemsUsed;
        ProjectionGemsManager.OnProjectionGemsRefunded += ProjectionGemsManager_OnProjectionGemsRefunded;
        ProjectionGemsManager.OnTotalProjectionGemsIncreased += ProjectionGemsManager_OnTotalProjectionGemsIncreased;
    }

    private void OnDisable()
    {
        ProjectionGemsManager.OnProjectionGemsManagerInitialized -= ProjectionGemsManager_OnProjectionGemsManagerInitialized;

        ProjectionGemsManager.OnProjectionGemsUsed -= ProjectionGemsManager_OnProjectionGemsUsed;
        ProjectionGemsManager.OnProjectionGemsRefunded -= ProjectionGemsManager_OnProjectionGemsRefunded;
        ProjectionGemsManager.OnTotalProjectionGemsIncreased -= ProjectionGemsManager_OnTotalProjectionGemsIncreased;
    }

    private void Start()
    {
        InitializeUI();
    }

    private void InitializeUI()
    {
        UpdateProjectionGemsUI();
    }

    private void UpdateProjectionGemsUI()
    {
        projectionGemsText.text = ($"{ProjectionGemsManager.Instance.AvailableProyectionGems}/{ProjectionGemsManager.Instance.TotalProjectionGems}");
    }

    #region ProjectionGemsManager Subscriptions
    private void ProjectionGemsManager_OnProjectionGemsManagerInitialized(object sender, EventArgs e)
    {
        InitializeUI();
    }

    private void ProjectionGemsManager_OnProjectionGemsUsed(object sender, ProjectionGemsManager.OnProjectionGemsEventArgs e)
    {
        UpdateProjectionGemsUI();
    }

    private void ProjectionGemsManager_OnProjectionGemsRefunded(object sender, ProjectionGemsManager.OnProjectionGemsEventArgs e)
    {
        UpdateProjectionGemsUI();
    }

    private void ProjectionGemsManager_OnTotalProjectionGemsIncreased(object sender, ProjectionGemsManager.OnProjectionGemsEventArgs e)
    {
        UpdateProjectionGemsUI();
    }
    #endregion
}
