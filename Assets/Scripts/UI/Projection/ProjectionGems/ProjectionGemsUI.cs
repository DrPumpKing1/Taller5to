using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ProjectionGemsUI : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private TextMeshProUGUI projectionGemsText;
    [SerializeField] private GameObject insuficientProjectionGemsText;
    [SerializeField] private float insuficientProjectionGemsTextShowTime;

    private void OnEnable()
    {
        ProjectionGemsManager.OnProjectionGemsManagerInitialized += ProjectionGemsManager_OnProjectionGemsManagerInitialized;       

        ProjectionGemsManager.OnProjectionGemsUsed += ProjectionGemsManager_OnProjectionGemsUsed;
        ProjectionGemsManager.OnProjectionGemsRefunded += ProjectionGemsManager_OnProjectionGemsRefunded;
        ProjectionGemsManager.OnTotalProjectionGemsIncreased += ProjectionGemsManager_OnTotalProjectionGemsIncreased;
        ProjectionGemsManager.OnInsuficentProjectionGems += ProjectionGemsManager_OnInsuficientProjectionGems;
    }

    private void OnDisable()
    {
        ProjectionGemsManager.OnProjectionGemsManagerInitialized -= ProjectionGemsManager_OnProjectionGemsManagerInitialized;

        ProjectionGemsManager.OnProjectionGemsUsed -= ProjectionGemsManager_OnProjectionGemsUsed;
        ProjectionGemsManager.OnProjectionGemsRefunded -= ProjectionGemsManager_OnProjectionGemsRefunded;
        ProjectionGemsManager.OnTotalProjectionGemsIncreased -= ProjectionGemsManager_OnTotalProjectionGemsIncreased;
        ProjectionGemsManager.OnInsuficentProjectionGems -= ProjectionGemsManager_OnInsuficientProjectionGems;
    }

    private void Start()
    {
        InitializeUI();
    }

    private void InitializeUI()
    {
        UpdateProjectionGemsUI();
        HideInsuficientGemsText();
    }

    private void UpdateProjectionGemsUI()
    {
        projectionGemsText.text = ($"Gemas de Proyección: {ProjectionGemsManager.Instance.AvailableProyectionGems}/{ProjectionGemsManager.Instance.TotalProjectionGems}");
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

    private void ProjectionGemsManager_OnInsuficientProjectionGems(object sender, ProjectionGemsManager.OnProjectionGemsEventArgs e)
    {
        //StopAllCoroutines();
        //StartCoroutine(InsuficientProjectionGemsCoroutine());
    }
    #endregion
}
