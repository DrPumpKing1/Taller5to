using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProjectableObjectSelectionUI : MonoBehaviour
{
    [Header("UI Components)")]
    [SerializeField] private TextMeshProUGUI selectedProjectableObjectText;
    [SerializeField] private Transform projectableObjectSelectionContainer;
    [SerializeField] private Transform projectableObjectSelectionSingleUIPrefab;

    private void OnEnable()
    {
        ProjectionManager.OnProjectionManagerInitialized += ProjectionManager_OnProjectionManagerInitialized;

        ProjectableObjectsLearningManager.OnProjectableObjectLearned += ProjectableObjectLearningManager_OnProjectableObjectLearned;
        ProjectionManager.OnProjectableObjectSelected += ProjectionManager_OnProjectableObjectSelected;
        ProjectionManager.OnProjectableObjectDeselected += ProjectionManager_OnProjectableObjectDeselected;
    }

    private void OnDisable()
    {
        ProjectionManager.OnProjectionManagerInitialized -= ProjectionManager_OnProjectionManagerInitialized;

        ProjectableObjectsLearningManager.OnProjectableObjectLearned -= ProjectableObjectLearningManager_OnProjectableObjectLearned;
        ProjectionManager.OnProjectableObjectSelected -= ProjectionManager_OnProjectableObjectSelected;
        ProjectionManager.OnProjectableObjectDeselected -= ProjectionManager_OnProjectableObjectDeselected;
    }
    private void InitializeUI()
    {
        SetSelectionContents(ProjectableObjectsLearningManager.Instance.ProjectableObjectsLearned);
        DeselectAllUI();

        SetProjectableObjectText(ProjectableObjectsLearningManager.Instance.ProjectableObjectsLearned[ProjectionManager.Instance.CurrentSelectionIndex].name);
        SelectProjectableObjectUIByIndex(ProjectionManager.Instance.CurrentSelectionIndex);
    }

    private void SetSelectionContents(List<ProjectableObjectSO> projectableObjectsSOs)
    {
        int index = 0;

        foreach (ProjectableObjectSO projectableObjectSO in projectableObjectsSOs)
        {
            Transform projectableObjectSelectionSingleUIGameObject = Instantiate(projectableObjectSelectionSingleUIPrefab, projectableObjectSelectionContainer);

            ProjectableObjectSelectionSingleUI projectableObjectSelectionUI = projectableObjectSelectionSingleUIGameObject.GetComponent<ProjectableObjectSelectionSingleUI>();

            if (!projectableObjectSelectionUI)
            {
                Debug.LogWarning("There's not a ProjectableObjectSelectionSingleUI attached to instantiated prefab");
                continue;
            }

            projectableObjectSelectionUI.SetProyectableObjectImage(projectableObjectSO.sprite);
            projectableObjectSelectionUI.SetProyectableObjectCost(projectableObjectSO.projectionGemsCost);
            projectableObjectSelectionUI.SetLinkedIndex(index);

            index++;
        }
    }

    private void DeselectAllUI()
    {
        foreach(Transform child in projectableObjectSelectionContainer)
        {
            ProjectableObjectSelectionSingleUI projectableObjectSelectionUI = child.GetComponent<ProjectableObjectSelectionSingleUI>();

            if (!projectableObjectSelectionUI)
            {
                Debug.LogWarning("There's not a ProjectableObjectSelectionSingleUI attached to instantiated prefab");
                continue;
            }

            projectableObjectSelectionUI.DeselectUI();
        }
    }

    private void SetProjectableObjectText(string objectName) => selectedProjectableObjectText.text = objectName;


    private void SelectProjectableObjectUIByIndex(int index)
    {
        foreach (Transform child in projectableObjectSelectionContainer)
        {
            ProjectableObjectSelectionSingleUI projectableObjectSelectionUI = child.GetComponent<ProjectableObjectSelectionSingleUI>();

            if (!projectableObjectSelectionUI)
            {
                Debug.LogWarning("There's not a ProjectableObjectSelectionSingleUI attached to instantiated prefab");
                continue;
            }

            if (projectableObjectSelectionUI.LinkedIndex == index)
            {
                projectableObjectSelectionUI.SelectUI();
                return;
            }
        }
    }

    private void DeselectProjectableObjectUIByIndex(int index)
    {
        foreach (Transform child in projectableObjectSelectionContainer)
        {
            ProjectableObjectSelectionSingleUI projectableObjectSelectionUI = child.GetComponent<ProjectableObjectSelectionSingleUI>();

            if (!projectableObjectSelectionUI)
            {
                Debug.LogWarning("There's not a ProjectableObjectSelectionSingleUI attached to instantiated prefab");
                continue;
            }

            if (projectableObjectSelectionUI.LinkedIndex == index)
            {
                projectableObjectSelectionUI.DeselectUI();
                return;
            }
        }
    }

    private void AddProjectableObjectToUI(ProjectableObjectSO projectableObjectSO, int indexToLink)
    {    
        Transform projectableObjectSelectionSingleUIGameObject = Instantiate(projectableObjectSelectionSingleUIPrefab, projectableObjectSelectionContainer);

        ProjectableObjectSelectionSingleUI projectableObjectSelectionUI = projectableObjectSelectionSingleUIGameObject.GetComponent<ProjectableObjectSelectionSingleUI>();

        if (!projectableObjectSelectionUI)
        {
            Debug.LogWarning("There's not a ProjectableObjectSelectionSingleUI attached to instantiated prefab");
            return;
        }

        projectableObjectSelectionUI.SetProyectableObjectImage(projectableObjectSO.sprite);
        projectableObjectSelectionUI.SetProyectableObjectCost(projectableObjectSO.projectionGemsCost);
        projectableObjectSelectionUI.SetLinkedIndex(indexToLink);
    }

    #region Events Subscriptions
    private void ProjectionManager_OnProjectionManagerInitialized(object sender, EventArgs e)
    {
        InitializeUI();     
    }

    private void ProjectableObjectLearningManager_OnProjectableObjectLearned(object sender, ProjectableObjectsLearningManager.OnProjectableObjectLearnedEventArgs e)
    {
        AddProjectableObjectToUI(e.projectableObjectLearned, ProjectableObjectsLearningManager.Instance.ProjectableObjectsLearned.Count - 1);
    }

    private void ProjectionManager_OnProjectableObjectSelected(object sender, ProjectionManager.OnSelectionEventArgs e)
    {
        SetProjectableObjectText(e.projectableObjectSO.name);
        SelectProjectableObjectUIByIndex(e.index);
    }

    private void ProjectionManager_OnProjectableObjectDeselected(object sender, ProjectionManager.OnSelectionEventArgs e)
    {
        DeselectProjectableObjectUIByIndex(e.index);
    }
    #endregion
}