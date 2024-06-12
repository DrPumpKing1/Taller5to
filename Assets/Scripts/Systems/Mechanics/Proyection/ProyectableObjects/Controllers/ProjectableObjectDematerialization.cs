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

    private bool triggerDematerializationEvents = false;

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

    public class OnAnyObjectDematerializedEventArgs : EventArgs
    {
        public ProjectableObjectSO projectableObjectSO;
    }

    public void OnEnable()
    {
        ProjectionManager.OnAllObjectsDematerialized += ProjectionManager_OnAllObjectsDematerialized;
    }
    public void OnDisable()
    {
        ProjectionManager.OnAllObjectsDematerialized -= ProjectionManager_OnAllObjectsDematerialized;
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
    public void HoldInteractionStart() => OnHoldInteractionStart?.Invoke(this, EventArgs.Empty);
    public void ContinousHoldInteraction(float holdTimer) => OnContinousHoldInteraction?.Invoke(this, new IHoldInteractable.OnHoldInteractionEventArgs { holdTimer = holdTimer, holdDuration = holdDuration });
    public void HoldInteractionEnd() => OnHoldInteractionEnd?.Invoke(this, EventArgs.Empty);
    public Transform GetTransform() => transform;

    #endregion

    private void DematerializeObject(bool triggerEvents)
    {
        if (projectableObject.ProjectionPlatform) projectableObject.ProjectionPlatform.ClearProjectionPlatform();

        OnUpdatedInteractableState?.Invoke(this, EventArgs.Empty);

        triggerDematerializationEvents = triggerEvents;

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        ProjectionManager.Instance.ObjectDematerialized(projectableObject.ProjectableObjectSO, projectableObject.ProjectionPlatform, projectableObject, triggerDematerializationEvents);
        OnObjectDematerialized?.Invoke(this, EventArgs.Empty);
        OnAnyObjectDematerialized?.Invoke(this, new OnAnyObjectDematerializedEventArgs { projectableObjectSO = projectableObject.ProjectableObjectSO });
    }

    #region ProjectionManager Subscriptions

    private void ProjectionManager_OnAllObjectsDematerialized(object sender, ProjectionManager.OnAllObjectsDematerializedEventArgs e)
    {
        DematerializeObject(false);
    }
    #endregion
}
