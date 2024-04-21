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

    private List<ProjectableObjectSO> AvailableProjectableObjects => LearningManager.Instance.ObjectsLearned;

    private void OnEnable()
    {
        LearningManager.OnObjectLearned += LearningManager_OnObjectLearned;
        ProjectionManager.OnObjectSelected += ProjectionManager_OnObjectSelected;
        ProjectionManager.OnObjectDeselected += ProjectionManager_OnObjectDeselected;
    }


    private void OnDisable()
    {
        LearningManager.OnObjectLearned -= LearningManager_OnObjectLearned;
        ProjectionManager.OnObjectSelected -= ProjectionManager_OnObjectSelected;
        ProjectionManager.OnObjectDeselected -= ProjectionManager_OnObjectDeselected;
    }

    private void Start()
    {
        SetSelectionContents(AvailableProjectableObjects);
        DeselectAllUI();
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

    private void SetProjectableObjectText(ProjectableObjectSO projectableObjectSO) => selectedProjectableObjectText.text = projectableObjectSO.name;

    private void LearningManager_OnObjectLearned(object sender, LearningManager.OnObjectLearnedEventArgs e)
    {
        int index = LearningManager.Instance.ObjectsLearned.Count - 1;

        Transform projectableObjectSelectionSingleUIGameObject = Instantiate(projectableObjectSelectionSingleUIPrefab, projectableObjectSelectionContainer);

        ProjectableObjectSelectionSingleUI projectableObjectSelectionUI = projectableObjectSelectionSingleUIGameObject.GetComponent<ProjectableObjectSelectionSingleUI>();

        if (!projectableObjectSelectionUI)
        {
            Debug.LogWarning("There's not a ProjectableObjectSelectionSingleUI attached to instantiated prefab");
            return;
        }

        projectableObjectSelectionUI.SetProyectableObjectImage(e.projectableObjectLearned.sprite);
        projectableObjectSelectionUI.SetLinkedIndex(index);
    }

    private void ProjectionManager_OnObjectSelected(object sender, ProjectionManager.OnSelectionEventArgs e)
    {
        SetProjectableObjectText(e.projectableObjectSO);

        foreach (Transform child in projectableObjectSelectionContainer)
        {
            ProjectableObjectSelectionSingleUI projectableObjectSelectionUI = child.GetComponent<ProjectableObjectSelectionSingleUI>();

            if (!projectableObjectSelectionUI)
            {
                Debug.LogWarning("There's not a ProjectableObjectSelectionSingleUI attached to instantiated prefab");
                continue;
            }

            if(projectableObjectSelectionUI.LinkedIndex == e.index)
            {
                projectableObjectSelectionUI.SelectUI();
                return;
            }   
        }
    }

    private void ProjectionManager_OnObjectDeselected(object sender, ProjectionManager.OnSelectionEventArgs e)
    {
        Debug.Log(sender);

        foreach (Transform child in projectableObjectSelectionContainer)
        {
            ProjectableObjectSelectionSingleUI projectableObjectSelectionUI = child.GetComponent<ProjectableObjectSelectionSingleUI>();

            if (!projectableObjectSelectionUI)
            {
                Debug.LogWarning("There's not a ProjectableObjectSelectionSingleUI attached to instantiated prefab");
                continue;
            }

            if (projectableObjectSelectionUI.LinkedIndex == e.index)
            {
                projectableObjectSelectionUI.DeselectUI();
                return;
            }
        }
    }
}
