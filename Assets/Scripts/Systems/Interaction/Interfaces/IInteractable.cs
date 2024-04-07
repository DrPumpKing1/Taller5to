using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IInteractable 
{
    public event EventHandler OnObjectInteracted;
    public event EventHandler OnObjectFailInteracted;
    public event EventHandler OnObjectHasAlreadyBeenInteracted;

    public bool IsSelectable { get; }
    public bool IsInteractable { get; }
    public bool HasAlreadyBeenInteracted { get; }
    public string TooltipMessage { get; }

    public void TryInteract();
    public void Interact();
    public void FailInteract();
    public void OnHasAlreadyBeenInteracted();
    public void OnSelection();
    public void OnDeselection();
    public Transform GetTransform();
}
