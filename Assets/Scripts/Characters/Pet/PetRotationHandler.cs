using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRotationHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerRotationHandler playerRotationHandler;
    [SerializeField] private PlayerInteract playerInteract;

    [Header("Rotation Settings")]
    [SerializeField, Range(1f, 100f)] private float smoothRotateFactor = 5f;

    [Header("Starting Rotation Settings")]
    [SerializeField] private bool applyStartingRotation;
    [SerializeField] private Vector3 startingFacingDirection;

    private Vector3 PlayerFacingDirection => playerRotationHandler.FacingDirection;
    public Vector3 FacingDirection { get; private set; }

    private Vector3 desiredFacingDirection;
    private bool respondToPlayerFacingDirection;

    private void OnEnable()
    {
        if (!playerInteract) return;

        playerInteract.OnInteractionStarted += PlayerInteract_OnInteractionStarted;
        playerInteract.OnInteractionEnded += PlayerInteract_OnInteractionEnded;
    }

    private void OnDisable()
    {
        if (!playerInteract) return;

        playerInteract.OnInteractionStarted -= PlayerInteract_OnInteractionStarted;
        playerInteract.OnInteractionEnded -= PlayerInteract_OnInteractionEnded;
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void Update()
    {
        DefineDesiredFacingDirection();
        HandleRotation();
    }

    private void InitializeVariables()
    {
        respondToPlayerFacingDirection = true;

        FacingDirection = transform.forward;
        if (applyStartingRotation) FacingDirection = startingFacingDirection;

        FacingDirection.Normalize();

        transform.localRotation = Quaternion.LookRotation(FacingDirection);
    }

    private void DefineDesiredFacingDirection()
    {
        if (respondToPlayerFacingDirection && playerRotationHandler) desiredFacingDirection = PlayerFacingDirection;
    }

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

    #region PlayerInteractionSubscriptions
    private void PlayerInteract_OnInteractionStarted(object sender, PlayerInteract.OnInteractionEventArgs e)
    {
        Vector3 interactablePosition = e.interactable.GetTransform().position;
        Vector3 facingVectorRaw = (interactablePosition - transform.position).normalized;

        desiredFacingDirection = facingVectorRaw;
        respondToPlayerFacingDirection = false;
    }

    private void PlayerInteract_OnInteractionEnded(object sender, PlayerInteract.OnInteractionEventArgs e)
    {
        respondToPlayerFacingDirection = true;
    }
    #endregion
}
