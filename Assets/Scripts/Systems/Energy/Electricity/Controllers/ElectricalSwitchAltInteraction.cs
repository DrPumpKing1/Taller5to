using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalAltSwitchInteraction : MonoBehaviour, IHoldInteractableAlternate
{
    [Header("Electrical Settings")]
    [SerializeField] private SwitchElectrode switchElectrode;

    [Header("Interactable Settings")]
    [SerializeField] private bool canBeSelected;
    [SerializeField] private bool isInteractable;
    [SerializeField] private bool hasAlreadyBeenInteracted;
    [Space]
    [SerializeField] private float holdDuration;

    #region IHoldInteractable Properties
    public bool IsSelectableAlternate => canBeSelected;
    public bool IsInteractableAlternate => isInteractable;
    public bool HasAlreadyBeenInteractedAlternate => hasAlreadyBeenInteracted;
    public string TooltipMessageAlternate => $"{(!switchElectrode.SwitchOn ? "Encender Switch" : "Apagar Switch")}";
    public float HoldDurationAlternate => holdDuration;
    #endregion

    #region IHoldInteractable Events
    public event EventHandler OnObjectSelectedAlternate;
    public event EventHandler OnObjectDeselectedAlternate;
    public event EventHandler OnObjectInteractedAlternate;
    public event EventHandler OnObjectFailInteractedAlternate;
    public event EventHandler OnObjectHasAlreadyBeenInteractedAlternate;

    public event EventHandler OnHoldInteractionAlternateStart;
    public event EventHandler OnHoldInteractionAlternateEnd;
    public event EventHandler<IHoldInteractableAlternate.OnHoldInteractionAlternateEventArgs> OnContinousHoldInteractionAlternate;
    #endregion

    #region IHoldInteractable Methods
    public void SelectAlternate()
    {
        OnObjectSelectedAlternate?.Invoke(this, EventArgs.Empty);
        Debug.Log("Electrical Switch Selected");
    }
    public void DeselectAlternate()
    {
        OnObjectDeselectedAlternate?.Invoke(this, EventArgs.Empty);
        Debug.Log("Electrical Switch Deselected");
    }
    public void TryInteractAlternate()
    {
        if (!IsInteractableAlternate)
        {
            FailInteractAlternate();
            return;
        }

        if (HasAlreadyBeenInteractedAlternate)
        {
            AlreadyInteractedAlternate();
            return;
        }

        InteractAlternate();
    }
    public void InteractAlternate()
    {
        SwitchComponent();

        Debug.Log("Electrical Switch Interacted");
        OnObjectInteractedAlternate?.Invoke(this, EventArgs.Empty);
    }
    public void FailInteractAlternate()
    {
        Debug.Log("Cant Interact with Electrical Switch");
        OnObjectFailInteractedAlternate?.Invoke(this, EventArgs.Empty);
    }
    public void AlreadyInteractedAlternate()
    {
        Debug.Log("Electrical Switch has Already Been Interacted");
        OnObjectHasAlreadyBeenInteractedAlternate?.Invoke(this, EventArgs.Empty);
    }
    public bool CheckSuccessAlternate()
    {
        if (!isInteractable)
        {
            FailInteractAlternate();
            return false;
        }

        return true;
    }
    public void HoldInteractionAlternateStart() => OnHoldInteractionAlternateStart?.Invoke(this, EventArgs.Empty);
    public void ContinousHoldInteractionAlternate(float holdTimer) => OnContinousHoldInteractionAlternate?.Invoke(this, new IHoldInteractableAlternate.OnHoldInteractionAlternateEventArgs { holdTimer = holdTimer, holdDuration = holdDuration });
    public void HoldInteractionAlternateEnd() => OnHoldInteractionAlternateEnd?.Invoke(this, EventArgs.Empty);

    public Transform GetTransform() => transform;
    #endregion

    private void SwitchComponent()
    {
        switchElectrode.SetSwitch(!switchElectrode.SwitchOn);
    }
}
