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
    public bool ObjectAvobe;

    public event EventHandler OnProjectionPlatformClear;
    public event EventHandler<OnProjectionEventArgs> OnProjectionPlatformSet;


    public class OnProjectionEventArgs : EventArgs
    {
        public ProjectableObjectSO projectableObjectSO;
    }

    private void FixedUpdate()
    {
        ObjectAvobe = CheckObjectAvobe();
    }

    private bool CheckObjectAvobe()
    {
        bool objectAvobe = Physics.Raycast(transform.position, transform.up, rayLenght, objectAvobeLayers);

        if (drawRaycasts) Debug.DrawRay(transform.position, transform.up * (rayLenght), Color.red);

        return objectAvobe;
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
