using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectableObject : MonoBehaviour
{
    [Header("Projectable Object Settings")]
    [SerializeField] private ProjectableObjectSO projectableObjectSO;
    [SerializeField] private ProjectionPlatform projectionPlatform;
    [SerializeField] private Transform projectableObjectCenter;

    public event EventHandler<OnProjectionPlatformSetEventArgs> OnProjectionPlatformSet;
    public event EventHandler OnProjectableObjectDestroyed;
    public static event EventHandler<OnAnyObjectDestroyedEventArgs> OnAnyObjectDestroyed;

    public class OnAnyObjectDestroyedEventArgs : EventArgs
    {
        public ProjectableObjectSO projectableObjectSO;
    }

    public class OnProjectionPlatformSetEventArgs : EventArgs
    {
        public ProjectionPlatform projectionPlatform;
    }

    public ProjectableObjectSO ProjectableObjectSO => projectableObjectSO;
    public ProjectionPlatform ProjectionPlatform => projectionPlatform;
    public Transform ProjectableObjectCenter => projectableObjectCenter;

    public void SetProjectionPlatform(ProjectionPlatform projectionPlatform)
    {
        this.projectionPlatform = projectionPlatform;
        OnProjectionPlatformSet?.Invoke(this, new OnProjectionPlatformSetEventArgs { projectionPlatform = projectionPlatform });
    }

    public void DestroyProjectableObject()
    {
        if (projectionPlatform) projectionPlatform.ClearProjectionPlatform();

        ProjectionManager.Instance.ObjectDestroyed(projectableObjectSO, projectionPlatform, this, false);
        OnProjectableObjectDestroyed?.Invoke(this, EventArgs.Empty);
        OnAnyObjectDestroyed?.Invoke(this, new OnAnyObjectDestroyedEventArgs { projectableObjectSO = projectableObjectSO });

        Destroy(gameObject);
    }
}
