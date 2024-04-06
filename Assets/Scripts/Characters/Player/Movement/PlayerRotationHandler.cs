using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotationHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerHorizontalMovement playerHorizontalMovement;
    [SerializeField] private PlayerInteract playerInteract;

    [Header("Rotation Settings")]
    [SerializeField, Range(1f, 100f)] private float smoothRotateFactor = 10f;

    [Header("Starting Rotation Settings")]
    [SerializeField] private bool applyStartingRotation;
    [SerializeField] private Vector3 startingFacingDirection;

    [Header("Hold Settings")]
    [SerializeField, Range (0f, 0.1f)] private float holdDirectionThresholdTime;

    private Vector2 DirectionInput => playerHorizontalMovement.FixedLastNonZeroInput;
    private Vector3 desiredFacingDirection;
    public Vector3 FacingDirection { get; private set; }

    private Vector2 previousDirectionInput;
    private float directionHoldingTimer = 0f;

    private bool respondToMovement;

    private void OnEnable()
    {
        playerInteract.OnInteractionStarted += PlayerInteract_OnInteractionStarted;
        playerInteract.OnInteractionEnded += PlayerInteract_OnInteractionEnded;    
    }

    private void OnDisable()
    {
        playerInteract.OnInteractionStarted -= PlayerInteract_OnInteractionStarted;
        playerInteract.OnInteractionEnded -= PlayerInteract_OnInteractionEnded;
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void Update()
    {
        HandleHoldDirection();

        DefineDesiredFacingDirection();
        HandleRotation();

        AvoidXZRotation();
    }

    private void InitializeVariables()
    {
        respondToMovement = true;
        previousDirectionInput = DirectionInput;

        FacingDirection = playerHorizontalMovement.transform.forward;
        if (applyStartingRotation) FacingDirection = startingFacingDirection;

        FacingDirection.Normalize();

        transform.localRotation = Quaternion.LookRotation(FacingDirection);
    }

    private void HandleHoldDirection()
    {
        if(previousDirectionInput == DirectionInput && playerHorizontalMovement.HasMovementInput())
        {
            directionHoldingTimer += Time.deltaTime;
        }
        else
        {
            directionHoldingTimer = 0f;
        }

        previousDirectionInput = DirectionInput;
    }

    private void DefineDesiredFacingDirection()
    {
        if (CanChangeDirectionDueToMovement() && respondToMovement) desiredFacingDirection = GeneralMethods.Vector2ToVector3(DirectionInput);
    }
    private bool CanChangeDirectionDueToMovement() => directionHoldingTimer >= holdDirectionThresholdTime;

    private void HandleRotation()
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

    private void AvoidXZRotation() => transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);

    #region PlayerInteractionSubscriptions
    private void PlayerInteract_OnInteractionStarted(object sender, PlayerInteract.OnInteractionEventArgs e)
    {
        Vector3 interactablePosition = e.interactable.GetTransform().position;
        Vector3 facingVectorRaw = (interactablePosition - transform.position).normalized;

        desiredFacingDirection = GeneralMethods.SupressYComponent(facingVectorRaw);
        respondToMovement = false;
    }

    private void PlayerInteract_OnInteractionEnded(object sender, PlayerInteract.OnInteractionEventArgs e)
    {
        respondToMovement = true;
    }

    #endregion
}
