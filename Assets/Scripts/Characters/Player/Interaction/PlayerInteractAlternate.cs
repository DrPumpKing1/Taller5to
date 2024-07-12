using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInteractAlternate : MonoBehaviour
{
    public static PlayerInteractAlternate Instance { get; private set; }

    [Header("Enabler")]
    [SerializeField] private bool interactionAlternateEnabled;

    [Header("Components")]
    [SerializeField] private InteractionInput interactionInput;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private CheckGround checkGround;

    private bool InteractionAlternateDownInput => interactionInput.GetInteractionAlternateDown();
    private bool InteractionAlternateHoldInput => interactionInput.GetInteractionAlternateHold();

    private bool CanProcessInteractionInput => interactionInput.CanProcessInteractionInput();

    public bool IsInteractingAlternate { get; private set; }
    public bool InteractionAlternateEnabled => interactionAlternateEnabled;

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

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        ResetInteractionsAlternate();
    }
    private void Update()
    {
        HandleInteractableAlternateSelections();
        HandleInteractionsAlternate();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one PlayerInteractAlternate instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void HandleInteractableAlternateSelections()
    {
        IInteractableAlternate interactableAlternate = CheckForInteractableAlternateWorldSpace();

        CheckIfInteractableIsTheSame(ref interactableAlternate);

        if (!CanInteractAlternate()) interactableAlternate = null;

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
    private void HandleInteractionsAlternate()
    {
        if (!CanInteractAlternate()) return;

        if(playerInteract.IsInteracting) { ResetInteractionsAlternate(); return; }

        if (curentInteractableAlternate == null) { ResetInteractionsAlternate(); return; }

        if (CheckIfHoldInteractableAlternate(curentInteractableAlternate)) HandleHoldInteractionsAlternate(curentInteractableAlternate as IHoldInteractableAlternate);
        else HandleDownInteractionsAlternate(curentInteractableAlternate);
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

        IInteractableAlternate potentialInteractableAlternate = CheckIfRayHitHasInteractableAlternate(hit);

        if (potentialInteractableAlternate == null) return null;
        if (!potentialInteractableAlternate.IsSelectableAlternate) return null;
        if (!CheckInteractableAlternateInRange(potentialInteractableAlternate)) return null;

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
    }

    private void DeselectInteractableAlternate(IInteractableAlternate interactableAlternate)
    {
        if (IsInteractingAlternate)
        {
            OnInteractionAlternateEnded?.Invoke(this, new OnInteractionAlternateEventArgs { interactableAlternate = interactableAlternate });

            if (CheckIfHoldInteractableAlternate(interactableAlternate))
            {
                OnHoldInteractionAlternateStopped?.Invoke(this, new OnInteractionAlternateEventArgs { interactableAlternate = interactableAlternate as IHoldInteractableAlternate });
                (interactableAlternate as IHoldInteractableAlternate).HoldInteractionAlternateEnd();
            }
        }

        curentInteractableAlternate = null;

        interactableAlternate.DeselectAlternate();
        OnInteractableAlternateDeselected?.Invoke(this, new OnInteractionAlternateEventArgs { interactableAlternate = interactableAlternate });

        ResetInteractionsAlternate();
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
                OnInteractionAlternateCompleted?.Invoke(this, new OnInteractionAlternateEventArgs { interactableAlternate = holdInteractableAlternate });
                OnInteractionAlternateEnded?.Invoke(this, new OnInteractionAlternateEventArgs { interactableAlternate = holdInteractableAlternate });

                holdInteractableAlternate.TryInteractAlternate();
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

    private bool CheckInteractableAlternateInRange(IInteractableAlternate interactableAlternate)
    {
        if (Vector3.Distance(GeneralMethods.SupressYComponent(interactableAlternate.GetTransform().position), GeneralMethods.SupressYComponent(transform.position)) > interactableAlternate.HorizontalInteractionRange) return false;
        if (Mathf.Abs(interactableAlternate.GetTransform().position.y - transform.position.y) > interactableAlternate.VerticalInteractionRange) return false;

        return true;
    }

    private bool CheckIfHoldInteractableAlternate(IInteractableAlternate interactableAlternate) => (interactableAlternate is IHoldInteractableAlternate);

    private bool CanHoldInteractAlternate() => InteractionAlternateHoldInput && inputDownToHold; //&& !playerHorizontalMovement.HasMovementInput();

    private bool CanInteractAlternate()
    {
        if (!CanProcessInteractionInput) return false;
        //if (!checkGround.IsGrounded) return false;
        if (!playerInteract.InteractionEnabled) return false;
        if (!interactionAlternateEnabled) return false;

        return true;
    }
}
