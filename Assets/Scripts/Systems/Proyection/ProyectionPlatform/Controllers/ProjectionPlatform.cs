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
    [SerializeField] private Vector3 originOffset;
    [SerializeField] private float rayLenght;


    [Header("Debug")]
    [SerializeField] private bool drawRaycasts;

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
        bool objectAbove = Physics.BoxCast(transform.position + new Vector3(0, 1.25f, 0), new Vector3(1, 1, 1),transform.up, Quaternion.identity, rayLenght, objectAvobeLayers);

        if (drawRaycasts) Debug.DrawRay(transform.position, transform.up * (rayLenght), Color.red);

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
