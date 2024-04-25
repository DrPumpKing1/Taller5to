using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionPlatform : MonoBehaviour
{
    [Header("Projection Platform Settings")]
    [SerializeField] private Transform projectionPoint;
    [SerializeField] private ProjectableObjectSO currentProjectedObject;

    [Header("Object Avobe Check Settings")]
    [SerializeField] private LayerMask objectAvobeLayers;
    [SerializeField] private Vector3 checkBoxCenter;
    [SerializeField] private Vector3 checkBoxHalfExtends;

    public Transform ProjectionPoint { get { return projectionPoint; } }
    public ProjectableObjectSO CurrentProjectedObject { get { return currentProjectedObject; } }
    public bool ObjectAbove;

    public event EventHandler OnProjectionPlatformClear;
    public event EventHandler<OnProjectionEventArgs> OnProjectionPlatformSet;


    public class OnProjectionEventArgs : EventArgs
    {
        public ProjectableObjectSO projectableObjectSO;
    }

    private void FixedUpdate()
    {
        ObjectAbove = CheckObjectAbove();
    }

    private bool CheckObjectAbove()
    {
        bool objectAbove = Physics.CheckBox(transform.position + checkBoxCenter, checkBoxHalfExtends, Quaternion.identity, objectAvobeLayers);

        return objectAbove;
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
