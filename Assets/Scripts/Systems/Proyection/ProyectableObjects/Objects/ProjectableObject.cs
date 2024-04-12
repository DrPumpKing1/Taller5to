using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectableObject : MonoBehaviour, IHoldInteractable, IInteractableAlternate
{
    [Header("Projectable Object Settings")]
    [SerializeField] private ProjectableObjectSO projectableObjectSO;
    [SerializeField] private ProjectionPlatform projectionPlatform;

    [Header("Rotation Settings")]
    [SerializeField] private Vector2 startingDirection;
    [SerializeField] private bool clockwiseRotation;
    [SerializeField] private int degreesPerTurn;
    [SerializeField,Range(1f,100f)] private float smoothRotateFactor;

    [Header("Interactable Settings")]
    [SerializeField] private bool canBeSelected;
    [SerializeField] private bool isInteractable;
    [SerializeField] private bool hasAlreadyBeenInteracted;
    [SerializeField] private string tooltipMessage;
    [Space]
    [SerializeField] private float holdDuration;

    [Header("Interactable Alternate Settings")]
    [SerializeField] private bool canBeSelectedAlternate;
    [SerializeField] private bool isInteractableAlternate;
    [SerializeField] private bool hasAlreadyBeenInteractedAlternate;
    [SerializeField] private string tooltipMessageAlternate;

    #region IInteractable Properties
    public bool IsSelectable => canBeSelected;
    public bool IsInteractable => isInteractable;
    public bool HasAlreadyBeenInteracted => hasAlreadyBeenInteracted;
    public string TooltipMessage => tooltipMessage;
    public float HoldDuration => holdDuration;
    #endregion

    #region IInteractableAlternate Properties
    public bool IsSelectableAlternate => canBeSelectedAlternate;
    public bool IsInteractableAlternate => isInteractableAlternate;
    public bool HasAlreadyBeenInteractedAlternate => hasAlreadyBeenInteractedAlternate;
    public string TooltipMessageAlternate => tooltipMessageAlternate;
    #endregion

    #region IHoldInteractable Events
    public event EventHandler OnObjectSelected;
    public event EventHandler OnObjectDeselected;
    public event EventHandler OnObjectInteracted;
    public event EventHandler OnObjectFailInteracted;
    public event EventHandler OnObjectHasAlreadyBeenInteracted;

    public event EventHandler OnHoldInteractionStart;
    public event EventHandler OnHoldInteractionEnd;
    public event EventHandler<IHoldInteractable.OnHoldInteractionEventArgs> OnContinousHoldInteraction;
    #endregion

    #region IInteractableAlternate Events
    public event EventHandler OnObjectSelectedAlternate;
    public event EventHandler OnObjectDeselectedAlternate;
    public event EventHandler OnObjectInteractedAlternate;
    public event EventHandler OnObjectFailInteractedAlternate;
    public event EventHandler OnObjectHasAlreadyBeenInteractedAlternate;
    #endregion

    public Vector3 DesiredDirection { get; private set; }
    public Vector3 CurrentDirection { get; private set; }

    public event EventHandler OnObjectDematerialized;

    protected virtual void Start()
    {
        InitializeRotation();
    }

    protected virtual void Update()
    {
        HandleRotation();
    }

    #region IHoldInteractable Methods
    public void Select()
    {
        OnObjectSelected?.Invoke(this, EventArgs.Empty);
        Debug.Log("ProjectableObject Selected");
    }
    public void Deselect()
    {
        OnObjectDeselected?.Invoke(this, EventArgs.Empty);
        Debug.Log("ProjectableObject Deselected");
    }

    public void TryInteract()
    {
        if (!isInteractable)
        {
            FailInteract();
            return;
        }

        if (hasAlreadyBeenInteracted)
        {
            AlreadyInteracted();
            return;
        }

        Interact();
    }

    public void Interact()
    {
        Debug.Log("ProjectableObject Interacted");
        OnObjectInteracted?.Invoke(this, EventArgs.Empty);

        DematerializeObject();
    }

    public void FailInteract()
    {
        Debug.Log("Cant Interact with ProjectableObject");
        OnObjectFailInteracted?.Invoke(this, EventArgs.Empty);
    }
    public void AlreadyInteracted()
    {
        Debug.Log("ProjectableObject has Already Been Interacted");
        OnObjectHasAlreadyBeenInteracted?.Invoke(this, EventArgs.Empty);
    }
    public bool CheckSuccess()
    {
        if (!isInteractable)
        {
            FailInteract();
            return false;
        }

        if (hasAlreadyBeenInteracted)
        {
            AlreadyInteracted();
            return false;
        }

        return true;
    }
    public void HoldInteractionStart() => OnHoldInteractionStart?.Invoke(this, EventArgs.Empty);
    public void ContinousHoldInteraction(float holdTimer) => OnContinousHoldInteraction?.Invoke(this, new IHoldInteractable.OnHoldInteractionEventArgs { holdTimer = holdTimer });
    public void HoldInteractionEnd() => OnHoldInteractionEnd?.Invoke(this, EventArgs.Empty);


    public Transform GetTransform() => transform;

    #endregion

    #region IInteractableAlternateMethods

    public void SelectAlternate()
    {
        OnObjectSelectedAlternate?.Invoke(this, EventArgs.Empty);
        Debug.Log("ProjectableObject Selected Alternate");
    }
    public void DeselectAlternate()
    {
        OnObjectDeselectedAlternate?.Invoke(this, EventArgs.Empty);
        Debug.Log("ProjectableObject Deselected Alternate");
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
        Debug.Log("ProjectableObject Interacted Alternate");
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

    #endregion

    private void InitializeRotation()
    {
        Vector3 initialDirectionVector3 = GeneralMethods.Vector2ToVector3(startingDirection);
        initialDirectionVector3 = initialDirectionVector3.magnitude == 0 ? transform.forward : initialDirectionVector3;

        DesiredDirection = initialDirectionVector3.normalized;
        CurrentDirection = DesiredDirection;

        transform.localRotation = Quaternion.LookRotation(DesiredDirection);
    }

    private void HandleRotation()
    {
        CurrentDirection = Vector3.Slerp(CurrentDirection, DesiredDirection, smoothRotateFactor * Time.deltaTime);
        CurrentDirection.Normalize();

        transform.localRotation = Quaternion.LookRotation(CurrentDirection);
    }

    private void RotateObject()
    {
        float degreesToTurn = clockwiseRotation ? degreesPerTurn : -degreesPerTurn;
        DesiredDirection = (Quaternion.AngleAxis(degreesToTurn, Vector3.up) * DesiredDirection).normalized;
    }
    private void DematerializeObject()
    {
        ProjectionManager.Instance.ObjectDematerialized(projectableObjectSO, projectionPlatform);
        OnObjectDematerialized?.Invoke(this, EventArgs.Empty);

        if(projectionPlatform) projectionPlatform.ResetProjectionPlatform();

        Destroy(gameObject);
    }

    public void SetProjectionPlatform(ProjectionPlatform projectionPlatform) => this.projectionPlatform = projectionPlatform;
}
