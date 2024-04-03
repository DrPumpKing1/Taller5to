using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IInteractable 
{
    public event EventHandler OnObjectInteracted;

    public event EventHandler OnObjectFailInteracted;

    public void TryInteract();
    public void Interact();
    public void FailInteract();
    public void OnSelection();
    public void OnDeselection();
    public Transform GetTransform();

    public bool IsSelectable { get; }
    public bool IsInteractable { get; }
    public string TooltipMessage { get; }
    public bool InfiniteUses { get; }
    public int UseTimes { get; }

}
