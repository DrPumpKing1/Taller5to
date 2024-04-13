using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ProjectionGemsUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ProjectionGemsManager projectionGemsManager;

    [Header("UI Settings")]
    [SerializeField] private TextMeshProUGUI projectionGemsText;
    [SerializeField] private GameObject insuficientProjectionGemsText;
    [SerializeField] private float insuficientProjectionGemsTextShowTime;

    private void OnEnable()
    {
        projectionGemsManager.OnProjectionGemsUsed += ProjectionGemsManager_OnProjectionGemsUsed;
        projectionGemsManager.OnProjectionGemsRefunded += ProjectionGemsManager_OnProjectionGemsRefunded;
        projectionGemsManager.OnTotalProjectionGemsIncreased += ProjectionGemsManager_OnTotalProjectionGemsIncreased;
        projectionGemsManager.OnInsuficentProjectionGems += ProjectionGemsManager_OnInsuficientProjectionGems;
    }

    private void OnDisable()
    {
        projectionGemsManager.OnProjectionGemsUsed -= ProjectionGemsManager_OnProjectionGemsUsed;
        projectionGemsManager.OnProjectionGemsRefunded -= ProjectionGemsManager_OnProjectionGemsRefunded;
        projectionGemsManager.OnTotalProjectionGemsIncreased -= ProjectionGemsManager_OnTotalProjectionGemsIncreased;
        projectionGemsManager.OnInsuficentProjectionGems -= ProjectionGemsManager_OnInsuficientProjectionGems;
    }

    private void Awake()
    {
        SetProjectionGemsManager();
    }

    private void Start()
    {
        InitializeUI();
    }

    private void SetProjectionGemsManager()
    {
        if (!projectionGemsManager) projectionGemsManager = ProjectionGemsManager.Instance;
    }

    private void InitializeUI()
    {
        UpdateProjectionGemsUI();
        HideInsuficientGemsText();
    }

    private void UpdateProjectionGemsUI()
    {
        projectionGemsText.text = ($"Gemas de Proyección: {projectionGemsManager.AvailableProyectionGems}/{projectionGemsManager.TotalProjectionGems}");
    }

    private void ShowInsuficientGemsText() => insuficientProjectionGemsText.SetActive(true);
    private void HideInsuficientGemsText() => insuficientProjectionGemsText.SetActive(false);

    private IEnumerator InsuficientProjectionGemsCoroutine()
    {
        ShowInsuficientGemsText();
        yield return new WaitForSeconds(insuficientProjectionGemsTextShowTime);
        HideInsuficientGemsText();
    }

    #region ProjectionGemsManager Subscriptions
    private void ProjectionGemsManager_OnProjectionGemsUsed(object sender, ProjectionGemsManager.OnProjectionGemsEventArgs e)
    {
        UpdateProjectionGemsUI();
        Debug.Log("Test");
    }

    private void ProjectionGemsManager_OnProjectionGemsRefunded(object sender, ProjectionGemsManager.OnProjectionGemsEventArgs e)
    {
        UpdateProjectionGemsUI();
    }

    private void ProjectionGemsManager_OnTotalProjectionGemsIncreased(object sender, ProjectionGemsManager.OnProjectionGemsEventArgs e)
    {
        UpdateProjectionGemsUI();
    }

    private void ProjectionGemsManager_OnInsuficientProjectionGems(object sender, ProjectionGemsManager.OnProjectionGemsEventArgs e)
    {
        StopAllCoroutines();
        StartCoroutine(InsuficientProjectionGemsCoroutine());
    }
    #endregion
}
