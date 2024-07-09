using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ProjectionGemsUIHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator projectionGemsUIAnimator;
    [SerializeField] private List<ProjectionGemUI> projectionGemUIs;

    private int TotalProjectionGems => ProjectionGemsManager.Instance.TotalProjectionGems;
    private int AvailableProjectionGems => ProjectionGemsManager.Instance.AvailableProjectionGems;
    private int currentProjectionGemsShown;

    private const string IDLE_0_ANIMATION = "Idle0";
    private const string IDLE_1_ANIMATION = "Idle1";
    private const string IDLE_2_ANIMATION = "Idle2";
    private const string IDLE_3_ANIMATION = "Idle3";
    private const string IDLE_4_ANIMATION = "Idle4";

    private const string TRANSITION_0_1_TRIGGER = "Transition0_1";
    private const string TRANSITION_1_2_TRIGGER = "Transition1_2";
    private const string TRANSITION_2_3_TRIGGER = "Transition2_3";
    private const string TRANSITION_3_4_TRIGGER = "Transition3_4";


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
        SetProjectionGemsUI(TotalProjectionGems);
    }

    private void SetProjectionGemsUI(int totalProjectionGems)
    {
        switch (totalProjectionGems)
        {
            case 0:
            default:
                PlayAnimation(IDLE_0_ANIMATION);
                break;
            case 1:
                PlayAnimation(IDLE_1_ANIMATION);
                break;
            case 2:
                PlayAnimation(IDLE_2_ANIMATION);
                break;
            case 3:
                PlayAnimation(IDLE_3_ANIMATION);
                break;
            case 4:
                PlayAnimation(IDLE_4_ANIMATION);
                break;
        }

        currentProjectionGemsShown = totalProjectionGems;
    }

    private void IncreaseProjectionGemsUI(int totalProjectionGems)
    {
        switch (totalProjectionGems)
        {
            case 0:
            default:
                break;
            case 1:
                TriggerAnimation(TRANSITION_0_1_TRIGGER);
                break;
            case 2:
                TriggerAnimation(TRANSITION_1_2_TRIGGER);
                break;
            case 3:
                TriggerAnimation(TRANSITION_2_3_TRIGGER);
                break;
            case 4:
                TriggerAnimation(TRANSITION_3_4_TRIGGER);
                break;
        }

        UpdateProjectionGemsUI(AvailableProjectionGems);
    }

    private void UpdateProjectionGemsUI(int availableProjectionGems)
    {
        if (availableProjectionGems == currentProjectionGemsShown) return;

        foreach(ProjectionGemUI projectionGemUI in projectionGemUIs)
        {
            if (projectionGemUI.GemNumber <= TotalProjectionGems - availableProjectionGems) projectionGemUI.HideProjectionGem();
            else projectionGemUI.ShowProjectionGem();
        }

        currentProjectionGemsShown = availableProjectionGems;
    }

    private void PlayAnimation(string animation) => projectionGemsUIAnimator.Play(animation);
    private void TriggerAnimation(string trigger) => projectionGemsUIAnimator.SetTrigger(trigger);

    #region ProjectionGemsManager Subscriptions
    private void ProjectionGemsManager_OnProjectionGemsManagerInitialized(object sender, EventArgs e)
    {
        InitializeUI();
    }

    private void ProjectionGemsManager_OnProjectionGemsUsed(object sender, ProjectionGemsManager.OnProjectionGemsEventArgs e)
    {
        UpdateProjectionGemsUI(AvailableProjectionGems);
    }

    private void ProjectionGemsManager_OnProjectionGemsRefunded(object sender, ProjectionGemsManager.OnProjectionGemsEventArgs e)
    {
        UpdateProjectionGemsUI(AvailableProjectionGems);
    }

    private void ProjectionGemsManager_OnTotalProjectionGemsIncreased(object sender, ProjectionGemsManager.OnProjectionGemsEventArgs e)
    {
        IncreaseProjectionGemsUI(TotalProjectionGems);
    }
    #endregion
}
