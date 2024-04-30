using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    [Header("Interactable Settings")]
    [SerializeField] private bool canBeSelected;
    [SerializeField] private bool isInteractable;
    [SerializeField] private bool hasAlreadyBeenInteracted;
    [SerializeField] private string tooltipMessage;

    public bool IsSelectable => canBeSelected;
    public bool IsInteractable => isInteractable;
    public bool HasAlreadyBeenInteracted => hasAlreadyBeenInteracted;
    public string TooltipMessage => tooltipMessage;

    public event EventHandler OnObjectSelected; 
    public event EventHandler OnObjectDeselected;
    public event EventHandler OnObjectInteracted;
    public event EventHandler OnObjectFailInteracted;
    public event EventHandler OnObjectHasAlreadyBeenInteracted;
    public event EventHandler OnUpdatedInteractableState;

    #region IInteractable
    public void Select()
    {
        OnObjectSelected?.Invoke(this, EventArgs.Empty);
        Debug.Log(gameObject.name + " Selected");
    }
    public void Deselect()
    {
        OnObjectDeselected?.Invoke(this, EventArgs.Empty);
        Debug.Log(gameObject.name + " Deselected");
    }
    public void TryInteract()
    {
        if (hasAlreadyBeenInteracted)
        {
            AlreadyInteracted();
            return;
        }

        if (IsInteractable) Interact();
        else FailInteract();
    }
    public void Interact()
    {
        Debug.Log(gameObject.name + " Interacted");
        OnObjectInteracted?.Invoke(this, EventArgs.Empty);

        hasAlreadyBeenInteracted = true;
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
    #endregion
}
