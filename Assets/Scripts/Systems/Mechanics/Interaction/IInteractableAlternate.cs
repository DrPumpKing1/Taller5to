using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IInteractableAlternate
{
    public event EventHandler OnObjectSelectedAlternate;
    public event EventHandler OnObjectDeselectedAlternate;
    public event EventHandler OnObjectInteractedAlternate;
    public event EventHandler OnObjectFailInteractedAlternate;
    public event EventHandler OnObjectHasAlreadyBeenInteractedAlternate;
    public event EventHandler OnUpdatedInteractableAlternateState;

    public float HorizontalInteractionRange { get; }
    public float VerticalInteractionRange { get; }

    public bool IsSelectableAlternate { get; }
    public bool IsInteractableAlternate { get; }
    public bool HasAlreadyBeenInteractedAlternate { get; }
    public string TooltipMessageAlternate { get; }

    public bool GrabPetAttention { get; }
    public bool GrabPlayerAttention { get; }

    public void SelectAlternate();
    public void DeselectAlternate();
    public void TryInteractAlternate();
    public void InteractAlternate();
    public void FailInteractAlternate();
    public void AlreadyInteractedAlternate();
    public Transform GetTransform();
    public Transform GetInteractionAlternateAttentionTransform();
    public Transform GetInteractionAlternatePositionTransform();
}
