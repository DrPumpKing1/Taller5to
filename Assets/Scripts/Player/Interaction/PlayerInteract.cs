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

    [Header("Interaction Settings")]
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField, Range(0.5f, 2f)] private float interactionRayLenght = 1f;
    [SerializeField, Range(0.5f, 1f)] private float interactionSphereRadius = 2f;

    [Header("Debug")]
    [SerializeField] private bool drawRaycasts;

    private bool InteractionDownInput => interactionInput.GetInteractionDown();
    private bool InteractionHoldInput => interactionInput.GetInteractionHold();
    private Vector3 interactionDirection => playerRotationHandler.FacingDirection;

    public bool IsInteracting;

    private CharacterController characterController;
    private float holdTimer;
    private bool inputDownToHold;
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
        Vector3 origin = transform.position + characterController.center;
        RaycastHit[] hits = Physics.SphereCastAll(origin, interactionSphereRadius, interactionDirection, interactionRayLenght, interactionLayer);

        if (drawRaycasts) Debug.DrawRay(origin, interactionDirection * interactionRayLenght, Color.yellow);

        if (hits.Length == 0) return null;

        IInteractable interactable = null;

        RaycastHit closestHit = hits[0];
        CheckIfRayHitHasInteractable(closestHit, ref interactable);
        float closestDistance = Vector3.Distance(transform.position, closestHit.point);

        foreach(RaycastHit hit in hits)
        {
            float distance = Vector3.Distance(transform.position, hit.point);

            if (distance < closestDistance)
            {
                closestHit = hit;
                CheckIfRayHitHasInteractable(closestHit, ref interactable);
            }
        }

        return interactable;
    }

    private void CheckIfRayHitHasInteractable(RaycastHit hit, ref IInteractable interactable)
    {
        if (hit.transform.TryGetComponent(out IInteractable hitInteractable))
        {
            if (!hitInteractable.IsSelectable) return;
            interactable = hitInteractable;
        }
    }

    private void SelectInteractable(IInteractable interactable)
    {
        curentInteractable = interactable;

        interactable.OnSelection();
        OnInteractableSelected?.Invoke(this, new OnInteractionEventArgs { interactable = interactable });

        //Debug.Log("Selected");
    }
    private void DeselectInteractable(IInteractable interactable)
    {
        curentInteractable = null;

        interactable.OnDeselection();
        OnInteractableDeselected?.Invoke(this, new OnInteractionEventArgs { interactable = interactable });

        ResetInteractions();

        //Debug.Log("Deselected");
    }

    private void HandleInteractions()
    {
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

    private void HandleHoldInteractions(IHoldInteractable holdInteractable )
    {  
        if (InteractionDownInput) inputDownToHold = true;

        if (CanHoldInteract())
        {
            if (!IsInteracting)
            {
                OnInteractionStarted?.Invoke(this, new OnInteractionEventArgs { interactable = holdInteractable });
                IsInteracting = true;
            }

            #region Optional if inmediate FailInteract after start holding
            if (!curentInteractable.IsInteractable)
            {
                curentInteractable.FailInteract();
                OnInteractionEnded?.Invoke(this, new OnInteractionEventArgs { interactable = curentInteractable });

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
        else
        {
            OnHoldInteractionStopped?.Invoke(this, new OnInteractionEventArgs { interactable = holdInteractable });
            OnInteractionEnded?.Invoke(this, new OnInteractionEventArgs { interactable = holdInteractable });

            ResetInteractions();
        }    
    }

    private void ResetInteractions()
    {
        holdTimer = 0f;
        inputDownToHold = false;
        IsInteracting = false;
    }

    private bool CheckIfHoldInteractable(IInteractable interactable) => (interactable is IHoldInteractable);

    private bool CanHoldInteract() => InteractionHoldInput && inputDownToHold && !playerHorizontalMovement.HasMovementInput();
}
