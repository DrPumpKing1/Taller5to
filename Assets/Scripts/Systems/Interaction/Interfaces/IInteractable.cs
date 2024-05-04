using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IInteractable 
{
    public event EventHandler OnObjectSelected;
    public event EventHandler OnObjectDeselected;

    public event EventHandler OnObjectInteracted;
    public event EventHandler OnObjectFailInteracted;
    public event EventHandler OnObjectHasAlreadyBeenInteracted;
    public event EventHandler OnUpdatedInteractableState;

    public float HorizontalInteractionRange { get; }
    public float VerticalInteractionRange { get; }

    public bool IsSelectable { get; }
    public bool IsInteractable { get; }
    public bool HasAlreadyBeenInteracted { get; }
    public string TooltipMessage { get; }

    public bool GrabPetAttention { get; }
    public bool GrabPlayerAttention { get; }

    public void Select();
    public void Deselect();
    public void TryInteract();
    public void Interact();
    public void FailInteract();
    public void AlreadyInteracted();
    public Transform GetTransform();
}
