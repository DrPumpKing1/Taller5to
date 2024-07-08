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
    [SerializeField] private bool useObjectAbove;
    [SerializeField] private LayerMask objectAvobeLayers;
    [SerializeField] private Vector3 checkBoxCenter;
    [SerializeField] private Vector3 checkBoxHalfExtends;

    public bool ObjectAbove { get; private set; }
    private GameObject player;

    private const string PLAYER_TAG = "Player";
    private const float DISTANCE_TO_UPDATE = 10f;

    public Transform ProjectionPoint => projectionPoint;
    public ProjectableObjectSO CurrentProjectedObjectSO => currentProjectedObjectSO;
    public ProjectableObject CurrentProjectedObject => currentProjectedObject;
    public Vector2 StartingDirection => startingDirection;
    public int ID => id;


    public event EventHandler OnProjectionPlatformClear;
    public event EventHandler<OnProjectionEventArgs> OnProjectionPlatformSet;
    public event EventHandler OnProjectionPlatformDestroyed;

    public class OnProjectionEventArgs : EventArgs
    {
        public ProjectableObjectSO projectableObjectSO;
        public ProjectableObject projectableObject;
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag(PLAYER_TAG);
    }

    private void FixedUpdate()
    {
        ObjectAbove = CheckObjectAbove() && useObjectAbove;
    }

    private bool CheckObjectAbove()
    {
        if (!CheckPlayerClose()) return false;

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

    private bool CheckPlayerClose()
    {
        if (!player) return true;
        if (Vector3.Distance(transform.position, player.transform.position) <= DISTANCE_TO_UPDATE) return true;

        return false;
    }
}
