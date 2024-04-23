using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectionManager : MonoBehaviour
{
    public static ProjectionManager Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private PlayerInteractAlternate playerInteractAlternate;
    [SerializeField] private ProjectionInput projectionInput;

    [Header("Projectable Object Settings")]
    [SerializeField] private ProjectableObjectSO selectedProjectableObjectSO;
    [SerializeField] private List<ProjectableObjectSO> currentProjectedObjects = new List<ProjectableObjectSO>();

    [Header("Selection Settings")]
    [SerializeField] private int InitialSelectionIndex;

    public static event EventHandler<OnProjectionEventArgs> OnObjectProjectionSuccess;
    public static event EventHandler<OnProjectionEventArgs> OnObjectProjectionFailed;
    public static event EventHandler<OnProjectionEventArgs> OnObjectDematerialized;

    public static event EventHandler<OnSelectionEventArgs> OnProjectableObjectSelected;
    public static event EventHandler<OnSelectionEventArgs> OnProjectableObjectDeselected;

    public static event EventHandler OnProjectionManagerInitialized;

    public List<ProjectableObjectSO> CurrentProjectedObjects { get { return currentProjectedObjects; } }
    private List<ProjectableObjectSO> availableProjectableObjects => ProjectableObjectsLearningManager.Instance.ProjectableObjectsLearned;

    public ProjectableObjectSO SelectedProjectableObjectSO { get { return selectedProjectableObjectSO; } }

    private bool SelectionInputNext => projectionInput.GetNextProjectableObjectDown();
    private bool SelectionInputPrevious => projectionInput.GetPreviousProjectableObjectDown();
    public int CurrentSelectionIndex { get; private set; }

    public class OnProjectionEventArgs : EventArgs
    {
        public ProjectableObjectSO projectableObjectSO;
        public ProjectionPlatform projectionPlatform;
    }

    public class OnSelectionEventArgs : EventArgs
    {
        public int index;
        public ProjectableObjectSO projectableObjectSO;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void Update()
    {
        HandleProjectableObjectSelection();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one ProjectionManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeVariables()
    {
        CurrentSelectionIndex = InitialSelectionIndex;
        SelectProjectableObject(availableProjectableObjects[InitialSelectionIndex]);

        OnProjectionManagerInitialized?.Invoke(this, EventArgs.Empty);
    }


    #region ProjectableObject Selection
    private void HandleProjectableObjectSelection()
    {
        HandleProjectableObjectSelectionNext();
        HandleProjectableObjectSelectionPrevious();
    }

    private void HandleProjectableObjectSelectionNext()
    {
        if (playerInteract.IsInteracting) return;
        if (playerInteractAlternate.IsInteractingAlternate) return;
        if (!SelectionInputNext) return;

        int previousIndex = CurrentSelectionIndex;
        int maxIndex = availableProjectableObjects.Count - 1;

        int desiredIndex = CurrentSelectionIndex + 1;

        CurrentSelectionIndex = desiredIndex > maxIndex ? 0 : desiredIndex;

        SelectProjectableObject(availableProjectableObjects[CurrentSelectionIndex]);

        OnProjectableObjectDeselected?.Invoke(this,new OnSelectionEventArgs { index = previousIndex, projectableObjectSO = availableProjectableObjects[previousIndex] });
        OnProjectableObjectSelected?.Invoke(this,new OnSelectionEventArgs { index = CurrentSelectionIndex, projectableObjectSO = availableProjectableObjects[CurrentSelectionIndex] });
    }

    private void HandleProjectableObjectSelectionPrevious()
    {
        if (playerInteract.IsInteracting) return;
        if (playerInteractAlternate.IsInteractingAlternate) return;
        if (!SelectionInputPrevious) return;

        int previousIndex = CurrentSelectionIndex;
        int maxIndex = availableProjectableObjects.Count - 1;

        int desiredIndex = CurrentSelectionIndex - 1;

        CurrentSelectionIndex = desiredIndex < 0 ? maxIndex : desiredIndex;

        SelectProjectableObject(availableProjectableObjects[CurrentSelectionIndex]);

        OnProjectableObjectDeselected?.Invoke(this, new OnSelectionEventArgs { index = previousIndex, projectableObjectSO = availableProjectableObjects[previousIndex] });
        OnProjectableObjectSelected?.Invoke(this, new OnSelectionEventArgs { index = CurrentSelectionIndex, projectableObjectSO = availableProjectableObjects[CurrentSelectionIndex] });
    }


    private void SelectProjectableObject(ProjectableObjectSO projectableObjectSO)
    {
        if (!projectableObjectSO) return;
        selectedProjectableObjectSO = projectableObjectSO; 
    }


    #endregion

    #region ProjectableObject Projection
    public bool CanProjectObject(ProjectableObjectSO projectableObjectSO) => ProjectionGemsManager.Instance.CheckCanUseProjectionGems(projectableObjectSO.projectionGemsCost);

    public void FailObjectProjection(ProjectableObjectSO projectableObjectSO, ProjectionPlatform projectionPlatform)
    {
        OnObjectProjectionFailed?.Invoke(this, new OnProjectionEventArgs { projectableObjectSO = projectableObjectSO, projectionPlatform = projectionPlatform });
    }

    public void FailObjectProjectionInsuficientGems(ProjectableObjectSO projectableObjectSO, ProjectionPlatform projectionPlatform)
    {
        OnObjectProjectionFailed?.Invoke(this, new OnProjectionEventArgs { projectableObjectSO = projectableObjectSO, projectionPlatform = projectionPlatform });
        ProjectionGemsManager.Instance.InsuficientProjectionGems(projectableObjectSO.projectionGemsCost);
    }

    public void SuccessObjectProjection(ProjectableObjectSO projectableObjectSO, ProjectionPlatform projectionPlatform)
    {
        currentProjectedObjects.Add(projectableObjectSO);

        ProjectionGemsManager.Instance.UseProyectionGems(projectableObjectSO.projectionGemsCost);
        OnObjectProjectionSuccess?.Invoke(this, new OnProjectionEventArgs { projectableObjectSO = projectableObjectSO, projectionPlatform = projectionPlatform });
    }

    public void ObjectDematerialized(ProjectableObjectSO projectableObjectSO, ProjectionPlatform projectionPlatform)
    {
        currentProjectedObjects.Remove(projectableObjectSO);

        ProjectionGemsManager.Instance.RefundProyectionGems(projectableObjectSO.projectionGemsCost);
        OnObjectDematerialized?.Invoke(this, new OnProjectionEventArgs { projectableObjectSO = projectableObjectSO, projectionPlatform = projectionPlatform });
    }

    #endregion
}
