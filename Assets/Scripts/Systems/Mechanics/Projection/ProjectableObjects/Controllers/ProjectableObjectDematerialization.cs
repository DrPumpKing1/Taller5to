using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class ProjectableObjectDematerialization : MonoBehaviour, IHoldInteractable
{
    [Header("Components")]
    [SerializeField] private ProjectableObject projectableObject;

    [Header("Interactable Settings")]
    [SerializeField, Range(1f,100f)] private float horizontalInteractionRange;
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
    [SerializeField] private Transform interactionAttentionTransform;
    [SerializeField] private Transform interactionPositionTransform;

    #region IHoldInteractable Properties
    public float HorizontalInteractionRange => horizontalInteractionRange;
    public float VerticalInteractionRange => verticalInteractionRange;
    public bool IsSelectable => canBeSelected;
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

    public event EventHandler OnObjectDematerialized;

    public static event EventHandler<OnAnyObjectDematerializedEventArgs> OnAnyObjectDematerialized;
    public static event EventHandler<OnAnyObjectDematerializedEventArgs> OnAnyObjectForceDematerialized;

    public static event EventHandler<OnAnyObjectDematerializedEventArgs> OnAnyStartDematerialization;
    public static event EventHandler<OnAnyObjectDematerializedEventArgs> OnAnyEndDematerialization;

    public event EventHandler<OnAnyObjectDematerializedEventArgs> OnStartDematerialization;
    public event EventHandler<OnAnyObjectDematerializedEventArgs> OnEndDematerialization;

    public class OnAnyObjectDematerializedEventArgs : EventArgs
    {
        public ProjectableObjectDematerialization projectableObjectDematerialization;
        public ProjectableObjectSO projectableObjectSO;
    }

    public void OnEnable()
    {
        ProjectionManager.OnAllObjectsDematerialized += ProjectionManager_OnAllObjectsDematerialized;
        ProjectionManager.OnAllObjectsForceDematerialized += ProjectionManager_OnAllObjectsForceDematerialized;

        projectableObject.OnProjectionPlatformSet += ProjectableObject_OnProjectionPlatformSet;
    }

    public void OnDisable()
    {
        ProjectionManager.OnAllObjectsDematerialized -= ProjectionManager_OnAllObjectsDematerialized;
        ProjectionManager.OnAllObjectsForceDematerialized -= ProjectionManager_OnAllObjectsForceDematerialized;

        projectableObject.OnProjectionPlatformSet -= ProjectableObject_OnProjectionPlatformSet;
        projectableObject.ProjectionPlatform.OnProjectionPlatformDestroyed -= ProjectionPlatform_OnProjectionPlatformDestroyed;
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
        OnObjectInteracted?.Invoke(this, EventArgs.Empty);

        DematerializeObject(true);
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

    public void HoldInteractionStart()
    {
        OnHoldInteractionStart?.Invoke(this, EventArgs.Empty);
        OnStartDematerialization?.Invoke(this, new OnAnyObjectDematerializedEventArgs {projectableObjectDematerialization = this, projectableObjectSO = projectableObject.ProjectableObjectSO });
        OnAnyStartDematerialization?.Invoke(this, new OnAnyObjectDematerializedEventArgs { projectableObjectDematerialization = this, projectableObjectSO = projectableObject.ProjectableObjectSO });
    }

    public void ContinousHoldInteraction(float holdTimer) => OnContinousHoldInteraction?.Invoke(this, new IHoldInteractable.OnHoldInteractionEventArgs { holdTimer = holdTimer, holdDuration = holdDuration });

    public void HoldInteractionEnd()
    {
        OnHoldInteractionEnd?.Invoke(this, EventArgs.Empty);
        OnEndDematerialization?.Invoke(this, new OnAnyObjectDematerializedEventArgs { projectableObjectDematerialization = this, projectableObjectSO = projectableObject.ProjectableObjectSO });
        OnAnyEndDematerialization?.Invoke(this, new OnAnyObjectDematerializedEventArgs { projectableObjectDematerialization = this, projectableObjectSO = projectableObject.ProjectableObjectSO });
    }

    public Transform GetTransform() => transform;
    public Transform GetInteractionAttentionTransform() => interactionAttentionTransform;
    public Transform GetInteractionPositionTransform() => interactionPositionTransform;

    #endregion

    private void DematerializeObject(bool triggerEvents)
    {
        if (projectableObject.ProjectionPlatform) projectableObject.ProjectionPlatform.ClearProjectionPlatform();

        ProjectionManager.Instance.ObjectDematerialized(projectableObject.ProjectableObjectSO, projectableObject.ProjectionPlatform, projectableObject, triggerEvents);
        OnObjectDematerialized?.Invoke(this, EventArgs.Empty);
        OnAnyObjectDematerialized?.Invoke(this, new OnAnyObjectDematerializedEventArgs { projectableObjectSO = projectableObject.ProjectableObjectSO });

        OnUpdatedInteractableState?.Invoke(this, EventArgs.Empty);

        Destroy(gameObject);
    }

    public void ForceDematerializeObject(bool triggerEvents)
    {
        if (projectableObject.ProjectionPlatform) projectableObject.ProjectionPlatform.ClearProjectionPlatform();

        OnUpdatedInteractableState?.Invoke(this, EventArgs.Empty);

        ProjectionManager.Instance.ObjectForceDematerialized(projectableObject.ProjectableObjectSO, projectableObject.ProjectionPlatform, projectableObject, triggerEvents);
        OnObjectDematerialized?.Invoke(this, EventArgs.Empty);
        OnAnyObjectForceDematerialized?.Invoke(this, new OnAnyObjectDematerializedEventArgs { projectableObjectSO = projectableObject.ProjectableObjectSO });

        Destroy(gameObject);
    }

    #region ProjectionManager Subscriptions
    private void ProjectionManager_OnAllObjectsDematerialized(object sender, ProjectionManager.OnAllObjectsDematerializedEventArgs e)
    {
        DematerializeObject(false);
    }

    private void ProjectionManager_OnAllObjectsForceDematerialized(object sender, ProjectionManager.OnAllObjectsDematerializedEventArgs e)
    {
        ForceDematerializeObject(false);
    }
    #endregion

    #region ProjectableObjectSubscriptions
    private void ProjectableObject_OnProjectionPlatformSet(object sender, ProjectableObject.OnProjectionPlatformSetEventArgs e)
    {
        projectableObject.ProjectionPlatform.OnProjectionPlatformDestroyed += ProjectionPlatform_OnProjectionPlatformDestroyed;
    }
    #endregion

    #region ProjectionPlatform Subscriptions
    private void ProjectionPlatform_OnProjectionPlatformDestroyed(object sender, EventArgs e)
    {
        ForceDematerializeObject(true);
    }
    #endregion

}
