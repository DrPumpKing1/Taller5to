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

    private bool AttachToPlayer => petPlayerAttachment.AttachToPlayer;

    private Vector3 PlayerFacingDirection => playerRotationHandler.FacingDirection;
    public Vector3 FacingDirection { get; private set; }

    private Vector3 desiredFacingDirection;

    private Transform currentAttentionTransform;
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
    }

    private void Update()
    {
        HandleRotation();
    }

    private void HandleRotation()
    {
        if (!AttachToPlayer) return;

        DefineDesiredFacingDirection();
        ApplyRotation();
    }

    private void InitializeVariables()
    {
        interactingRotate = false;
        FacingDirection = transform.forward;

        if (applyStartingRotation) FacingDirection = startingFacingDirection;

        FacingDirection.Normalize();

        transform.localRotation = Quaternion.LookRotation(FacingDirection);
    }

    private void DefineDesiredFacingDirection()
    {
        if (currentAttentionTransform)
        {
            desiredFacingDirection = (currentAttentionTransform.position - transform.position).normalized;

            if(!interactingRotate) currentAttentionTransform = null;
            return;
        }

        if (playerRotationHandler) 
        {
            desiredFacingDirection = PlayerFacingDirection;
        }
    }

    private void ApplyRotation()
    {
        if (desiredFacingDirection.magnitude <= 0f) return;

        RotateTowardsDirection(desiredFacingDirection);
    }

    private void RotateTowardsDirection(Vector3 direction)
    {
        FacingDirection = Vector3.Slerp(FacingDirection, direction, smoothRotateFactor * Time.deltaTime);
        FacingDirection.Normalize();

        transform.localRotation = Quaternion.LookRotation(FacingDirection);
    }

    #region PlayerInteractionSubscriptions
    private void PlayerInteract_OnInteractionStarted(object sender, PlayerInteract.OnInteractionEventArgs e)
    {
        if (!e.interactable.GrabPetAttention) return;

        currentAttentionTransform = e.interactable.AttentionTransform;
        interactingRotate = true;
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

        currentAttentionTransform = e.interactableAlternate.AttentionTransform;
        interactingRotate = true;
    }
    private void PlayerInteractAlternate_OnInteractionAlternateEnded(object sender, PlayerInteractAlternate.OnInteractionAlternateEventArgs e)
    {
        interactingRotate = false;
    }
    #endregion
}
