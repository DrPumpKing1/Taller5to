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
    [SerializeField] private bool isInteractable;
    [SerializeField] private bool hasAlreadyBeenInteracted;
    [SerializeField] private string tooltipMessage;
    [Space]
    [SerializeField] private float holdDuration;
    [Space]
    [SerializeField] private bool grabPetAttention;
    [SerializeField] private bool grabPlayerAttention;

    #region IHoldInteractable Properties
    public float HorizontalInteractionRange => horizontalInteractionRange;
    public float VerticalInteractionRange => verticalInteractionRange;
    public bool IsSelectable => canBeSelected && !projectionPlatform.ObjectAbove && ProjectableObjectSelectionManager.Instance.SelectedProjectableObjectSO != null;
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

    public event EventHandler<OnProjectionEventArgs> OnObjectProjectionSuccess;
    public event EventHandler<OnProjectionEventArgs> OnObjectProjectionFailed;
    public event EventHandler<OnProjectionEventArgs> OnObjectProjectionFailedInsuficientGems;

    public static event EventHandler<OnAnyProjectionEventArgs> OnAnyObjectProjectionSuccess;
    public static event EventHandler<OnAnyProjectionEventArgs> OnAnyObjectProjectionFailed;
    public static event EventHandler<OnAnyProjectionEventArgs> OnAnyObjectProjectionFailedInsuficientGems;

    public static event EventHandler OnStartProjection;
    public static event EventHandler OnEndProjection;

    public class OnProjectionEventArgs : EventArgs
    {
        public ProjectableObjectSO projectableObjectSO;
    }

    public class OnAnyProjectionEventArgs : EventArgs
    {
        public ProjectableObjectSO projectableObjectSO;
    }

    private void OnEnable()
    {
        projectionPlatform.OnProjectionPlatformClear += ProjectionPlatform_OnProjectionPlatformClear;
    }

    private void OnDisable()
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

        if (!IsInteractable)
        {
            FailInteract();
            return;
        }

        if (projectionPlatform.CurrentProjectedObjectSO != null)
        {
            FailObjectProjection(ProjectableObjectSelectionManager.Instance.SelectedProjectableObjectSO);
            return;
        }

        if (!ProjectionManager.Instance.CanProjectObject(ProjectableObjectSelectionManager.Instance.SelectedProjectableObjectSO))
        {
            FailObjectProjectionInsuficientGems(ProjectableObjectSelectionManager.Instance.SelectedProjectableObjectSO);
            return;
        }

        Interact();
    }

    public void Interact()
    {
        ProjectObject(ProjectableObjectSelectionManager.Instance.SelectedProjectableObjectSO);

        OnObjectInteracted?.Invoke(this, EventArgs.Empty);

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

        if (projectionPlatform.CurrentProjectedObjectSO != null && !projectionPlatform.ObjectAbove)
        {
            FailObjectProjection(ProjectableObjectSelectionManager.Instance.SelectedProjectableObjectSO);
            return false;
        }

        if (!ProjectionManager.Instance.CanProjectObject(ProjectableObjectSelectionManager.Instance.SelectedProjectableObjectSO))
        {
            FailObjectProjectionInsuficientGems(ProjectableObjectSelectionManager.Instance.SelectedProjectableObjectSO);
            return false;
        }

        return true;
    }

    public void HoldInteractionStart()
    {
        OnHoldInteractionStart?.Invoke(this, EventArgs.Empty);
        OnStartProjection?.Invoke(this, EventArgs.Empty);
    }
    public void ContinousHoldInteraction(float holdTimer) => OnContinousHoldInteraction?.Invoke(this, new IHoldInteractable.OnHoldInteractionEventArgs { holdTimer = holdTimer, holdDuration = holdDuration });
    public void HoldInteractionEnd()
    {
        OnHoldInteractionEnd?.Invoke(this, EventArgs.Empty);
        OnEndProjection?.Invoke(this, EventArgs.Empty);
    }

    public Transform GetTransform() => transform;
    #endregion

    private void FailObjectProjection(ProjectableObjectSO projectableObjectSO)
    {
        ProjectionManager.Instance.FailObjectProjection(projectableObjectSO, projectionPlatform);
        OnObjectProjectionFailed?.Invoke(this, new OnProjectionEventArgs { projectableObjectSO = projectableObjectSO });
        OnAnyObjectProjectionFailed?.Invoke(this, new OnAnyProjectionEventArgs { projectableObjectSO = projectableObjectSO });
        Debug.Log("Cant Project Object");
    }

    private void FailObjectProjectionInsuficientGems(ProjectableObjectSO projectableObjectSO)
    {
        ProjectionManager.Instance.FailObjectProjectionInsuficientGems(projectableObjectSO, projectionPlatform);
        OnObjectProjectionFailedInsuficientGems?.Invoke(this, new OnProjectionEventArgs { projectableObjectSO = projectableObjectSO });
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
        OnObjectProjectionSuccess?.Invoke(this, new OnProjectionEventArgs { projectableObjectSO = projectableObjectSO });
        OnAnyObjectProjectionSuccess?.Invoke(this, new OnAnyProjectionEventArgs { projectableObjectSO = projectableObjectSO });

        projectionPlatform.SetProjectionPlatform(projectableObjectSO, projectableObject);
        OnUpdatedInteractableState?.Invoke(this, EventArgs.Empty);
    }

    private void ProjectionPlatform_OnProjectionPlatformClear(object sender, EventArgs e)
    {
        canBeSelected = true;
    }
}
