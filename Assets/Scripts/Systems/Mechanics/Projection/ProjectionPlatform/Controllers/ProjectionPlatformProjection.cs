using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectionPlatformProjection : MonoBehaviour, IHoldInteractable
{
    [Header("Components")]
    [SerializeField] private ProjectionPlatform projectionPlatform;

    [Header("Interactable Settings")]
    [SerializeField, Range(1f, 100f)] private float horizontalInteractionRange;
    [SerializeField, Range(1f, 100f)] private float verticalInteractionRange;
    [Space]
    [SerializeField] private bool canBeSelected;
    [SerializeField] protected bool isInteractable;
    [SerializeField] private bool hasAlreadyBeenInteracted;
    [SerializeField] private string tooltipMessage;
    [Space]
    [SerializeField] private float holdDuration;
    [Space]
    [SerializeField] private bool grabPetAttention;
    [SerializeField] private bool grabPlayerAttention;
    [SerializeField] private Transform interactionAttentionTransform;
    [SerializeField] private Transform interactionPositionTransform;

    #region IHoldInteractable Properties
    public float HorizontalInteractionRange => horizontalInteractionRange;
    public float VerticalInteractionRange => verticalInteractionRange;
    public bool IsSelectable => canBeSelected && !projectionPlatform.ObjectAbove && ProjectableObjectSelectionManager.Instance.ProjectableObjectsIndexed.Count>0;
    public bool IsInteractable => isInteractable;
    public bool HasAlreadyBeenInteracted => hasAlreadyBeenInteracted;
    public string TooltipMessage => tooltipMessage;
    public float HoldDuration => holdDuration;
    public bool GrabPetAttention => grabPetAttention;
    public bool GrabPlayerAttention => grabPlayerAttention;
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

    public event EventHandler<OnObjectProjectionEventArgs> OnObjectProjectionSuccess;
    public event EventHandler<OnObjectProjectionEventArgs> OnObjectProjectionFailed;
    public event EventHandler<OnObjectProjectionEventArgs> OnObjectProjectionFailedInsuficientGems;

    public static event EventHandler<OnAnyProjectionEventArgs> OnAnyObjectProjectionSuccess;
    public static event EventHandler<OnAnyProjectionEventArgs> OnAnyObjectProjectionFailed;
    public static event EventHandler<OnAnyProjectionEventArgs> OnAnyObjectProjectionFailedInsuficientGems;

    public static event EventHandler<OnProjectionEventArgs> OnStartProjection;
    public static event EventHandler<OnProjectionEventArgs> OnEndProjection;

    public class OnObjectProjectionEventArgs : EventArgs
    {
        public ProjectableObjectSO projectableObjectSO;
    }

    public class OnAnyProjectionEventArgs : EventArgs
    {
        public ProjectableObjectSO projectableObjectSO;
    }

    public class OnProjectionEventArgs : EventArgs
    {
        public Transform interactionAttentionTransform;
        public float holdDuration;
    }

    protected virtual void OnEnable()
    {
        projectionPlatform.OnProjectionPlatformClear += ProjectionPlatform_OnProjectionPlatformClear;
    }

    protected virtual void OnDisable()
    {
        projectionPlatform.OnProjectionPlatformClear -= ProjectionPlatform_OnProjectionPlatformClear;
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

        if (!isInteractable)
        {
            FailInteract();
            return;
        }

        if (projectionPlatform.CurrentProjectedObjectSO != null)
        {
            FailObjectProjection(ProjectableObjectSelectionManager.Instance.SelectedProjectableObjectIndexed.projectableObjectSO);
            return;
        }

        if (!ProjectionManager.Instance.CanProjectObject(ProjectableObjectSelectionManager.Instance.SelectedProjectableObjectIndexed.projectableObjectSO))
        {
            FailObjectProjectionInsuficientGems(ProjectableObjectSelectionManager.Instance.SelectedProjectableObjectIndexed.projectableObjectSO);
            return;
        }

        Interact();
    }

    public void Interact()
    {
        ProjectObject(ProjectableObjectSelectionManager.Instance.SelectedProjectableObjectIndexed.projectableObjectSO);

        OnObjectInteracted?.Invoke(this, EventArgs.Empty);

        canBeSelected = false; //Can´t be selected until is Platform is Reseted (clearing the projected object resets it)
    }

    public virtual void FailInteract()
    {
        OnObjectFailInteracted?.Invoke(this, EventArgs.Empty);
        //Debug.Log("FailInteract");
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

        if (projectionPlatform.CurrentProjectedObjectSO != null && !projectionPlatform.ObjectAbove)
        {
            FailObjectProjection(ProjectableObjectSelectionManager.Instance.SelectedProjectableObjectIndexed.projectableObjectSO);
            return false;
        }

        if (!ProjectionManager.Instance.CanProjectObject(ProjectableObjectSelectionManager.Instance.SelectedProjectableObjectIndexed.projectableObjectSO))
        {
            FailObjectProjectionInsuficientGems(ProjectableObjectSelectionManager.Instance.SelectedProjectableObjectIndexed.projectableObjectSO);
            return false;
        }

        return true;
    }

    public void HoldInteractionStart()
    {
        OnHoldInteractionStart?.Invoke(this, EventArgs.Empty);
        OnStartProjection?.Invoke(this, new OnProjectionEventArgs { interactionAttentionTransform = interactionAttentionTransform, holdDuration = holdDuration });
    }
    public void ContinousHoldInteraction(float holdTimer) => OnContinousHoldInteraction?.Invoke(this, new IHoldInteractable.OnHoldInteractionEventArgs { holdTimer = holdTimer, holdDuration = holdDuration });
    public void HoldInteractionEnd()
    {
        OnHoldInteractionEnd?.Invoke(this, EventArgs.Empty);
        OnEndProjection?.Invoke(this, new OnProjectionEventArgs { interactionAttentionTransform = interactionAttentionTransform, holdDuration = holdDuration });
    }

    public Transform GetTransform() => transform;
    public Transform GetInteractionAttentionTransform() => interactionAttentionTransform;
    public Transform GetInteractionPositionTransform() => interactionPositionTransform;
    #endregion

    private void FailObjectProjection(ProjectableObjectSO projectableObjectSO)
    {
        ProjectionManager.Instance.FailObjectProjection(projectableObjectSO, projectionPlatform);
        OnObjectProjectionFailed?.Invoke(this, new OnObjectProjectionEventArgs { projectableObjectSO = projectableObjectSO });
        OnAnyObjectProjectionFailed?.Invoke(this, new OnAnyProjectionEventArgs { projectableObjectSO = projectableObjectSO });
        Debug.Log("Cant Project Object");
    }

    private void FailObjectProjectionInsuficientGems(ProjectableObjectSO projectableObjectSO)
    {
        ProjectionManager.Instance.FailObjectProjectionInsuficientGems(projectableObjectSO, projectionPlatform);
        OnObjectProjectionFailedInsuficientGems?.Invoke(this, new OnObjectProjectionEventArgs { projectableObjectSO = projectableObjectSO });
        OnAnyObjectProjectionFailedInsuficientGems?.Invoke(this, new OnAnyProjectionEventArgs { projectableObjectSO = projectableObjectSO });
        Debug.Log("Insuficient Projection Gems");
    }

    private void ProjectObject(ProjectableObjectSO projectableObjectSO)
    {
        GameObject projectedObject = Instantiate(projectableObjectSO.prefab.gameObject, projectionPlatform.ProjectionPoint.position, projectionPlatform.ProjectionPoint.rotation);
        ProjectableObject projectableObject = projectedObject.GetComponent<ProjectableObject>();

        projectedObject.transform.SetParent(projectionPlatform.ProjectionPoint);
        projectableObject.SetProjectionPlatform(projectionPlatform);

        ProjectionManager.Instance.SuccessObjectProjection(projectableObjectSO, projectionPlatform, projectableObject);
        OnObjectProjectionSuccess?.Invoke(this, new OnObjectProjectionEventArgs { projectableObjectSO = projectableObjectSO });
        OnAnyObjectProjectionSuccess?.Invoke(this, new OnAnyProjectionEventArgs { projectableObjectSO = projectableObjectSO });

        projectionPlatform.SetProjectionPlatform(projectableObjectSO, projectableObject);
        OnUpdatedInteractableState?.Invoke(this, EventArgs.Empty);
    }

    private void ProjectionPlatform_OnProjectionPlatformClear(object sender, EventArgs e)
    {
        canBeSelected = true;
    }
}
