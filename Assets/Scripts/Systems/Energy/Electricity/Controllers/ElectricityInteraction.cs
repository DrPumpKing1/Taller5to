using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityInteraction : MonoBehaviour, IHoldInteractable
{
    [Header("Electrical Settings")]
    public ElectricityComponent component;

    [Header("Interactable Settings")]
    [SerializeField] private bool canBeSelected;
    [SerializeField] private bool isInteractable;
    [SerializeField] private bool hasAlreadyBeenInteracted;
    [Space]
    [SerializeField] private float holdDuration;

    public bool IsSelectable => canBeSelected;
    public bool IsInteractable => isInteractable;
    public bool HasAlreadyBeenInteracted => hasAlreadyBeenInteracted;
    public string TooltipMessage => $"Switch component {(!component.On ? "on" : "off")}";
    public float HoldDuration => holdDuration;

    public event EventHandler OnObjectSelected;
    public event EventHandler OnObjectDeselected;
    public event EventHandler OnObjectInteracted;
    public event EventHandler OnObjectFailInteracted;
    public event EventHandler OnObjectHasAlreadyBeenInteracted;

    #region IInteractable
    public void Select()
    {
        OnObjectSelected?.Invoke(this, EventArgs.Empty);
        Debug.Log("Electrical Switch Selected");
    }
    public void Deselect()
    {
        OnObjectDeselected?.Invoke(this, EventArgs.Empty);
        Debug.Log("Electrical Switch Deselected");
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
        SwitchComponent();

        Debug.Log("Electrical Switch Interacted");
        OnObjectInteracted?.Invoke(this, EventArgs.Empty);
    }
    public void FailInteract()
    {
        Debug.Log("Cant Interact with Electrical Switch");
        OnObjectFailInteracted?.Invoke(this, EventArgs.Empty);
    }
    public void AlreadyInteracted()
    {
        Debug.Log("Electrical Switch has Already Been Interacted");
        OnObjectHasAlreadyBeenInteracted?.Invoke(this, EventArgs.Empty);
    }
    public bool CheckSuccess()
    {
        if (!isInteractable)
        {
            FailInteract();
            return false;
        }

        return true;
    }
    public Transform GetTransform() => transform;
    #endregion

    private void SwitchComponent()
    {
        component.Switch(!component.On);
    }
}
