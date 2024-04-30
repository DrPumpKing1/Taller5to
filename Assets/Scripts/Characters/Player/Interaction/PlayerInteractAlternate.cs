using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInteractAlternate : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InteractionInput interactionInput;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private CheckGround checkGround;

    private bool InteractionAlternateDownInput => interactionInput.GetInteractionAlternateDown();
    private bool InteractionAlternateHoldInput => interactionInput.GetInteractionAlternateHold();

    public bool IsInteractingAlternate { get; private set; }

    private float holdTimer;
    private bool inputDownToHold;

    private bool canHoldInteractAlternate;
    private bool previousCanHoldInteractAlternate;

    private IInteractableAlternate curentInteractableAlternate;

    public event EventHandler<OnInteractionAlternateEventArgs> OnInteractableAlternateSelected;
    public event EventHandler<OnInteractionAlternateEventArgs> OnInteractableAlternateDeselected;

    public event EventHandler<OnInteractionAlternateEventArgs> OnInteractionAlternateStarted;
    public event EventHandler<OnInteractionAlternateEventArgs> OnInteractionAlternateEnded;

    public event EventHandler<OnInteractionAlternateEventArgs> OnInteractionAlternateCompleted;
    public event EventHandler<OnInteractionAlternateEventArgs> OnHoldInteractionAlternateStopped;

    public event EventHandler<OnHoldInteractionAlternateEventArgs> OnHoldInteractionAlternate;

    public class OnInteractionAlternateEventArgs : EventArgs
    {
        public IInteractableAlternate interactableAlternate;
    }

    public class OnHoldInteractionAlternateEventArgs : EventArgs
    {
        public IHoldInteractableAlternate holdInteractableAlternate;
        public float holdTimer;
    }

    private void Start()
    {
        ResetInteractionsAlternate();
    }
    private void Update()
    {
        HandleInteractableAlternateSelections();

        if (!playerInteract.InteractionEnabled) return;
        if (!checkGround.IsGrounded) return;

        HandleInteractionsAlternate();
    }

    private void HandleInteractableAlternateSelections()
    {
        IInteractableAlternate interactableAlternate = CheckForInteractableAlternateWorldSpace();

        CheckIfInteractableIsTheSame(ref interactableAlternate);

        if (!playerInteract.InteractionEnabled) interactableAlternate = null;
        if (!checkGround.IsGrounded) interactableAlternate = null;

        if (interactableAlternate != null)
        {
            if (curentInteractableAlternate == null)
            {
                SelectInteractableAlternate(interactableAlternate);
            }
            else if (curentInteractableAlternate != interactableAlternate)
            {
                DeselectInteractableAlternate(curentInteractableAlternate);
                SelectInteractableAlternate(interactableAlternate);
            }
        }
        else if (curentInteractableAlternate != null)
        {
            DeselectInteractableAlternate(curentInteractableAlternate);
        }
    }

    private void CheckIfInteractableIsTheSame(ref IInteractableAlternate interactableAlternate)
    {
        try
        {
            if (playerInteract.CurrentInteractable != null && interactableAlternate != null)
            {
                if (playerInteract.CurrentInteractable.GetTransform() != interactableAlternate.GetTransform())
                {
                    interactableAlternate = null;
                }
            }
        }
        catch
        {
            Debug.Log("Avoided Exception");
        }
    }

    private IInteractableAlternate CheckForInteractableAlternateNormal()
    {
        RaycastHit[] hits = playerInteract.GetInteractableLayerHitsNormal();

        if (hits.Length == 0) return null;

        RaycastHit closestHit = hits[0];
        IInteractableAlternate interactableAlternate = null;
        float closestDistance = float.MaxValue;

        foreach (RaycastHit hit in hits)
        {
            IInteractableAlternate potentialInteractableAlternate = CheckIfRayHitHasInteractableAlternate(hit);

            if (potentialInteractableAlternate == null) continue;

            if (!potentialInteractableAlternate.IsSelectableAlternate) continue;

            float distance = Vector3.Distance(playerInteract.GetRaycastOrigin(), potentialInteractableAlternate.GetTransform().position);

            if (distance < closestDistance)
            {
                closestHit = hit;
                closestDistance = distance;
                interactableAlternate = potentialInteractableAlternate;
            }

        }

        return interactableAlternate;
    }

    private IInteractableAlternate CheckForInteractableAlternateWorldSpace()
    {
        RaycastHit hit = playerInteract.GetInteractableLayerHitsWorldSpace();

        if (hit.collider == null) return null;
        if (Vector3.Distance(GeneralMethods.SupressYComponent(hit.collider.transform.position), GeneralMethods.SupressYComponent(transform.position)) > playerInteract.MaxHorizontalDistanceFromPlayer) return null;
        if (Mathf.Abs(hit.collider.transform.position.y - transform.position.y) > playerInteract.MaxVerticalDistanceFromPlayer) return null;

        IInteractableAlternate potentialInteractableAlternate = CheckIfRayHitHasInteractableAlternate(hit);

        if (potentialInteractableAlternate == null) return null;
        if (!potentialInteractableAlternate.IsSelectableAlternate) return null;

        return potentialInteractableAlternate;
    }

    private IInteractableAlternate CheckIfRayHitHasInteractableAlternate(RaycastHit hit)
    {
        if (hit.transform.TryGetComponent(out IInteractableAlternate hitInteractableAlternate))
        {
            return hitInteractableAlternate;
        }

        return null;
    }

    private void SelectInteractableAlternate(IInteractableAlternate interactableAlternate)
    {
        curentInteractableAlternate = interactableAlternate;

        interactableAlternate.SelectAlternate();
        OnInteractableAlternateSelected?.Invoke(this, new OnInteractionAlternateEventArgs { interactableAlternate = interactableAlternate });

        //Debug.Log("Selected");
    }

    private void DeselectInteractableAlternate(IInteractableAlternate interactableAlternate)
    {
        if (IsInteractingAlternate)
        {
            OnInteractionAlternateEnded?.Invoke(this, new OnInteractionAlternateEventArgs { interactableAlternate = interactableAlternate });
        }

        curentInteractableAlternate = null;

        interactableAlternate.DeselectAlternate();
        OnInteractableAlternateDeselected?.Invoke(this, new OnInteractionAlternateEventArgs { interactableAlternate = interactableAlternate });

        ResetInteractionsAlternate();

        //Debug.Log("Deselected");
    }

    private void HandleInteractionsAlternate()
    {
        if(playerInteract.IsInteracting) { ResetInteractionsAlternate(); return; }

        if (curentInteractableAlternate == null) { ResetInteractionsAlternate(); return; }

        if (CheckIfHoldInteractableAlternate(curentInteractableAlternate)) HandleHoldInteractionsAlternate(curentInteractableAlternate as IHoldInteractableAlternate);
        else HandleDownInteractionsAlternate(curentInteractableAlternate);
    }

    private void HandleDownInteractionsAlternate(IInteractableAlternate interactableAlternate)
    {
        if (InteractionAlternateDownInput)
        {
            interactableAlternate.TryInteractAlternate();

            OnInteractionAlternateStarted?.Invoke(this, new OnInteractionAlternateEventArgs { interactableAlternate = interactableAlternate });
            OnInteractionAlternateCompleted?.Invoke(this, new OnInteractionAlternateEventArgs { interactableAlternate = interactableAlternate });
            OnInteractionAlternateEnded?.Invoke(this, new OnInteractionAlternateEventArgs { interactableAlternate = interactableAlternate });
        }
    }

    private void HandleHoldInteractionsAlternate(IHoldInteractableAlternate holdInteractableAlternate)
    {
        canHoldInteractAlternate = CanHoldInteractAlternate();

        if (InteractionAlternateDownInput) inputDownToHold = true;

        if (canHoldInteractAlternate)
        {
            if (!IsInteractingAlternate)
            {
                OnInteractionAlternateStarted?.Invoke(this, new OnInteractionAlternateEventArgs { interactableAlternate = holdInteractableAlternate });
                holdInteractableAlternate.HoldInteractionAlternateStart();
                IsInteractingAlternate = true;
            }

            #region Optional to check if interaction will be successfull and execute FailInteract, OnHasAlreadyBeenInteracted,etc methods if won't. (Only One and ordered by priority)
            if (!holdInteractableAlternate.CheckSuccessAlternate())
            {
                OnInteractionAlternateEnded?.Invoke(this, new OnInteractionAlternateEventArgs { interactableAlternate = holdInteractableAlternate });
                holdInteractableAlternate.HoldInteractionAlternateEnd();
                ResetInteractionsAlternate();
            }
            #endregion

            holdTimer += Time.deltaTime;
            float holdPercent = holdTimer / holdInteractableAlternate.HoldDurationAlternate;

            OnHoldInteractionAlternate?.Invoke(this, new OnHoldInteractionAlternateEventArgs { holdInteractableAlternate = holdInteractableAlternate, holdTimer = holdTimer });
            holdInteractableAlternate.ContinousHoldInteractionAlternate(holdTimer);

            if (holdPercent >= 1)
            {
                holdInteractableAlternate.TryInteractAlternate();

                OnInteractionAlternateCompleted?.Invoke(this, new OnInteractionAlternateEventArgs { interactableAlternate = holdInteractableAlternate });
                OnInteractionAlternateEnded?.Invoke(this, new OnInteractionAlternateEventArgs { interactableAlternate = holdInteractableAlternate });
                holdInteractableAlternate.HoldInteractionAlternateEnd();

                ResetInteractionsAlternate();
            }
        }
        else if (previousCanHoldInteractAlternate && IsInteractingAlternate)
        { 
            OnHoldInteractionAlternateStopped?.Invoke(this, new OnInteractionAlternateEventArgs { interactableAlternate = holdInteractableAlternate });
            OnInteractionAlternateEnded?.Invoke(this, new OnInteractionAlternateEventArgs { interactableAlternate = holdInteractableAlternate });
            holdInteractableAlternate.HoldInteractionAlternateEnd();

            ResetInteractionsAlternate();
        }

        previousCanHoldInteractAlternate = canHoldInteractAlternate;
    }

    private void ResetInteractionsAlternate()
    {
        holdTimer = 0f;
        inputDownToHold = false;
        IsInteractingAlternate = false;
    }

    private bool CheckIfHoldInteractableAlternate(IInteractableAlternate interactableAlternate) => (interactableAlternate is IHoldInteractableAlternate);

    private bool CanHoldInteractAlternate() => InteractionAlternateHoldInput && inputDownToHold; //&& !playerHorizontalMovement.HasMovementInput();
}
