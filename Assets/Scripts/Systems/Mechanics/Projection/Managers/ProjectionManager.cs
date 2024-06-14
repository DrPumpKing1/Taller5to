using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectionManager : MonoBehaviour
{
    public static ProjectionManager Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private ProjectionInput projectionInput;

    [Header("Projectable Object Settings")]
    [SerializeField] private List<ProjectableObjectSO> currentProjectedObjectsSOs = new List<ProjectableObjectSO>();
    [SerializeField] private List<ProjectableObject> currentProjectedObjectsComponents = new List<ProjectableObject>();

    [Header("Debug")]
    [SerializeField] private bool enableAllProjectableObjectsDematerialization;

    public static event EventHandler<OnProjectionEventArgs> OnObjectProjectionSuccess;
    public static event EventHandler<OnProjectionEventArgs> OnObjectProjectionFailed;
    public static event EventHandler<OnProjectionEventArgs> OnObjectDematerialized;
    public static event EventHandler<OnAllObjectsDematerializedEventArgs> OnAllObjectsDematerialized;

    public bool AllProjectableObjectDematerializationInput => projectionInput.GetAllProjectableObjectsDematerializationDown();

    public List<ProjectableObjectSO> CurrentProjectedObjectsSOs => currentProjectedObjectsSOs;
    public List<ProjectableObject> CurrentProjectedObjectsComponents => currentProjectedObjectsComponents;

    public class OnProjectionEventArgs : EventArgs
    {
        public ProjectableObjectSO projectableObjectSO;
        public ProjectionPlatform projectionPlatform;
    }

    public class OnAllObjectsDematerializedEventArgs : EventArgs
    {
        public List<ProjectableObjectSO> projectableObjectSOs;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Update()
    {
        CheckAllObjectsDematerialized();
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

    private void CheckAllObjectsDematerialized()
    {
        if (!enableAllProjectableObjectsDematerialization) return;
        if (!AllProjectableObjectDematerializationInput) return;

        OnAllObjectsDematerialized?.Invoke(this,  new OnAllObjectsDematerializedEventArgs { projectableObjectSOs = currentProjectedObjectsSOs });
    }

    public void DematerializeAllObjects()
    {
        OnAllObjectsDematerialized?.Invoke(this, new OnAllObjectsDematerializedEventArgs { projectableObjectSOs = currentProjectedObjectsSOs });
    }

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

    public void SuccessObjectProjection(ProjectableObjectSO projectableObjectSO, ProjectionPlatform projectionPlatform, ProjectableObject projectableObject)
    {
        currentProjectedObjectsSOs.Add(projectableObjectSO);
        currentProjectedObjectsComponents.Add(projectableObject);

        ProjectionGemsManager.Instance.UseProyectionGems(projectableObjectSO.projectionGemsCost);
        OnObjectProjectionSuccess?.Invoke(this, new OnProjectionEventArgs { projectableObjectSO = projectableObjectSO, projectionPlatform = projectionPlatform });
    }

    public void ObjectDematerialized(ProjectableObjectSO projectableObjectSO, ProjectionPlatform projectionPlatform, ProjectableObject projectableObject, bool triggerEvents)
    {
        currentProjectedObjectsSOs.Remove(projectableObjectSO);
        currentProjectedObjectsComponents.Remove(projectableObject);

        ProjectionGemsManager.Instance.RefundProyectionGems(projectableObjectSO.projectionGemsCost);

        if(triggerEvents) OnObjectDematerialized?.Invoke(this, new OnProjectionEventArgs { projectableObjectSO = projectableObjectSO, projectionPlatform = projectionPlatform });
    }

    public bool AnyObjectsProjected() => currentProjectedObjectsSOs.Count > 0;
}
