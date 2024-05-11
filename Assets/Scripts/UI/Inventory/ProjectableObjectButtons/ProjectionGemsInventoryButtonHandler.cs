using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class ProjectionGemsInventoryButtonHandler : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Button projectionGemsButton;
    [SerializeField] private TextMeshProUGUI projectionGemsText;

    public static event EventHandler<OnProjectionGemsButtonClickedEventArgs> OnProjectionGemsButtonClicked;

    public class OnProjectionGemsButtonClickedEventArgs : EventArgs
    {
        public int totalProjectionGems;
        public int availableProjectionGems;
    }
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

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void Start()
    {
        InitializeUI();
    }

    private void InitializeButtonsListeners()
    {
        projectionGemsButton.onClick.AddListener(ClickButton);
    }

    private void ClickButton()
    {
        OnProjectionGemsButtonClicked?.Invoke(this, new OnProjectionGemsButtonClickedEventArgs { totalProjectionGems = ProjectionGemsManager.Instance.TotalProjectionGems, availableProjectionGems = ProjectionGemsManager.Instance.AvailableProjectionGems });
    }

    private void InitializeUI()
    {
        UpdateProjectionGemsUI();
    }

    private void UpdateProjectionGemsUI()
    {
        projectionGemsText.text = ($"{ProjectionGemsManager.Instance.AvailableProjectionGems}/{ProjectionGemsManager.Instance.TotalProjectionGems}");
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
