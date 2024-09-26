using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectableObjectRotation : MonoBehaviour, IInteractableAlternate
{
    [Header("Components")]
    [SerializeField] private Transform transformToRotate;
    [SerializeField] private ProjectableObject projectableObject;

    [Header("Rotation Settings")]
    [SerializeField] private bool clockwiseRotation;
    [SerializeField] private int degreesPerTurn;
    [SerializeField, Range(1f, 100f)] private float smoothRotateFactor;

    [Header("Interactable Alternate Settings")]
    [SerializeField, Range(1f, 100f)] private float horizontalInteractionRange;
    [SerializeField, Range(1f, 100f)] private float verticalInteractionRange;
    [Space]
    [SerializeField] private bool canBeSelectedAlternate;
    [SerializeField] private bool isInteractableAlternate;
    [SerializeField] private bool hasAlreadyBeenInteractedAlternate;
    [SerializeField] private string tooltipMessageAlternate;
    [Space]
    [SerializeField] private bool grabPetAttention;
    [SerializeField] private bool grabPlayerAttention;
    [SerializeField] private Transform interactionAttentionTransform;
    [SerializeField] private Transform interactionPositionTransform;

    public static event EventHandler<OnAnyObjectRotatedEventArgs> OnAnyObjectRotated;

    public class OnAnyObjectRotatedEventArgs : EventArgs
    {
        public ProjectableObjectSO projectableObjectSO;
        public int projectionPlatformID;
    }

    #region IInteractableAlternate Properties
    public float HorizontalInteractionRange => horizontalInteractionRange;
    public float VerticalInteractionRange => verticalInteractionRange;
    public bool IsSelectableAlternate => canBeSelectedAlternate;
    public bool IsInteractableAlternate => isInteractableAlternate;
    public bool HasAlreadyBeenInteractedAlternate => hasAlreadyBeenInteractedAlternate;
    public string TooltipMessageAlternate => tooltipMessageAlternate;
    public bool GrabPetAttention => grabPetAttention;
    public bool GrabPlayerAttention => grabPlayerAttention;
    #endregion

    #region IInteractableAlternate Events
    public event EventHandler OnObjectSelectedAlternate;
    public event EventHandler OnObjectDeselectedAlternate;
    public event EventHandler OnObjectInteractedAlternate;
    public event EventHandler OnObjectFailInteractedAlternate;
    public event EventHandler OnObjectHasAlreadyBeenInteractedAlternate;
    public event EventHandler OnUpdatedInteractableAlternateState;
    #endregion

    public Vector3 DesiredDirection { get; private set; }
    public Vector3 CurrentDirection { get; private set; }

    private void OnEnable()
    {
        projectableObject.OnProjectionPlatformSet += ProjectableObject_OnProjectionPlatformSet;
    }

    private void OnDisable()
    {
        projectableObject.OnProjectionPlatformSet -= ProjectableObject_OnProjectionPlatformSet;
    }

    private void Update()
    {
        HandleRotation();
    }

    #region IInteractableAlternateMethods

    public void SelectAlternate()
    {
        OnObjectSelectedAlternate?.Invoke(this, EventArgs.Empty);
        //Debug.Log("ProjectableObject Selected Alternate");
    }
    public void DeselectAlternate()
    {
        OnObjectDeselectedAlternate?.Invoke(this, EventArgs.Empty);
        //Debug.Log("ProjectableObject Deselected Alternate");
    }

    public void TryInteractAlternate()
    {
        if (!isInteractableAlternate)
        {
            FailInteractAlternate();
            return;
        }

        if (hasAlreadyBeenInteractedAlternate)
        {
            AlreadyInteractedAlternate();
            return;
        }

        InteractAlternate();
    }
    public void InteractAlternate()
    {
        //Debug.Log("ProjectableObject Interacted Alternate");
        OnObjectInteractedAlternate?.Invoke(this, EventArgs.Empty);

        RotateObject();
    }

    public void FailInteractAlternate()
    {
        Debug.Log("Cant InteractAlternate with ProjectableObject");
        OnObjectFailInteractedAlternate?.Invoke(this, EventArgs.Empty);
    }

    public void AlreadyInteractedAlternate()
    {
        Debug.Log("ProjectableObject has Already Been Interacted Alternate");
        OnObjectHasAlreadyBeenInteractedAlternate?.Invoke(this, EventArgs.Empty);
    }

    public Transform GetTransform() => transform;
    public Transform GetInteractionAlternateAttentionTransform() => interactionAttentionTransform;
    public Transform GetInteractionAlternatePositionTransform() => interactionPositionTransform;

    #endregion

    private void InitializeRotation(Vector2 startingDirection)
    {
        Vector3 initialDirectionVector3 = GeneralMethods.Vector2ToVector3(startingDirection);
        initialDirectionVector3 = initialDirectionVector3.magnitude == 0 ? transformToRotate.forward : initialDirectionVector3;

        DesiredDirection = initialDirectionVector3.normalized;
        CurrentDirection = DesiredDirection;

        transformToRotate.localRotation = Quaternion.LookRotation(DesiredDirection);
    }

    private void HandleRotation()
    {
        CurrentDirection = Vector3.Slerp(CurrentDirection, DesiredDirection, smoothRotateFactor * Time.deltaTime);
        CurrentDirection.Normalize();

        transformToRotate.localRotation = Quaternion.LookRotation(CurrentDirection);
    }

    private void RotateObject()
    {
        float degreesToTurn = clockwiseRotation ? degreesPerTurn : -degreesPerTurn;
        DesiredDirection = (Quaternion.AngleAxis(degreesToTurn, Vector3.up) * DesiredDirection).normalized;

        OnAnyObjectRotated?.Invoke(this, new OnAnyObjectRotatedEventArgs { projectableObjectSO = projectableObject.ProjectableObjectSO , projectionPlatformID = projectableObject.ProjectionPlatform.ID});
        OnUpdatedInteractableAlternateState?.Invoke(this, EventArgs.Empty);
    }

    #region ProjectableObject Subscriptions
    private void ProjectableObject_OnProjectionPlatformSet(object sender, ProjectableObject.OnProjectionPlatformSetEventArgs e)
    {
        InitializeRotation(e.projectionPlatform.StartingDirection);
    }
    #endregion
}
