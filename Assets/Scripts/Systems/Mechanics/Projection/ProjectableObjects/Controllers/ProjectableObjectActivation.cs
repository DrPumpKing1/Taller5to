using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectableObjectActivation : MonoBehaviour, IInteractableAlternate
{
    [Header("Components")]
    [SerializeField] private ProjectableObject projectableObject;

    [Header("Electrical Settings")]
    [SerializeField] private ActivableDevice activableDevice;

    [Header("Interactable Settings")]
    [SerializeField, Range(1f, 100f)] private float horizontalInteractionRange;
    [SerializeField, Range(1f, 100f)] private float verticalInteractionRange;
    [Space]
    [SerializeField] private bool canBeSelectedAlternate;
    [SerializeField] private bool isInteractableAlternate;
    [SerializeField] private bool hasAlreadyBeenInteractedAlternate;
    [SerializeField] private string tooltipMessageInactive;
    [SerializeField] private string tooltipMessageActive;
    [Space]
    [SerializeField] private bool grabPetAttention;
    [SerializeField] private bool grabPlayerAttention;
    [SerializeField] private Transform interactionAttentionTransform;
    [SerializeField] private Transform interactionPositionTransform;

    #region IHoldInteractable Properties
    public float HorizontalInteractionRange => horizontalInteractionRange;
    public float VerticalInteractionRange => verticalInteractionRange;
    public bool IsSelectableAlternate => canBeSelectedAlternate;
    public bool IsInteractableAlternate => isInteractableAlternate;
    public bool HasAlreadyBeenInteractedAlternate => hasAlreadyBeenInteractedAlternate;
    public string TooltipMessageAlternate => $"{(!activableDevice.IsActive ? tooltipMessageInactive : tooltipMessageActive)}";
    public bool GrabPetAttention => grabPetAttention;
    public bool GrabPlayerAttention => grabPlayerAttention;
    #endregion

    private bool Power => activableDevice.Power;
    private float notPoweredTimer = 0f;
    private const float NOT_POWERED_TIME_THRESHOLD = 0.25f;

    private bool previousPowered;

    #region IInteractable Events
    public event EventHandler OnObjectSelectedAlternate;
    public event EventHandler OnObjectDeselectedAlternate;
    public event EventHandler OnObjectInteractedAlternate;
    public event EventHandler OnObjectFailInteractedAlternate;
    public event EventHandler OnObjectHasAlreadyBeenInteractedAlternate;
    public event EventHandler OnUpdatedInteractableAlternateState;
    #endregion

    public static event EventHandler<OnAnyObjectActivatedEventArgs> OnAnyObjectActivated;
    public static event EventHandler<OnAnyObjectActivatedEventArgs> OnAnyObjectDeactivated;
    public event EventHandler OnProjectableObjectActivated;

    public class OnAnyObjectActivatedEventArgs : EventArgs
    {
        public ProjectableObjectSO projectableObjectSO;
        public int projectionPlatformID;
    }

    #region IInteractable Methods
    public void SelectAlternate()
    {
        OnObjectSelectedAlternate?.Invoke(this, EventArgs.Empty);
    }
    public void DeselectAlternate()
    {
        OnObjectDeselectedAlternate?.Invoke(this, EventArgs.Empty);
    }
    public void TryInteractAlternate()
    {
        if (!isInteractableAlternate)
        {
            FailInteractAlternate();
            return;
        }

        if (hasAlreadyBeenInteractedAlternate)
        {
            AlreadyInteractedAlternate();
            return;
        }

        InteractAlternate();
    }
    public void InteractAlternate()
    {
        ToggleActivation();

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

    public Transform GetTransform() => transform;
    public Transform GetInteractionAlternateAttentionTransform() => interactionAttentionTransform;
    public Transform GetInteractionAlternatePositionTransform() => interactionPositionTransform;
    #endregion

    private void Start()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        previousPowered = Power;
    }

    private void Update()
    {
        HandlePowered();
        HandleActivation();
    }

    private void HandlePowered()
    {
        if (Power)
        {
            notPoweredTimer = 0f;

            if(!previousPowered && activableDevice.IsActive)
            {
                OnAnyObjectActivated?.Invoke(this, new OnAnyObjectActivatedEventArgs { projectableObjectSO = projectableObject.ProjectableObjectSO, projectionPlatformID = projectableObject.ProjectionPlatform.ID });
            }

            previousPowered = true;
        }
        else
        {
            notPoweredTimer += Time.deltaTime;
        }

        if (notPoweredTimer >= NOT_POWERED_TIME_THRESHOLD)
        {
            if (previousPowered && activableDevice.IsActive)
            {
                OnAnyObjectDeactivated?.Invoke(this, new OnAnyObjectActivatedEventArgs { projectableObjectSO = projectableObject.ProjectableObjectSO, projectionPlatformID = projectableObject.ProjectionPlatform.ID });
            }

            previousPowered = false;
        }
    }

    private void HandleActivation()
    {
        if (activableDevice.canBeActivated && Power)
        {
            canBeSelectedAlternate = true;
            isInteractableAlternate = true;
        }
        else
        {
            canBeSelectedAlternate = false;
            isInteractableAlternate = false;
        }
    }

    private void ToggleActivation()
    {
        if (activableDevice.IsActive)
        {
            OnAnyObjectDeactivated?.Invoke(this, new OnAnyObjectActivatedEventArgs { projectableObjectSO = projectableObject.ProjectableObjectSO , projectionPlatformID = projectableObject.ProjectionPlatform.ID});
        }
        else
        {
            OnAnyObjectActivated?.Invoke(this, new OnAnyObjectActivatedEventArgs { projectableObjectSO = projectableObject.ProjectableObjectSO, projectionPlatformID = projectableObject.ProjectionPlatform.ID });
        }

        OnProjectableObjectActivated?.Invoke(this, EventArgs.Empty);
        OnUpdatedInteractableAlternateState?.Invoke(this, EventArgs.Empty);
    }
}
