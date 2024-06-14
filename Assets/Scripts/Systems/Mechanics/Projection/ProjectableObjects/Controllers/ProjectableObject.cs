using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectableObject : MonoBehaviour
{
    [Header("Projectable Object Settings")]
    [SerializeField] private ProjectableObjectSO projectableObjectSO;
    [SerializeField] private ProjectionPlatform projectionPlatform;

    public event EventHandler<OnProjectionPlatformSetEventArgs> OnProjectionPlatformSet;

    public class OnProjectionPlatformSetEventArgs : EventArgs
    {
        public ProjectionPlatform projectionPlatform;
    }

    public ProjectableObjectSO ProjectableObjectSO { get { return projectableObjectSO ; } }
    public ProjectionPlatform ProjectionPlatform { get { return projectionPlatform; } }

    public void SetProjectionPlatform(ProjectionPlatform projectionPlatform)
    {
        this.projectionPlatform = projectionPlatform;
        OnProjectionPlatformSet?.Invoke(this, new OnProjectionPlatformSetEventArgs { projectionPlatform = projectionPlatform });
    }
}
