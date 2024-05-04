using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Enabler")]
    [SerializeField] private bool interactionEnabled;

    [Header("Components")]
    [SerializeField] private InteractionInput interactionInput;
    [SerializeField] private PlayerRotationHandler playerRotationHandler;
    [SerializeField] private PlayerInteractAlternate playerInteractAlternate;
    [SerializeField] private CheckGround checkGround;

    [Header("Interaction Settings")]
    [SerializeField] private LayerMask interactionLayer;

    [Header("Normal Interaction Settings")]
    [SerializeField, Range(0f, 2.5f)] private float interactionRayStartDistance = 1f;
    [SerializeField, Range(0.1f, 2.5f)] private float interactionRayLenght = 1f;
    [SerializeField, Range(0.1f, 1f)] private float interactionSphereRadius = 2f;

    [Header("World Space Interaction Settings")]
    [SerializeField, Range(200f,350f)] private float maxDistanceFromCamera;

    [Header("Debug")]
    [SerializeField] private bool drawRaycasts;

    private bool InteractionDownInput => interactionInput.GetInteractionDown();
    private bool InteractionHoldInput => interactionInput.GetInteractionHold();
    private bool CanProcessInteractionInput => interactionInput.CanProcessInteractionInput();
    public Vector3 InteractionDirection => playerRotationHandler.DesiredFacingDirection.normalized;

    public bool IsInteracting { get; private set; }
    public bool InteractionEnabled { get { return interactionEnabled; } }

    private float holdTimer;
    private bool inputDownToHold;

    private bool canHoldInteract;
    private bool previousCanHoldInteract;

    private CapsuleCollider capsulleCollider;

    private IInteractable currentInteractable;
    public IInteractable CurrentInteractable { get { return currentInteractable; } }

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
        capsulleCollider = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        ResetInteractions();
    }
    private void Update()
    {
        if (!CanProcessInteractionInput) return;

        HandleInteractableSelections();

        if (!checkGround.IsGrounded) return;
        if (!interactionEnabled) return;

        HandleInteractions();
    }

    private void HandleInteractableSelections()
    {
        IInteractable interactable = CheckForInteractableWorldSpace();

        if (!checkGround.IsGrounded) interactable = null;
        if (!interactionEnabled) interactable = null;

        if (interactable != null)
        {
            if (currentInteractable == null)
            {
                SelectInteractable(interactable);
            }
            else if (currentInteractable != interactable)
            {
                DeselectInteractable(currentInteractable);
                SelectInteractable(interactable);
            }
        }
        else if (currentInteractable != null)
        {
            DeselectInteractable(currentInteractable);
        }
    }

    private IInteractable CheckForInteractableNormal()
    {
        RaycastHit[] hits = GetInteractableLayerHitsNormal();

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

    private IInteractable CheckForInteractableWorldSpace()
    {
        RaycastHit hit = GetInteractableLayerHitsWorldSpace();

        if (hit.collider == null) return null;


        IInteractable potentialInteractable = CheckIfRayHitHasInteractable(hit);

        if (potentialInteractable == null) return null;
        if (!potentialInteractable.IsSelectable) return null;
        if (!CheckInteractableInRange(potentialInteractable)) return null;

        return potentialInteractable;
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
        currentInteractable = interactable;

        interactable.Select();
        OnInteractableSelected?.Invoke(this, new OnInteractionEventArgs { interactable = interactable });
    }

    private void DeselectInteractable(IInteractable interactable)
    {
        if (IsInteracting)
        {
            OnInteractionEnded?.Invoke(this, new OnInteractionEventArgs { interactable = interactable });
        }

        currentInteractable = null;

        interactable.Deselect();
        OnInteractableDeselected?.Invoke(this, new OnInteractionEventArgs { interactable = interactable });

        ResetInteractions();
    }

    private void HandleInteractions()
    {
        if(playerInteractAlternate.IsInteractingAlternate) { ResetInteractions(); return; }

        if (currentInteractable == null) { ResetInteractions(); return; }

        if(CheckIfHoldInteractable(currentInteractable)) HandleHoldInteractions(currentInteractable as IHoldInteractable);
        else HandleDownInteractions(currentInteractable);
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
                holdInteractable.HoldInteractionStart();
                IsInteracting = true;
            }

            #region Optional to check if interaction will be successfull and execute FailInteract, OnHasAlreadyBeenInteracted,etc methods if won't. (Only One and ordered by priority)
            if (!holdInteractable.CheckSuccess())
            {
                OnInteractionEnded?.Invoke(this, new OnInteractionEventArgs { interactable = holdInteractable });
                holdInteractable.HoldInteractionEnd();
                ResetInteractions();
            }
            #endregion

            holdTimer += Time.deltaTime;
            float holdPercent = holdTimer / holdInteractable.HoldDuration;

            OnHoldInteraction?.Invoke(this, new OnHoldInteractionEventArgs { holdInteractable = holdInteractable, holdTimer = holdTimer });
            holdInteractable.ContinousHoldInteraction(holdTimer);

            if (holdPercent >= 1)
            {
                holdInteractable.TryInteract();

                OnInteractionCompleted?.Invoke(this, new OnInteractionEventArgs { interactable = holdInteractable });
                OnInteractionEnded?.Invoke(this, new OnInteractionEventArgs { interactable = holdInteractable });
                holdInteractable.HoldInteractionEnd();
                ResetInteractions();
            }
        }
        else if (previousCanHoldInteract && IsInteracting)
        {
            OnHoldInteractionStopped?.Invoke(this, new OnInteractionEventArgs { interactable = holdInteractable });
            OnInteractionEnded?.Invoke(this, new OnInteractionEventArgs { interactable = holdInteractable });
            holdInteractable.HoldInteractionEnd();
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

    private bool CheckInteractableInRange(IInteractable interactable)
    {
        if (Vector3.Distance(GeneralMethods.SupressYComponent(interactable.GetTransform().position), GeneralMethods.SupressYComponent(transform.position)) > interactable.HorizontalInteractionRange) return false;
        if (Mathf.Abs(interactable.GetTransform().position.y - transform.position.y) > interactable.VerticalInteractionRange) return false;

        return true;
    }

    private bool CheckIfHoldInteractable(IInteractable interactable) => (interactable is IHoldInteractable);

    private bool CanHoldInteract() => InteractionHoldInput && inputDownToHold; //&& !playerHorizontalMovement.HasMovementInput();

    public RaycastHit[] GetInteractableLayerHitsNormal()
    {
        RaycastHit[] hits = Physics.SphereCastAll(GetRaycastOrigin(), interactionSphereRadius, InteractionDirection, interactionRayLenght, interactionLayer); ;

        if (drawRaycasts) Debug.DrawRay(GetRaycastOrigin(), InteractionDirection * interactionRayLenght, Color.yellow);

        return hits;
    }

    public RaycastHit GetInteractableLayerHitsWorldSpace()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (drawRaycasts) Debug.DrawRay(ray.origin, ray.direction * maxDistanceFromCamera, Color.red);

        bool hitInteractable = Physics.Raycast(ray, out RaycastHit hit, maxDistanceFromCamera, interactionLayer);

        return hit;
    }

    public Vector3 GetRaycastOrigin() => transform.position + capsulleCollider.center + InteractionDirection * interactionRayStartDistance;
}
