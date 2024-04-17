using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionPlatform : MonoBehaviour
{
    [Header("Projection Platform Settings")]
    [SerializeField] private Transform projectionPoint;
    [SerializeField] private ProjectableObjectSO currentProjectedObject;

    public Transform ProjectionPoint { get { return projectionPoint; } }
    public ProjectableObjectSO CurrentProjectedObject { get { return currentProjectedObject; } }

    public event EventHandler OnProjectionPlatformClear;
    public event EventHandler<OnProjectionEventArgs> OnProjectionPlatformSet;

    public class OnProjectionEventArgs: EventArgs
    {
        public ProjectableObjectSO projectableObjectSO;
    }

    public void ClearProjectionPlatform() 
    {
        currentProjectedObject = null;
        OnProjectionPlatformClear?.Invoke(this, EventArgs.Empty);
    }

    public void SetProjectionPlatform(ProjectableObjectSO projectableObjectSO)
    {
        currentProjectedObject = projectableObjectSO;
        OnProjectionPlatformSet?.Invoke(this, new OnProjectionEventArgs { projectableObjectSO = projectableObjectSO });
    }
}
