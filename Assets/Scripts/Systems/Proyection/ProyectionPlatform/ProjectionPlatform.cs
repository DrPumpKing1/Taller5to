using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionPlatform : MonoBehaviour, IHoldInteractable
{
    [Header("Interactable Settings")]
    [SerializeField] private bool canBeSelected;
    [SerializeField] private bool isInteractable;
    [SerializeField] private bool hasAlreadyBeenInteracted;
    [SerializeField] private string tooltipMessage;
    [Space]
    [SerializeField] private float holdDuration;

    public bool IsSelectable => canBeSelected;

    public bool IsInteractable => isInteractable;

    public bool HasAlreadyBeenInteracted => hasAlreadyBeenInteracted;

    public string TooltipMessage => tooltipMessage;

    public float HoldDuration => holdDuration;

    public event EventHandler OnObjectSelected;
    public event EventHandler OnObjectDeselected;

    public event EventHandler OnObjectInteracted;
    public event EventHandler OnObjectFailInteracted;
    public event EventHandler OnObjectHasAlreadyBeenInteracted;

    public void AlreadyInteracted()
    {
        OnObjectHasAlreadyBeenInteracted?.Invoke(this, EventArgs.Empty);
        Debug.Log("Platform was already interacted");
    }
    public void Select()
    {
        OnObjectSelected?.Invoke(this, EventArgs.Empty);
    }
    public void Deselect()
    {
        OnObjectDeselected?.Invoke(this, EventArgs.Empty);
    }

    public void TryInteract()
    {
        if (hasAlreadyBeenInteracted)
        {
            AlreadyInteracted();
            return;
        }

        if (!IsInteractable)
        {
            FailInteract();
            return;
        }

        Interact();
    }

    public void Interact()
    {
        OnObjectInteracted?.Invoke(this, EventArgs.Empty);
        Debug.Log("Interact");
    }

    public void FailInteract()
    {
        OnObjectFailInteracted?.Invoke(this, EventArgs.Empty);
        Debug.Log("FailInteract");
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

    public Transform GetTransform() => transform;
}
