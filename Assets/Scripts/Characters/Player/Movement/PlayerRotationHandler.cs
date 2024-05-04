using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotationHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerHorizontalMovement playerHorizontalMovement;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private PlayerInteractAlternate playerInteractAlternate;
    [SerializeField] private CheckGround checkGround;

    [Header("Rotation Settings")]
    [SerializeField, Range(1f, 100f)] private float smoothRotateFactor = 10f;

    [Header("Starting Rotation Settings")]
    [SerializeField] private bool applyStartingRotation;
    [SerializeField] private Vector2 startingFacingDirection;

    [Header("Hold Settings")]
    [SerializeField, Range (0f, 0.1f)] private float holdDirectionThresholdTime;

    private Vector2 DirectionInput => playerHorizontalMovement.FixedLastNonZeroInput;
    public Vector3 DesiredFacingDirection { get; private set; }
    public Vector3 FacingDirection { get; private set; }

    private Vector2 previousDirectionInput;
    private float directionHoldingTimer = 0f;

    private bool respondToMovement;

    private Transform curentInteractingTransform;

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

        if (applyStartingRotation)
        {
            FacingDirection = GeneralMethods.Vector2ToVector3(startingFacingDirection);
            FacingDirection = FacingDirection.magnitude == 0 ? playerHorizontalMovement.transform.forward : FacingDirection;
        }

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
        if (CanChangeDirectionDueToMovement())
        {
            DesiredFacingDirection = GeneralMethods.Vector2ToVector3(DirectionInput);
        }

        if (curentInteractingTransform)
        {
            Vector3 facingVectorRaw = (curentInteractingTransform.position - transform.position).normalized;
            DesiredFacingDirection = GeneralMethods.SupressYComponent(facingVectorRaw);
        }
    }

    private bool CanChangeDirectionDueToMovement() => directionHoldingTimer >= holdDirectionThresholdTime;

    private void HandleRotation()
    {
        if (!playerHorizontalMovement.MovementEnabled) return;
        if (DesiredFacingDirection.magnitude <= 0f) return;

        RotateTowardsDirection(DesiredFacingDirection);
    }

    private void RotateTowardsDirection(Vector3 direction)
    {
        //if (!checkGround.IsGrounded) return;

        FacingDirection = Vector3.Slerp(FacingDirection, direction, smoothRotateFactor * Time.deltaTime);
        FacingDirection.Normalize();

        transform.localRotation = Quaternion.LookRotation(FacingDirection);
    }

    private void AvoidXZRotation() => transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);

    #region PlayerInteractionSubscriptions
    private void PlayerInteract_OnInteractionStarted(object sender, PlayerInteract.OnInteractionEventArgs e)
    {
        if (!e.interactable.GrabPlayerAttention) return;
        curentInteractingTransform = e.interactable.GetTransform();
    }

    private void PlayerInteract_OnInteractionEnded(object sender, PlayerInteract.OnInteractionEventArgs e)
    {
        curentInteractingTransform = null;
    }
    #endregion

    #region PlayerInteractionAlternateSubscriptions
    private void PlayerInteractAlternate_OnInteractionAlternateStarted(object sender, PlayerInteractAlternate.OnInteractionAlternateEventArgs e)
    {
        if (!e.interactableAlternate.GrabPlayerAttention) return;
        curentInteractingTransform = e.interactableAlternate.GetTransform();
    }
    private void PlayerInteractAlternate_OnInteractionAlternateEnded(object sender, PlayerInteractAlternate.OnInteractionAlternateEventArgs e)
    {
        curentInteractingTransform = null;
    }
    #endregion

}
