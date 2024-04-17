using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionPlatform : MonoBehaviour, IHoldInteractable
{
    [Header("Projection Platform Settings")]
    [SerializeField] private Transform projectionPoint;
    [SerializeField] private ProjectableObjectSO currentProjectedObject;

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

    public event EventHandler OnHoldInteractionStart;
    public event EventHandler OnHoldInteractionEnd;
    public event EventHandler<IHoldInteractable.OnHoldInteractionEventArgs> OnContinousHoldInteraction;
    #endregion

    public event EventHandler<OnProjectionEventArgs> OnObjectProjectionSuccess;
    public event EventHandler<OnProjectionEventArgs> OnObjectProjectionFailed;
    public event EventHandler<OnProjectionEventArgs> OnObjectProjectionFailedInsuficientGems;

    public class OnProjectionEventArgs : EventArgs
    {
        public ProjectableObjectSO projectableObjectSO;
    }

    #region IHoldInteractable Methods
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

        if(currentProjectedObject != null)
        {
            FailObjectProjection(ProjectionManager.Instance.SelectedProjectableObjectSO);
            return;
        }

        if (!ProjectionManager.Instance.CanProjectObject(ProjectionManager.Instance.SelectedProjectableObjectSO))
        {
            FailObjectProjectionInsuficientGems(ProjectionManager.Instance.SelectedProjectableObjectSO);
            return;
        }

        Interact();
    }

    public void Interact()
    {
        ProjectObject(ProjectionManager.Instance.SelectedProjectableObjectSO);

        OnObjectInteracted?.Invoke(this, EventArgs.Empty);
        Debug.Log("Interact");

        canBeSelected = false; //Can´t be selected until is Platform is Reseted (clearing the projected object resets it)
    }

    public void FailInteract()
    {
        OnObjectFailInteracted?.Invoke(this, EventArgs.Empty);
        Debug.Log("FailInteract");
    }
    public void AlreadyInteracted()
    {
        OnObjectHasAlreadyBeenInteracted?.Invoke(this, EventArgs.Empty);
        Debug.Log("Platform was already interacted");
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

        if (currentProjectedObject != null)
        {
            FailObjectProjection(ProjectionManager.Instance.SelectedProjectableObjectSO);
            return false;
        }

        if (!ProjectionManager.Instance.CanProjectObject(ProjectionManager.Instance.SelectedProjectableObjectSO))
        {
            FailObjectProjectionInsuficientGems(ProjectionManager.Instance.SelectedProjectableObjectSO);
            return false;
        }

        return true;
    }

    public void HoldInteractionStart() => OnHoldInteractionStart?.Invoke(this, EventArgs.Empty);
    public void ContinousHoldInteraction(float holdTimer) => OnContinousHoldInteraction?.Invoke(this, new IHoldInteractable.OnHoldInteractionEventArgs { holdTimer = holdTimer, holdDuration = holdDuration });
    public void HoldInteractionEnd() => OnHoldInteractionEnd?.Invoke(this, EventArgs.Empty);

    public Transform GetTransform() => transform;
    #endregion

    private void FailObjectProjection(ProjectableObjectSO projectableObjectSO)
    {
        ProjectionManager.Instance.FailObjectProjection(projectableObjectSO, this);
        OnObjectProjectionFailed?.Invoke(this, new OnProjectionEventArgs { projectableObjectSO = projectableObjectSO });
        Debug.Log("Cant Project Object");
    }


    private void FailObjectProjectionInsuficientGems(ProjectableObjectSO projectableObjectSO)
    {
        ProjectionManager.Instance.FailObjectProjectionInsuficientGems(projectableObjectSO, this);
        OnObjectProjectionFailedInsuficientGems?.Invoke(this, new OnProjectionEventArgs { projectableObjectSO = projectableObjectSO } );
        Debug.Log("Insuficient Projection Gems");
    }

    private void ProjectObject(ProjectableObjectSO projectableObjectSO)
    {
        currentProjectedObject = projectableObjectSO;

        GameObject projectedObject = Instantiate(projectableObjectSO.prefab.gameObject, projectionPoint.position, projectionPoint.rotation);
        projectedObject.transform.SetParent(projectionPoint);
        projectedObject.GetComponent<ProjectableObject>().SetProjectionPlatform(this);

        ProjectionManager.Instance.SuccessObjectProjection(projectableObjectSO, this);
        OnObjectProjectionSuccess?.Invoke(this, new OnProjectionEventArgs { projectableObjectSO = projectableObjectSO } );

        Debug.Log("Object Projected");
    }

    public void ResetProjectionPlatform()
    {
        ClearCurrentProjectedObject();
        canBeSelected = true;
    }

    private void ClearCurrentProjectedObject() => currentProjectedObject = null;
}
