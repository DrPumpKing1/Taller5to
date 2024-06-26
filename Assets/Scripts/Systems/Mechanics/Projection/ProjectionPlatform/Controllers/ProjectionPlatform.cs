using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionPlatform : MonoBehaviour
{
    [Header("Identifiers")]
    [SerializeField] private int id;

    [Header("Projection Platform Settings")]
    [SerializeField] private Transform projectionPoint;
    [SerializeField] private ProjectableObjectSO currentProjectedObjectSO;
    [SerializeField] private ProjectableObject currentProjectedObject;

    [Header("Projectable Object Rotation Settings")]
    [SerializeField] private Vector2 startingDirection;

    [Header("Object Avobe Check Settings")]
    [SerializeField] private LayerMask objectAvobeLayers;
    [SerializeField] private Vector3 checkBoxCenter;
    [SerializeField] private Vector3 checkBoxHalfExtends;

    public Transform ProjectionPoint => projectionPoint;
    public ProjectableObjectSO CurrentProjectedObjectSO => currentProjectedObjectSO;
    public ProjectableObject CurrentProjectedObject => currentProjectedObject;
    public Vector2 StartingDirection => startingDirection;
    public int ID => id;

    public bool ObjectAbove;
    public bool useObjectAbove;

    public event EventHandler OnProjectionPlatformClear;
    public event EventHandler<OnProjectionEventArgs> OnProjectionPlatformSet;
    public event EventHandler OnProjectionPlatformDestroyed;

    public class OnProjectionEventArgs : EventArgs
    {
        public ProjectableObjectSO projectableObjectSO;
        public ProjectableObject projectableObject;
    }

    private void FixedUpdate()
    {
        ObjectAbove = CheckObjectAbove() && useObjectAbove;
    }

    private bool CheckObjectAbove()
    {
        bool objectAbove = Physics.CheckBox(transform.position + transform.TransformDirection(checkBoxCenter), checkBoxHalfExtends, transform.rotation, objectAvobeLayers);
        return objectAbove;
    }

    public void ClearProjectionPlatform()
    {
        currentProjectedObjectSO = null;
        currentProjectedObject = null;
        OnProjectionPlatformClear?.Invoke(this, EventArgs.Empty);
    }

    public void SetProjectionPlatform(ProjectableObjectSO projectableObjectSO, ProjectableObject projectableObject)
    {
        currentProjectedObjectSO = projectableObjectSO;
        currentProjectedObject = projectableObject;
        OnProjectionPlatformSet?.Invoke(this, new OnProjectionEventArgs { projectableObjectSO = projectableObjectSO, projectableObject = projectableObject });
    }

    public void DestroyProjectionPlatform()
    {
        OnProjectionPlatformDestroyed?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
    }

    public bool HasObject() => currentProjectedObject != null;
}
