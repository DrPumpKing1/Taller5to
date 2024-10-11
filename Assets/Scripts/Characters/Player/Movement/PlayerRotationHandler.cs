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
    [Space]
    [SerializeField] private Transform petFacePoint;

    [Header("Rotation Settings")]
    [SerializeField, Range(1f, 100f)] private float smoothRotateFactor = 10f;

    [Header("Hold Settings")]
    [SerializeField, Range (0f, 0.1f)] private float holdDirectionThresholdTime;

    private Vector2 DirectionInput => playerHorizontalMovement.FixedLastNonZeroInput;
    public Vector3 DesiredFacingDirection { get; private set; }
    public Vector3 FacingDirection { get; private set; }

    private Vector2 previousDirectionInput;
    private float directionHoldingTimer = 0f;

    private Transform currentInteractionAttentionTransform;
    private bool interactingRotate;

    private bool lookAtPetDialogue;

    private void OnEnable()
    {
        PlayerDirectionHandler.OnPlayerStartDirectioned += PlayerStartDirection_OnPlayerStartDirectioned;
        PlayerDirectionHandler.OnPlayerInstantDirectioned += PlayerDirectionHandler_OnPlayerInstantDirectioned;

        playerInteract.OnInteractionStarted += PlayerInteract_OnInteractionStarted;
        playerInteract.OnInteractionEnded += PlayerInteract_OnInteractionEnded;

        playerInteractAlternate.OnInteractionAlternateStarted += PlayerInteractAlternate_OnInteractionAlternateStarted;
        playerInteractAlternate.OnInteractionAlternateEnded += PlayerInteractAlternate_OnInteractionAlternateEnded;

        DialogueManager.OnDialogueStart += DialogueManager_OnDialogueStart;
        DialogueManager.OnDialogueEnd += DialogueManager_OnDialogueEnd;
    }

    private void OnDisable()
    {
        PlayerDirectionHandler.OnPlayerStartDirectioned -= PlayerStartDirection_OnPlayerStartDirectioned;
        PlayerDirectionHandler.OnPlayerInstantDirectioned -= PlayerDirectionHandler_OnPlayerInstantDirectioned;

        playerInteract.OnInteractionStarted -= PlayerInteract_OnInteractionStarted;
        playerInteract.OnInteractionEnded -= PlayerInteract_OnInteractionEnded;

        playerInteractAlternate.OnInteractionAlternateStarted -= PlayerInteractAlternate_OnInteractionAlternateStarted;
        playerInteractAlternate.OnInteractionAlternateEnded -= PlayerInteractAlternate_OnInteractionAlternateEnded;

        DialogueManager.OnDialogueStart -= DialogueManager_OnDialogueStart;
        DialogueManager.OnDialogueEnd -= DialogueManager_OnDialogueEnd;
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
        if (currentInteractionAttentionTransform)
        {
            SetDesiredFacingDirectionTowardsTransform(currentInteractionAttentionTransform);
            if (!interactingRotate) ClearInteractionAttentionTransform();
            return;
        }

        if (lookAtPetDialogue)
        {
            SetDesiredFacingDirectionTowardsTransform(petFacePoint);
            return;
        }

        if (CanChangeDirectionDueToMovement())
        {
            SetDesiredFacingDirectionTowardsDirection(DirectionInput);
        }
    }

    public bool CanChangeDirectionDueToMovement() => directionHoldingTimer >= holdDirectionThresholdTime;

    private void HandleRotation()
    {
        if (!playerHorizontalMovement.MovementEnabled) return;
        if (DesiredFacingDirection.magnitude <= 0f) return;

        RotateTowardsDirection(DesiredFacingDirection);
    }

    #region General Methods

    private void SetDesiredFacingDirectionTowardsDirection(Vector3 direction) => DesiredFacingDirection = GeneralMethods.Vector2ToVector3(direction);
    private void SetDesiredFacingDirectionTowardsTransform(Transform lookTransform)
    {
        Vector3 facingVectorRaw = (lookTransform.position - transform.position).normalized;
        DesiredFacingDirection = GeneralMethods.SupressYComponent(facingVectorRaw);
    }

    private void RotateTowardsDirection(Vector3 direction)
    {
        FacingDirection = Vector3.Slerp(FacingDirection, direction, smoothRotateFactor * Time.deltaTime);
        FacingDirection.Normalize();

        transform.localRotation = Quaternion.LookRotation(FacingDirection);
    }

    private void RotateInstantlyTowardsDirection(Vector3 direction)
    {
        direction.Normalize();
        transform.localRotation = Quaternion.LookRotation(direction);
    }

    private void SetFacingDirection(Vector3 direction) => FacingDirection = direction;
    private void AvoidXZRotation() => transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);

    #endregion

    #region InteractionSettings
    private void SetInteractionAttentionTransform(Transform transform) => currentInteractionAttentionTransform = transform;
    private void ClearInteractionAttentionTransform() => currentInteractionAttentionTransform = null;
    #endregion

    #region PlayerDirectionHandler Subscriptions
    private void PlayerStartDirection_OnPlayerStartDirectioned(object sender, PlayerDirectionHandler.OnPlayerDirectionedEventArgs e)
    {
        SetFacingDirection(e.facingDirection);
    }
    private void PlayerDirectionHandler_OnPlayerInstantDirectioned(object sender, PlayerDirectionHandler.OnPlayerDirectionedEventArgs e)
    {
        RotateInstantlyTowardsDirection(e.facingDirection);
    }

    #endregion
    ///

    #region PlayerInteractionSubscriptions
    private void PlayerInteract_OnInteractionStarted(object sender, PlayerInteract.OnInteractionEventArgs e)
    {
        if (!e.interactable.GrabPlayerAttention) return;
        if (e.interactable.GetInteractionAttentionTransform() == null) return;

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
        if (!e.interactableAlternate.GrabPlayerAttention) return;
        if (e.interactableAlternate.GetInteractionAlternateAttentionTransform() == null) return;

        interactingRotate = true;
        SetInteractionAttentionTransform(e.interactableAlternate.GetInteractionAlternateAttentionTransform());
    }
    private void PlayerInteractAlternate_OnInteractionAlternateEnded(object sender, PlayerInteractAlternate.OnInteractionAlternateEventArgs e)
    {
        interactingRotate = false;
    }
    #endregion

    #region DialogueManager Subscriptions
    private void DialogueManager_OnDialogueStart(object sender, DialogueManager.OnDialogueEventArgs e)
    {
        if (e.dialogueSO.lookAtPet) lookAtPetDialogue = true;
        else lookAtPetDialogue = false;
    }

    private void DialogueManager_OnDialogueEnd(object sender, DialogueManager.OnDialogueEventArgs e)
    {
        lookAtPetDialogue = false;
    }

    #endregion

}
