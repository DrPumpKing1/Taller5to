using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InteractionInput interactionInput;
    [SerializeField] private PlayerHorizontalMovement playerHorizontalMovement;
    [SerializeField] private PlayerRotationHandler playerRotationHandler;
    [SerializeField] private PlayerInteractAlternate playerInteractAlternate;

    [Header("Interaction Settings")]
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField, Range(0f, 2.5f)] private float interactionRayStartDistance = 1f;
    [SerializeField, Range(0.1f, 2f)] private float interactionRayLenght = 1f;
    [SerializeField, Range(0.1f, 1f)] private float interactionSphereRadius = 2f;

    [Header("Debug")]
    [SerializeField] private bool drawRaycasts;

    private bool InteractionDownInput => interactionInput.GetInteractionDown();
    private bool InteractionHoldInput => interactionInput.GetInteractionHold();
    public Vector3 InteractionDirection => playerRotationHandler.DesiredFacingDirection.normalized;

    public bool IsInteracting { get; private set; }

    private float holdTimer;
    private bool inputDownToHold;

    private bool canHoldInteract;
    private bool previousCanHoldInteract;

    private CharacterController characterController;

    private IInteractable curentInteractable;

    public event EventHandler<OnInteractionEventArgs> OnInteractableSelected;
    public event EventHandler<OnInteractionEventArgs> OnInteractableDeselected;

    public event EventHandler<OnInteractionEventArgs> OnInteractionStarted;
    public event EventHandler<OnInteractionEventArgs> OnInteractionEnded;

    public event EventHandler<OnInteractionEventArgs> OnInteractionCompleted;
    public event EventHandler<OnInteractionEventArgs> OnHoldInteractionStopped;

    public event EventHandler<OnHoldInteractionEventArgs> OnHoldInteraction;

    public class OnInteractionEventArgs : EventArgs
    {
        public IInteractable interactable;
    }

    public class OnHoldInteractionEventArgs : EventArgs
    {
        public IHoldInteractable holdInteractable;
        public float holdTimer;
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        ResetInteractions();
    }
    private void Update()
    {
        HandleInteractableSelections();
        HandleInteractions();
    }

    private void HandleInteractableSelections()
    {
        IInteractable interactable = CheckForInteractable();

        if (interactable != null)
        {
            if (curentInteractable == null)
            {
                SelectInteractable(interactable);
            }
            else if (curentInteractable != interactable)
            {
                DeselectInteractable(curentInteractable);
                SelectInteractable(interactable);
            }
        }
        else if (curentInteractable != null)
        {
            DeselectInteractable(curentInteractable);
        }
    }

    private IInteractable CheckForInteractable()
    {
        RaycastHit[] hits = GetInteractableLayerHits();

        if (hits.Length == 0) return null;

        RaycastHit closestHit = hits[0];
        IInteractable interactable = null;
        float closestDistance = float.MaxValue;

        foreach(RaycastHit hit in hits)
        {
            IInteractable potentialInteractable = CheckIfRayHitHasInteractable(hit);

            if (potentialInteractable == null) continue;

            if (!potentialInteractable.IsSelectable) continue;

            float distance = Vector3.Distance(GetRaycastOrigin(), potentialInteractable.GetTransform().position);

            if (distance < closestDistance)
            {
                closestHit = hit;
                closestDistance = distance;
                interactable = potentialInteractable;
            }

        }

        return interactable;
    }

    private IInteractable CheckIfRayHitHasInteractable(RaycastHit hit)
    {
        if (hit.transform.TryGetComponent(out IInteractable hitInteractable))
        {
            return hitInteractable;
        }

        return null;
    }

    private void SelectInteractable(IInteractable interactable)
    {
        curentInteractable = interactable;

        interactable.Select();
        OnInteractableSelected?.Invoke(this, new OnInteractionEventArgs { interactable = interactable });

        //Debug.Log("Selected");
    }

    private void DeselectInteractable(IInteractable interactable)
    {
        curentInteractable = null;

        interactable.Deselect();
        OnInteractableDeselected?.Invoke(this, new OnInteractionEventArgs { interactable = interactable });

        ResetInteractions();

        //Debug.Log("Deselected");
    }

    private void HandleInteractions()
    {
        if(playerInteractAlternate.IsInteractingAlternate) { ResetInteractions(); return; }

        if (curentInteractable == null) { ResetInteractions(); return; }

        if(CheckIfHoldInteractable(curentInteractable)) HandleHoldInteractions(curentInteractable as IHoldInteractable);
        else HandleDownInteractions(curentInteractable);
    }

    private void HandleDownInteractions(IInteractable interactable)
    {    
        if (InteractionDownInput)
        {
            interactable.TryInteract();

            OnInteractionStarted?.Invoke(this, new OnInteractionEventArgs { interactable = interactable });
            OnInteractionCompleted?.Invoke(this, new OnInteractionEventArgs { interactable = interactable });
            OnInteractionEnded?.Invoke(this, new OnInteractionEventArgs { interactable = interactable });
        }
        
    }

    private void HandleHoldInteractions(IHoldInteractable holdInteractable)
    {  
        canHoldInteract = CanHoldInteract();

        if (InteractionDownInput) inputDownToHold = true;

        if (canHoldInteract)
        {
            if (!IsInteracting)
            {
                OnInteractionStarted?.Invoke(this, new OnInteractionEventArgs { interactable = holdInteractable });
                IsInteracting = true;
            }

            #region Optional to check if interaction will be successfull and execute FailInteract, OnHasAlreadyBeenInteracted,etc methods if won't. (Only One and ordered by priority)
            if (!holdInteractable.CheckSuccess())
            {
                OnInteractionEnded?.Invoke(this, new OnInteractionEventArgs { interactable = holdInteractable });
                ResetInteractions();
            }
            #endregion

            holdTimer += Time.deltaTime;
            float holdPercent = holdTimer / holdInteractable.HoldDuration;

            OnHoldInteraction?.Invoke(this, new OnHoldInteractionEventArgs { holdInteractable = holdInteractable, holdTimer = holdTimer });

            if (holdPercent >= 1)
            {
                holdInteractable.TryInteract();

                OnInteractionCompleted?.Invoke(this, new OnInteractionEventArgs { interactable = holdInteractable });
                OnInteractionEnded?.Invoke(this, new OnInteractionEventArgs { interactable = holdInteractable });

                ResetInteractions();
            }
        }
        else if (previousCanHoldInteract)
        {
            OnHoldInteractionStopped?.Invoke(this, new OnInteractionEventArgs { interactable = holdInteractable });
            OnInteractionEnded?.Invoke(this, new OnInteractionEventArgs { interactable = holdInteractable });

            ResetInteractions();
        }

        previousCanHoldInteract = canHoldInteract;
    }

    private void ResetInteractions()
    {
        holdTimer = 0f;
        inputDownToHold = false;
        IsInteracting = false;
    }

    private bool CheckIfHoldInteractable(IInteractable interactable) => (interactable is IHoldInteractable);

    private bool CanHoldInteract() => InteractionHoldInput && inputDownToHold && !playerHorizontalMovement.HasMovementInput();

    public RaycastHit[] GetInteractableLayerHits()
    {
        RaycastHit[] hits = Physics.SphereCastAll(GetRaycastOrigin(), interactionSphereRadius, InteractionDirection, interactionRayLenght, interactionLayer); ;

        if (drawRaycasts) Debug.DrawRay(GetRaycastOrigin(), InteractionDirection * interactionRayLenght, Color.yellow);

        return hits;
    }

    public Vector3 GetRaycastOrigin() => transform.position + characterController.center + InteractionDirection * interactionRayStartDistance;
}
