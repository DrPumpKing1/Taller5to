using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRotationHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PetPlayerAttachment petPlayerAttachment;
    [SerializeField] private PlayerRotationHandler playerRotationHandler;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private PlayerInteractAlternate playerInteractAlternate;

    [Header("Rotation Settings")]
    [SerializeField, Range(1f, 100f)] private float smoothRotateFactor = 5f;

    [Header("Starting Rotation Settings")]
    [SerializeField] private bool applyStartingRotation;
    [SerializeField] private Vector3 startingFacingDirection;

    private Vector3 PlayerFacingDirection => playerRotationHandler.FacingDirection;
    public Vector3 FacingDirection { get; private set; }

    private Vector3 desiredFacingDirection;

    private Transform currentInteractionAttentionTransform;
    private bool interactingRotate;


    private void OnEnable()
    {
        playerInteract.OnInteractionStarted += PlayerInteract_OnInteractionStarted;
        playerInteract.OnInteractionEnded += PlayerInteract_OnInteractionEnded;

        playerInteractAlternate.OnInteractionAlternateStarted += PlayerInteractAlternate_OnInteractionAlternateStarted;
        playerInteractAlternate.OnInteractionAlternateEnded += PlayerInteractAlternate_OnInteractionAlternateEnded;
    }

    private void OnDisable()
    {
        playerInteract.OnInteractionStarted -= PlayerInteract_OnInteractionStarted;
        playerInteract.OnInteractionEnded -= PlayerInteract_OnInteractionEnded;

        playerInteractAlternate.OnInteractionAlternateStarted -= PlayerInteractAlternate_OnInteractionAlternateStarted;
        playerInteractAlternate.OnInteractionAlternateEnded -= PlayerInteractAlternate_OnInteractionAlternateEnded;
    }

    private void Start()
    {
        InitializeVariables();
        InitializeStartingRotation();
    }

    private void Update()
    {
        HandleRotationState();
        AvoidXZRotation();
    }

    private void InitializeVariables()
    {
        interactingRotate = false;
    }

    private void InitializeStartingRotation()
    {
        FacingDirection = transform.forward;

        if (applyStartingRotation) FacingDirection = startingFacingDirection;

        FacingDirection.Normalize();

        RotateInstantlyTowardsDirection(FacingDirection);
    }

    private void HandleRotationState()
    {
        switch (PetStateHandler.Instance.PetState)
        {
            case PetStateHandler.State.Still:
                StillLogic();
                break;
            case PetStateHandler.State.FollowingPlayer:
                FollowingPlayerLogic();
                break;
            case PetStateHandler.State.OnGuidance:
                OnGuidanceLogic();
                break;
        }
    }

    private void StillLogic()
    {
        //Nothing
    }

    private void FollowingPlayerLogic()
    {
        DefineDesiredPlayerFollowFacingDirection();
        ApplyRotation();
    }

    private void OnGuidanceLogic()
    {
        if (PetStateHandler.Instance.CurrentPetGuidanceObject == null) return;

        SetDesiredFacingDirectionTowardsTransform(PetStateHandler.Instance.CurrentPetGuidanceObject.lookTransform);
        ApplyRotation();
    }

    private void DefineDesiredPlayerFollowFacingDirection()
    {
        if (currentInteractionAttentionTransform) //If interacting
        {
            SetDesiredFacingDirectionTowardsTransform(currentInteractionAttentionTransform);
            if (!interactingRotate) ClearInteractionAttentionTransform();
            return;
        }

        if (playerRotationHandler)
        {
            SetDesiredFacingDirectionTowardsDirection(PlayerFacingDirection);
        }
    }

    private void ApplyRotation()
    {
        if (desiredFacingDirection.magnitude <= 0f) return;

        RotateTowardsDirection(desiredFacingDirection);
    }

    private void RotateInstantlyTowardsDirection(Vector3 direction) => transform.localRotation = Quaternion.LookRotation(direction);

    private void SetDesiredFacingDirectionTowardsTransform(Transform lookTransform) => desiredFacingDirection = (lookTransform.position - transform.position).normalized;
    private void SetDesiredFacingDirectionTowardsDirection(Vector3 direction) => desiredFacingDirection = direction;
    private void RotateTowardsDirection(Vector3 direction)
    {
        FacingDirection = Vector3.Slerp(FacingDirection, direction, smoothRotateFactor * Time.deltaTime);
        FacingDirection.Normalize();

        transform.localRotation = Quaternion.LookRotation(FacingDirection);
    }
    private void AvoidXZRotation() => transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
    
    #region Interaction Methods
    private void SetInteractionAttentionTransform(Transform interactionAttentionTransform) => currentInteractionAttentionTransform = interactionAttentionTransform;
    private void ClearInteractionAttentionTransform() => currentInteractionAttentionTransform = null;
    #endregion

    private void RotateTowardsTransform(Transform lookTransform)
    {
        Vector3 facingDirection = (lookTransform.position - transform.position).normalized;
        RotateTowardsDirection(facingDirection);
    }

    #region PlayerInteractionSubscriptions
    private void PlayerInteract_OnInteractionStarted(object sender, PlayerInteract.OnInteractionEventArgs e)
    {
        if (!e.interactable.GrabPetAttention) return;
        if(e.interactable.GetInteractionAttentionTransform() == null) return;

        interactingRotate = true;
        SetInteractionAttentionTransform(e.interactable.GetInteractionAttentionTransform());
    }

    private void PlayerInteract_OnInteractionEnded(object sender, PlayerInteract.OnInteractionEventArgs e)
    {
        interactingRotate = false;
    }
    #endregion

    #region PlayerInteractionAlternateSubscriptions

    private void PlayerInteractAlternate_OnInteractionAlternateStarted(object sender, PlayerInteractAlternate.OnInteractionAlternateEventArgs e)
    {
        if (!e.interactableAlternate.GrabPetAttention) return;
        if (e.interactableAlternate.GetInteractionAlternateAttentionTransform() == null) return;

        interactingRotate = true;
        SetInteractionAttentionTransform(e.interactableAlternate.GetInteractionAlternateAttentionTransform());
    }
    private void PlayerInteractAlternate_OnInteractionAlternateEnded(object sender, PlayerInteractAlternate.OnInteractionAlternateEventArgs e)
    {
        interactingRotate = false;
    }
    #endregion
}
