using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProjectableObjectsUIHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator projectableObjectSelectionUIAnimator;
    [SerializeField] private List<ProjectableObjectUI> projectableObjectUIs;

    [Header("Debug")]
    [SerializeField] private bool debug;

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
        ProjectableObjectSelectionManager.OnProjectableObjectSelectionManagerInitialized += ProjectableObjectSelectionManager_OnProjectableObjectSelectionManagerInitialized;

        ProjectableObjectSelectionManager.OnObjectAddedToInventory += ProjectableObjectSelectionManager_OnObjectAddedToInventory;
        ProjectableObjectSelectionManager.OnProjectableObjectSelected += ProjectableObjectSelectionManager_OnProjectableObjectSelected;
        ProjectableObjectSelectionManager.OnProjectableObjectDeselected += ProjectableObjectSelectionManager_OnProjectableObjectDeselected;
    }

    private void OnDisable()
    {
        ProjectableObjectSelectionManager.OnProjectableObjectSelectionManagerInitialized -= ProjectableObjectSelectionManager_OnProjectableObjectSelectionManagerInitialized;

        ProjectableObjectSelectionManager.OnObjectAddedToInventory -= ProjectableObjectSelectionManager_OnObjectAddedToInventory;
        ProjectableObjectSelectionManager.OnProjectableObjectSelected -= ProjectableObjectSelectionManager_OnProjectableObjectSelected;
        ProjectableObjectSelectionManager.OnProjectableObjectDeselected -= ProjectableObjectSelectionManager_OnProjectableObjectDeselected;
    }

    private void InitializeUI()
    {
        SetProjectableObjectsUI(ProjectableObjectSelectionManager.Instance.ProjectableObjectsIndexed);
        PlayRawAnimation(ProjectableObjectSelectionManager.Instance.ProjectableObjectsIndexed.Count);
        DeselectAllUI();
    }

    private void SetProjectableObjectsUI(List<ProjectableObjectSelectionManager.ProjectableObjectIndexed> availableProjectableObjectsIndexed)
    {
        foreach(ProjectableObjectSelectionManager.ProjectableObjectIndexed availableProjectableObjectIndexed in availableProjectableObjectsIndexed)
        {
            SetProjectableObjectUI(availableProjectableObjectIndexed);
        }
    }

    private void SetProjectableObjectUI(ProjectableObjectSelectionManager.ProjectableObjectIndexed projectableObjectIndexed)
    {
        foreach(ProjectableObjectUI projectableObjectUI in projectableObjectUIs)
        {
            if(projectableObjectUI.Index == projectableObjectIndexed.index)
            {
                projectableObjectUI.SetUI(projectableObjectIndexed.projectableObjectSO);
                return;
            }
        }

        if(debug) Debug.Log($"No projectable object UI matches index: {projectableObjectIndexed.index}");
    }

    private void PlayRawAnimation(int availableProjectableObjectsIndexed)
    {
        switch (availableProjectableObjectsIndexed)
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
    }

    private void PlayTransitionAnimation(int availableProjectableObjectsIndexed)
    {
        switch (availableProjectableObjectsIndexed)
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
    }

    private void PlayAnimation(string animation) => projectableObjectSelectionUIAnimator.Play(animation);
    private void TriggerAnimation(string trigger) => projectableObjectSelectionUIAnimator.SetTrigger(trigger);

    private void DeselectAllUI()
    {
        foreach(ProjectableObjectUI projectableObjectUI in projectableObjectUIs)
        {
            projectableObjectUI.DeselectUI();
        }
    }

    private void SelectIndexedProjectableObject(ProjectableObjectSelectionManager.ProjectableObjectIndexed projectableObjectIndexed)
    {
        foreach (ProjectableObjectUI projectableObjectUI in projectableObjectUIs)
        {
            if(projectableObjectUI.Index == projectableObjectIndexed.index)
            {
                projectableObjectUI.SelectUI();
                return;
            }
        }

        if (debug) Debug.Log($"No projectable object UI matches selection index: {projectableObjectIndexed.index}");
    }

    private void DeselectIndexedProjectableObject(ProjectableObjectSelectionManager.ProjectableObjectIndexed projectableObjectIndexed)
    {
        foreach(ProjectableObjectUI projectableObjectUI in projectableObjectUIs)
        {
            if (projectableObjectUI.Index == projectableObjectIndexed.index)
            {
                projectableObjectUI.DeselectUI();
                return;
            }
        }

        if (debug) Debug.Log($"No projectable object UI matches deselection index: {projectableObjectIndexed.index}");
    }


    #region Events Subscriptions
    private void ProjectableObjectSelectionManager_OnProjectableObjectSelectionManagerInitialized(object sender, EventArgs e)
    {
        InitializeUI();     
    }

    private void ProjectableObjectSelectionManager_OnObjectAddedToInventory(object sender, ProjectableObjectSelectionManager.OnObjectAddedToInventoryEventArgs e)
    {
        SetProjectableObjectUI(e.projectableObjectIndexed);
        PlayTransitionAnimation(ProjectableObjectSelectionManager.Instance.ProjectableObjectsIndexed.Count);
    }

    private void ProjectableObjectSelectionManager_OnProjectableObjectSelected(object sender, ProjectableObjectSelectionManager.OnSelectionEventArgs e)
    {
        SelectIndexedProjectableObject(e.projectableObjectIndexed);
    }

    private void ProjectableObjectSelectionManager_OnProjectableObjectDeselected(object sender, ProjectableObjectSelectionManager.OnSelectionEventArgs e)
    {
        DeselectIndexedProjectableObject(e.projectableObjectIndexed);
    }
    #endregion
}
