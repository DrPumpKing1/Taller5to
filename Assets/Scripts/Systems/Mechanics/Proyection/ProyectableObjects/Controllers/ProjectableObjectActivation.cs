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
    private const float NOT_POWERED_TIME_THRESHOLD = 0.5f;

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
    #endregion

    private void Update()
    {
        HandlePowered();
    }

    private void HandlePowered()
    {
        if (Power)
        {
            notPoweredTimer = 0f;

            canBeSelectedAlternate = true;
            isInteractableAlternate = true;
        }
        else
        {
            notPoweredTimer += Time.deltaTime;
        }

        if (notPoweredTimer >= NOT_POWERED_TIME_THRESHOLD)
        {
            canBeSelectedAlternate = false;
            isInteractableAlternate = false;
        }
    }

    private void ToggleActivation()
    {
        if (activableDevice.IsActive)
        {
            OnAnyObjectDeactivated?.Invoke(this, new OnAnyObjectActivatedEventArgs { projectableObjectSO = projectableObject.ProjectableObjectSO });
        }
        else
        {
            OnAnyObjectActivated?.Invoke(this, new OnAnyObjectActivatedEventArgs { projectableObjectSO = projectableObject.ProjectableObjectSO });
        }

        OnProjectableObjectActivated?.Invoke(this, EventArgs.Empty);
        OnUpdatedInteractableAlternateState?.Invoke(this, EventArgs.Empty);
    }
}
