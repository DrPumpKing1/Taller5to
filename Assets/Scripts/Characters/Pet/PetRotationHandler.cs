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

    [Header("States")]
    [SerializeField] private State state;

    public enum State { Still, FollowingPlayer, OnGuidance }

    public State RotationState => state;

    private Vector3 PlayerFacingDirection => playerRotationHandler.FacingDirection;
    public Vector3 FacingDirection { get; private set; }

    private Vector3 desiredFacingDirection;

    private Transform currentAttentionTransform;

    private Transform currentGuidanceLookTransform;

    private void OnEnable()
    {
        PetPlayerAttachment.OnVyrxAttachToPlayer += PetPlayerAttachment_OnVyrxAttachToPlayer;
        PetPlayerAttachment.OnVyrxUnattachToPlayer += PetPlayerAttachment_OnVyrxUnattachToPlayer;

        PetGuidanceListener.OnPetGuidanceStart += PetGuidanceListener_OnPetGuidanceStart;
        PetGuidanceListener.OnPetGuidanceEnd += PetGuidanceListener_OnPetGuidanceEnd;

        playerInteract.OnInteractionStarted += PlayerInteract_OnInteractionStarted;
        playerInteract.OnInteractionEnded += PlayerInteract_OnInteractionEnded;

        playerInteractAlternate.OnInteractionAlternateStarted += PlayerInteractAlternate_OnInteractionAlternateStarted;
        playerInteractAlternate.OnInteractionAlternateEnded += PlayerInteractAlternate_OnInteractionAlternateEnded;
    }

    private void OnDisable()
    {
        PetPlayerAttachment.OnVyrxAttachToPlayer -= PetPlayerAttachment_OnVyrxAttachToPlayer;
        PetPlayerAttachment.OnVyrxUnattachToPlayer -= PetPlayerAttachment_OnVyrxUnattachToPlayer;

        PetGuidanceListener.OnPetGuidanceStart -= PetGuidanceListener_OnPetGuidanceStart;
        PetGuidanceListener.OnPetGuidanceEnd -= PetGuidanceListener_OnPetGuidanceEnd;

        playerInteract.OnInteractionStarted -= PlayerInteract_OnInteractionStarted;
        playerInteract.OnInteractionEnded -= PlayerInteract_OnInteractionEnded;

        playerInteractAlternate.OnInteractionAlternateStarted -= PlayerInteractAlternate_OnInteractionAlternateStarted;
        playerInteractAlternate.OnInteractionAlternateEnded -= PlayerInteractAlternate_OnInteractionAlternateEnded;
    }

    private void Start()
    {
        InitializeStartingRotation();
        ClearGuidanceLookTransform();
    }

    private void Update()
    {
        HandleRotationState();
    }

    private void InitializeStartingRotation()
    {
        FacingDirection = transform.forward;

        if (applyStartingRotation) FacingDirection = startingFacingDirection;

        FacingDirection.Normalize();

        RotateInstantlyTowardsDirection(FacingDirection);
    }

    private void SetRotationState(State state) => this.state = state;

    private void HandleRotationState()
    {
        switch (state)
        {
            case State.Still:
                StillLogic();
                break;
            case State.FollowingPlayer:
                FollowingPlayerLogic();
                break;
            case State.OnGuidance:
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
        if (currentGuidanceLookTransform == null) return;

        SetDesiredFacingDirectionTowardsTransform(currentGuidanceLookTransform);
        ApplyRotation();
    }

    private void DefineDesiredPlayerFollowFacingDirection()
    {
        if (currentAttentionTransform) //If interacting
        {
            SetDesiredFacingDirectionTowardsTransform(currentAttentionTransform);
            //if(!interactingRotate) currentAttentionTransform = null;
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
    private void SetAttentionTransform(Transform attentionTransform) => currentAttentionTransform = attentionTransform;
    private void ClearAttentionTransform() => currentAttentionTransform = null;

    private void SetDesiredFacingDirectionTowardsTransform(Transform lookTransform) => desiredFacingDirection = (lookTransform.position - transform.position).normalized;
    private void SetDesiredFacingDirectionTowardsDirection(Vector3 direction) => desiredFacingDirection = direction;
    private void RotateTowardsDirection(Vector3 direction)
    {
        FacingDirection = Vector3.Slerp(FacingDirection, direction, smoothRotateFactor * Time.deltaTime);
        FacingDirection.Normalize();

        transform.localRotation = Quaternion.LookRotation(FacingDirection);
    }

    private void RotateTowardsTransform(Transform lookTransform)
    {
        Vector3 facingDirection = (lookTransform.position - transform.position).normalized;
        RotateTowardsDirection(facingDirection);
    }

    #region Guidance Methods
    private void SetGuidanceLookTransform(Transform guidanceLookTransform) => currentGuidanceLookTransform = guidanceLookTransform;
    private void ClearGuidanceLookTransform() => currentGuidanceLookTransform = null;

    #endregion

    #region PetGuidanceListener Subscriptions
    private void PetGuidanceListener_OnPetGuidanceStart(object sender, PetGuidanceListener.OnPetGuidanceEventArgs e)
    {
        SetGuidanceLookTransform(e.lookTransform);
        SetRotationState(State.OnGuidance);
    }

    private void PetGuidanceListener_OnPetGuidanceEnd(object sender, PetGuidanceListener.OnPetGuidanceEventArgs e)
    {
        ClearGuidanceLookTransform();
        SetRotationState(State.FollowingPlayer);
    }
    #endregion


    #region PetPlayerAttachment Subscriptions
    private void PetPlayerAttachment_OnVyrxAttachToPlayer(object sender, EventArgs e)
    {
        SetRotationState(State.FollowingPlayer);
    }

    private void PetPlayerAttachment_OnVyrxUnattachToPlayer(object sender, EventArgs e)
    {
        SetRotationState(State.Still);
    }
    #endregion


    #region PlayerInteractionSubscriptions
    private void PlayerInteract_OnInteractionStarted(object sender, PlayerInteract.OnInteractionEventArgs e)
    {
        if (!e.interactable.GrabPetAttention) return;

        SetAttentionTransform(e.interactable.AttentionTransform);
    }

    private void PlayerInteract_OnInteractionEnded(object sender, PlayerInteract.OnInteractionEventArgs e)
    {
        ClearAttentionTransform();
    }
    #endregion

    #region PlayerInteractionAlternateSubscriptions

    private void PlayerInteractAlternate_OnInteractionAlternateStarted(object sender, PlayerInteractAlternate.OnInteractionAlternateEventArgs e)
    {
        if (!e.interactableAlternate.GrabPetAttention) return;

        SetAttentionTransform(e.interactableAlternate.AttentionTransform);
    }
    private void PlayerInteractAlternate_OnInteractionAlternateEnded(object sender, PlayerInteractAlternate.OnInteractionAlternateEventArgs e)
    {
        ClearAttentionTransform();
    }
    #endregion
}
