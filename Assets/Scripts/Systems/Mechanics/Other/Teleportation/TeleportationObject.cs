using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationObject : MonoBehaviour, IInteractable
{
    [Header("Identifiers")]
    [SerializeField] protected int id;

    [Header("Interactable Settings")]
    [SerializeField, Range(1f, 100f)] protected float horizontalInteractionRange;
    [SerializeField, Range(1f, 100f)] protected float verticalInteractionRange;
    [Space]
    [SerializeField] protected bool canBeSelected;
    [SerializeField] protected bool isInteractable;
    [SerializeField] protected bool hasAlreadyBeenInteracted;
    [Space]
    [SerializeField] protected bool grabPetAttention;
    [SerializeField] protected bool grabPlayerAttention;
    [SerializeField] protected Transform interactionAttentionTransform;
    [SerializeField] protected Transform interactionPositionTransform;
    [Space]
    [SerializeField] protected string tooltipMessage;

    public static event EventHandler<OnTeleportationObjectInteractedEventArgs> OnTeleportationObjectInteracted;

    public class OnTeleportationObjectInteractedEventArgs : EventArgs
    {
        public int id;
    }

    #region IInteractable Properties
    public float HorizontalInteractionRange => horizontalInteractionRange;
    public float VerticalInteractionRange => verticalInteractionRange;
    public bool IsSelectable => canBeSelected;
    public bool IsInteractable => isInteractable;
    public bool HasAlreadyBeenInteracted => hasAlreadyBeenInteracted;
    public string TooltipMessage => tooltipMessage;
    public bool GrabPetAttention => grabPetAttention;
    public bool GrabPlayerAttention => grabPlayerAttention;
    #endregion

    #region IInteractableEvents
    public event EventHandler OnObjectSelected;
    public event EventHandler OnObjectDeselected;
    public event EventHandler OnObjectInteracted;
    public event EventHandler OnObjectFailInteracted;
    public event EventHandler OnObjectHasAlreadyBeenInteracted;
    public event EventHandler OnUpdatedInteractableState;
    #endregion

    #region  IInteractable Methods
    public void Select()
    {
        //Enable some UI feedback
        OnObjectSelected?.Invoke(this, EventArgs.Empty);
        //Debug.Log(gameObject.name + " Selected");
    }

    public void Deselect()
    {
        //Disable some UI feedback
        OnObjectDeselected?.Invoke(this, EventArgs.Empty);
        //Debug.Log(gameObject.name + " Deselected");
    }

    public void TryInteract()
    {
        if (hasAlreadyBeenInteracted)
        {
            AlreadyInteracted();
            return;
        }

        if (!isInteractable)
        {
            FailInteract();
            return;
        }

        Interact();
    }

    public void Interact()
    {
        OnObjectInteracted?.Invoke(this, EventArgs.Empty);
        InteractElement();
        OnUpdatedInteractableState?.Invoke(this, EventArgs.Empty);
    }

    public void FailInteract()
    {
        Debug.Log(gameObject.name + " Fail Interacted");
        OnObjectFailInteracted?.Invoke(this, EventArgs.Empty);
    }

    public void AlreadyInteracted()
    {
        Debug.Log(gameObject.name + " Has Already Been Interacted");
        OnObjectHasAlreadyBeenInteracted?.Invoke(this, EventArgs.Empty);
    }

    public Transform GetTransform() => transform;
    public Transform GetInteractionAttentionTransform() => interactionAttentionTransform;
    public Transform GetInteractionPositionTransform() => interactionPositionTransform;
    #endregion

    private void InteractElement()
    {
        OnTeleportationObjectInteracted?.Invoke(this, new OnTeleportationObjectInteractedEventArgs { id = id });
    }
}
