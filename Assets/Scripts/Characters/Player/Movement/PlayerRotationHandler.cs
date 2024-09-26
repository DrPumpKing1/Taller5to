using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotationHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerStartDirection playerStartDirection;
    [SerializeField] private PlayerHorizontalMovement playerHorizontalMovement;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private PlayerInteractAlternate playerInteractAlternate;
    [SerializeField] private CheckGround checkGround;

    [Header("Rotation Settings")]
    [SerializeField, Range(1f, 100f)] private float smoothRotateFactor = 10f;

    [Header("Hold Settings")]
    [SerializeField, Range (0f, 0.1f)] private float holdDirectionThresholdTime;

    private Vector2 DirectionInput => playerHorizontalMovement.FixedLastNonZeroInput;
    public Vector3 DesiredFacingDirection { get; private set; }
    public Vector3 FacingDirection { get; private set; }

    private Vector2 previousDirectionInput;
    private float directionHoldingTimer = 0f;

    private Transform currentAttentionTransform;
    private bool interactingRotate;

    private void OnEnable()
    {
        playerStartDirection.OnPlayerStartDirectioned += PlayerStartDirection_OnPlayerStartDirectioned;

        playerInteract.OnInteractionStarted += PlayerInteract_OnInteractionStarted;
        playerInteract.OnInteractionEnded += PlayerInteract_OnInteractionEnded;

        playerInteractAlternate.OnInteractionAlternateStarted += PlayerInteractAlternate_OnInteractionAlternateStarted;
        playerInteractAlternate.OnInteractionAlternateEnded += PlayerInteractAlternate_OnInteractionAlternateEnded;   
    }

    private void OnDisable()
    {
        playerStartDirection.OnPlayerStartDirectioned -= PlayerStartDirection_OnPlayerStartDirectioned;

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
        interactingRotate = false;
        previousDirectionInput = DirectionInput;
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
        if (currentAttentionTransform)
        {
            Vector3 facingVectorRaw = (currentAttentionTransform.position - transform.position).normalized;
            DesiredFacingDirection = GeneralMethods.SupressYComponent(facingVectorRaw);

            if (!interactingRotate) currentAttentionTransform = null;
            return;
        }

        if (CanChangeDirectionDueToMovement())
        {
            DesiredFacingDirection = GeneralMethods.Vector2ToVector3(DirectionInput);
        }
    }

    public bool CanChangeDirectionDueToMovement() => directionHoldingTimer >= holdDirectionThresholdTime;

    private void HandleRotation()
    {
        if (!playerHorizontalMovement.MovementEnabled) return;
        if (DesiredFacingDirection.magnitude <= 0f) return;

        RotateTowardsDirection(DesiredFacingDirection);
    }

    private void RotateTowardsDirection(Vector3 direction)
    {
        FacingDirection = Vector3.Slerp(FacingDirection, direction, smoothRotateFactor * Time.deltaTime);
        FacingDirection.Normalize();

        transform.localRotation = Quaternion.LookRotation(FacingDirection);
    }

    private void AvoidXZRotation() => transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);

    #region PlayerStartDirectioningSettings
    private void PlayerStartDirection_OnPlayerStartDirectioned(object sender, PlayerStartDirection.OnPlayerStartDirectionedEventArgs e)
    {
        FacingDirection = e.startingFacingDirection;
    }
    #endregion

    #region PlayerInteractionSubscriptions
    private void PlayerInteract_OnInteractionStarted(object sender, PlayerInteract.OnInteractionEventArgs e)
    {
        if (!e.interactable.GrabPlayerAttention) return;

        currentAttentionTransform = e.interactable.GetInteractionAttentionTransform();
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
        if (!e.interactableAlternate.GrabPlayerAttention) return;

        currentAttentionTransform = e.interactableAlternate.GetInteractionAlternateAttentionTransform();
        interactingRotate = true;
    }
    private void PlayerInteractAlternate_OnInteractionAlternateEnded(object sender, PlayerInteractAlternate.OnInteractionAlternateEventArgs e)
    {
        interactingRotate = false;
    }
    #endregion

}
