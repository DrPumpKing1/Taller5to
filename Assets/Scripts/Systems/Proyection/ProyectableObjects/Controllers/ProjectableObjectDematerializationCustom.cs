using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectableObjectDematerializationCustom : MonoBehaviour, IHoldInteractable
{
    [Header("Components")]
    [SerializeField] private ProjectableObject projectableObject;
    [SerializeField] private GameObject customObject;

    [Header("Interactable Settings")]
    [SerializeField] private bool canBeSelected;
    [SerializeField] private bool isInteractable;
    [SerializeField] private bool hasAlreadyBeenInteracted;
    [SerializeField] private string tooltipMessage;
    [Space]
    [SerializeField] private float holdDuration;

    #region IHoldInteractable Properties
    public bool IsSelectable => canBeSelected;
    public bool IsInteractable => isInteractable;
    public bool HasAlreadyBeenInteracted => hasAlreadyBeenInteracted;
    public string TooltipMessage => tooltipMessage;
    public float HoldDuration => holdDuration;
    #endregion

    #region IHoldInteractable Events
    public event EventHandler OnObjectSelected;
    public event EventHandler OnObjectDeselected;
    public event EventHandler OnObjectInteracted;
    public event EventHandler OnObjectFailInteracted;
    public event EventHandler OnObjectHasAlreadyBeenInteracted;
    public event EventHandler OnUpdatedInteractableState;

    public event EventHandler OnHoldInteractionStart;
    public event EventHandler OnHoldInteractionEnd;
    public event EventHandler<IHoldInteractable.OnHoldInteractionEventArgs> OnContinousHoldInteraction;
    #endregion

    public event EventHandler OnObjectDematerialized;

    #region IHoldInteractable Methods
    public void Select()
    {
        OnObjectSelected?.Invoke(this, EventArgs.Empty);
        Debug.Log("ProjectableObject Selected");
    }
    public void Deselect()
    {
        OnObjectDeselected?.Invoke(this, EventArgs.Empty);
        Debug.Log("ProjectableObject Deselected");
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
        Debug.Log("ProjectableObject Interacted");
        OnObjectInteracted?.Invoke(this, EventArgs.Empty);

        DematerializeObject();
    }

    public void FailInteract()
    {
        Debug.Log("Cant Interact with ProjectableObject");
        OnObjectFailInteracted?.Invoke(this, EventArgs.Empty);
    }
    public void AlreadyInteracted()
    {
        Debug.Log("ProjectableObject has Already Been Interacted");
        OnObjectHasAlreadyBeenInteracted?.Invoke(this, EventArgs.Empty);
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
    public void HoldInteractionStart() => OnHoldInteractionStart?.Invoke(this, EventArgs.Empty);
    public void ContinousHoldInteraction(float holdTimer) => OnContinousHoldInteraction?.Invoke(this, new IHoldInteractable.OnHoldInteractionEventArgs { holdTimer = holdTimer, holdDuration = holdDuration });
    public void HoldInteractionEnd() => OnHoldInteractionEnd?.Invoke(this, EventArgs.Empty);


    public Transform GetTransform() => transform;

    #endregion

    private void DematerializeObject()
    {
        ProjectionManager.Instance.ObjectDematerialized(projectableObject.ProjectableObjectSO, projectableObject.ProjectionPlatform);
        OnObjectDematerialized?.Invoke(this, EventArgs.Empty);

        if (projectableObject.ProjectionPlatform) projectableObject.ProjectionPlatform.ClearProjectionPlatform();

        Destroy(customObject);
    }
}
