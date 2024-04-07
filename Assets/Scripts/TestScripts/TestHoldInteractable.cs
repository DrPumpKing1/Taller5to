using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestHoldInteractable : MonoBehaviour, IHoldInteractable
{
    [Header("Interactable Settings")]
    [SerializeField] private bool canBeSelected;
    [SerializeField] private bool isInteractable;
    [SerializeField] private bool hasAlreadyBeenInteracted;

    [Space]
    [SerializeField] private bool holdInteract;
    [SerializeField] private float holdDuration;

    [Space]
    [SerializeField] private string tooltipMessage;

    public bool IsSelectable => canBeSelected;
    public bool IsInteractable => isInteractable;
    public bool HasAlreadyBeenInteracted => hasAlreadyBeenInteracted;
    public string TooltipMessage => tooltipMessage;
    public float HoldDuration => holdDuration;

    public event EventHandler OnObjectInteracted;
    public event EventHandler OnObjectFailInteracted;
    public event EventHandler OnObjectHasAlreadyBeenInteracted;

    #region IInteractable
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

    public void OnHasAlreadyBeenInteracted()
    {
        Debug.Log(gameObject.name + " Has Already Been Interacted");
        OnObjectHasAlreadyBeenInteracted?.Invoke(this, EventArgs.Empty);
    }

    public void OnDeselection() => Debug.Log(gameObject.name + " Deselected");

    public void OnSelection() => Debug.Log(gameObject.name + " Selected");

    public void TryInteract()
    {
        if (hasAlreadyBeenInteracted)
        {
            OnHasAlreadyBeenInteracted();
            return;
        }

        if (IsInteractable) Interact();
        else FailInteract();
    }
    public Transform GetTransform() => transform;
    #endregion

}
