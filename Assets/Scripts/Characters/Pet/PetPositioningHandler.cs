using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetPositioningHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PetPlayerAttachment petPlayerAttachment;
    [SerializeField] private Transform orbitPoint;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private PlayerInteractAlternate playerInteractAlternate;

    [Header("Initial Position Settings")]
    [SerializeField] private Vector3 startPositionOffsetFromOrbitPoint;

    [Header("Player Follow Settings")]
    [SerializeField, Range(1f, 5f)] private float orbitRadius;
    [Space]
    [SerializeField, Range(0.1f, 100f)] private float minSmoothPositionSpeedFactor;
    [SerializeField, Range(0.1f, 100f)] private float maxSmoothPositionSpeedFactor;
    [SerializeField, Range(0.1f, 10f)] private float timeToReachMaxSmoothPositionSpeedFactor;
    [Space]
    [SerializeField, Range(0.1f, 100f)] private float smoothVectorSpeedFactor;
    [SerializeField] List<Vector3> preferredDirectionVectors;

    [Header("Safe Positioning Settings")]
    [SerializeField, Range(1f, 2f)] private float safeOrbitRadius;
    [SerializeField] private Vector3 safePositionVector;

    [Header("Guidance Positioning Settings")]
    [SerializeField, Range(0.1f, 100f)] private float minGuidanceSmoothPositionSpeedFactor;
    [SerializeField, Range(0.1f, 100f)] private float maxGuidanceSmoothPositionSpeedFactor;
    [SerializeField, Range(0.1f, 10f)] private float timeToReachMaxGuidanceSmoothPositionSpeedFactor;

    [Header("Player Interaction Settings")]
    [SerializeField] private bool moveTowardsObject;
    [SerializeField] private Vector3 offsetFromInteractableObject;

    [Header("Colision Detection")]
    [SerializeField, Range(0.5f, 1.75f)] private float colisionDetectionRadius;
    [SerializeField] private LayerMask collisionLayers;

    private Vector3 playerFollowDesiredDirectionVector;
    private float playerFollowTargetRadius;

    private Vector3 playerFollowTargetDirectionVector;
    private Vector3 playerFollowDesiredPosition;

    private Transform curentInteractingTransform;

    private float currentSmoothPositionSpeedFactor;
    private float timeFollowing;

    private float currentGuidanceSmoothPositionSpeedFactor;
    private float timeGuiding;

    private void OnEnable()
    {
        PlayerStartPositioning.OnPlayerStartPositionedFirstUpdate += PlayerStartPositioning_OnPlayerStartPositionedFirstUpdate;

        playerInteract.OnInteractionStarted += PlayerInteract_OnInteractionStarted;
        playerInteract.OnInteractionEnded += PlayerInteract_OnInteractionEnded;

        playerInteractAlternate.OnInteractionAlternateStarted += PlayerInteractAlternate_OnInteractionAlternateStarted;
        playerInteractAlternate.OnInteractionAlternateEnded += PlayerInteractAlternate_OnInteractionAlternateEnded;
    }

    private void OnDisable()
    {
        PlayerStartPositioning.OnPlayerStartPositionedFirstUpdate -= PlayerStartPositioning_OnPlayerStartPositionedFirstUpdate;

        playerInteract.OnInteractionStarted -= PlayerInteract_OnInteractionStarted;
        playerInteract.OnInteractionEnded -= PlayerInteract_OnInteractionEnded;

        playerInteractAlternate.OnInteractionAlternateStarted -= PlayerInteractAlternate_OnInteractionAlternateStarted;
        playerInteractAlternate.OnInteractionAlternateEnded -= PlayerInteractAlternate_OnInteractionAlternateEnded;
    }

    private void FixedUpdate()
    {
        HandlePositioningState();
    }

    private void HandlePositioningState()
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
        ResetTimeFollowing();
        ResetTimeGuiding();
    }

    private void FollowingPlayerLogic()
    {
        ResetTimeGuiding();

        timeFollowing += Time.fixedDeltaTime;

        HandlePositionSpeedFactor(ref currentSmoothPositionSpeedFactor, minSmoothPositionSpeedFactor,maxSmoothPositionSpeedFactor, timeFollowing, timeToReachMaxSmoothPositionSpeedFactor);

        if (orbitPoint)
        {
            HandleRegularPositioning();
        }

        if (curentInteractingTransform)
        {
            playerFollowDesiredPosition = curentInteractingTransform.position + offsetFromInteractableObject;
        }

        MoveToPosition(playerFollowDesiredPosition,currentSmoothPositionSpeedFactor);
    }

    private void OnGuidanceLogic()
    {
        ResetTimeFollowing();

        if (PetStateHandler.Instance.CurrentPetGuidanceObject == null) return;

        timeGuiding += Time.fixedDeltaTime;

        HandlePositionSpeedFactor(ref currentGuidanceSmoothPositionSpeedFactor, minGuidanceSmoothPositionSpeedFactor, maxGuidanceSmoothPositionSpeedFactor, timeGuiding, timeToReachMaxGuidanceSmoothPositionSpeedFactor);

        MoveToPosition(PetStateHandler.Instance.CurrentPetGuidanceObject.positionTransform.position,currentGuidanceSmoothPositionSpeedFactor);
    }

    #region General Methods
    private void MoveToPosition(Vector3 position, float smoothFactor) => transform.position = Vector3.Lerp(transform.position, position, smoothFactor * Time.fixedDeltaTime);

    private void HandlePositionSpeedFactor(ref float currentFactor, float initialFactor, float finalFactor, float time, float timeToReachFinalFactor) => currentFactor = Mathf.Lerp(initialFactor, finalFactor, time / timeToReachFinalFactor);

    #endregion

    #region FollowingPlayer Methods

    private void HandleRegularPositioning()
    {
        DefineDesiredDirectionVectorAndRadius();
        CalculateTargetDirectionVector();
        CalculateDesiredPosition();
    }

    private void DefineDesiredDirectionVectorAndRadius()
    {
        Vector3 dirVector = CalculateLocalPreferredPositionVector(safePositionVector, orbitPoint); //.normalized
        float radius = safeOrbitRadius;

        foreach (Vector3 directionVector in preferredDirectionVectors)
        {
            Vector3 localDirectionVector = CalculateLocalPreferredPositionVector(directionVector, orbitPoint);
            Vector3 posiblePosition = CalculateOrbitPosition(orbitPoint, localDirectionVector, orbitRadius);

            bool willCollideSomething = Physics.CheckSphere(posiblePosition, colisionDetectionRadius, collisionLayers);

            if (!willCollideSomething)
            {
                dirVector = localDirectionVector;
                radius = orbitRadius;

                break;
            }
        }

        playerFollowDesiredDirectionVector = dirVector;
        playerFollowTargetRadius = radius;
    }
    private Vector3 CalculateLocalPreferredPositionVector(Vector3 worldPreferredPosition, Transform refferenceTransform)
    {
        if (worldPreferredPosition.magnitude == 0f) return refferenceTransform.up;

        Vector3 preferredVector = refferenceTransform.TransformDirection(worldPreferredPosition); //.normalized

        return preferredVector;
    }

    private Vector3 CalculateOrbitPosition(Transform refferenceTransform, Vector3 directionVector, float orbitRadius) => refferenceTransform.position + orbitRadius * directionVector;

    private void CalculateTargetDirectionVector() => playerFollowTargetDirectionVector = Vector3.Slerp(playerFollowTargetDirectionVector, playerFollowDesiredDirectionVector, smoothVectorSpeedFactor * Time.fixedDeltaTime);

    private void CalculateDesiredPosition() => playerFollowDesiredPosition = CalculateOrbitPosition(orbitPoint, playerFollowTargetDirectionVector, playerFollowTargetRadius);

    private void MoveInstantlyToDesiredPosition() => transform.position = playerFollowDesiredPosition;

    private void ResetTimeFollowing() => timeFollowing = 0f;
    #endregion

    #region Guidance Methods
    private void ResetTimeGuiding() => timeGuiding = 0f;

    #endregion

    #region StartPosition Methods
    private bool CanStartPosition() => PetStateHandler.Instance.PetState == PetStateHandler.State.FollowingPlayer;
    private void StartPositionPet(Vector3 positionVector, float radius)
    {
        if (!CanStartPosition()) return;

        transform.position = orbitPoint.position + startPositionOffsetFromOrbitPoint;
    }

    #endregion

    ///

    #region PlayerStartPositioning Subscriptions
    private void PlayerStartPositioning_OnPlayerStartPositionedFirstUpdate(object sender, System.EventArgs e)
    {
        StartPositionPet(preferredDirectionVectors[0],orbitRadius);
    }
    #endregion

    #region PlayerInteraction Subscriptions
    private void PlayerInteract_OnInteractionStarted(object sender, PlayerInteract.OnInteractionEventArgs e)
    {
        if (!moveTowardsObject) return;

        curentInteractingTransform = e.interactable.GetTransform();

        if (e.interactable.GetTransform().GetComponent<ProjectableObject>()) return;
        if (e.interactable.GetTransform().GetComponent<ProjectionPlatform>()) return;

        curentInteractingTransform = null;
    }

    private void PlayerInteract_OnInteractionEnded(object sender, PlayerInteract.OnInteractionEventArgs e)
    {
        curentInteractingTransform = null;
    }
    #endregion

    #region PlayerInteractionAlternate Subscriptions

    private void PlayerInteractAlternate_OnInteractionAlternateStarted(object sender, PlayerInteractAlternate.OnInteractionAlternateEventArgs e)
    {
        if (!moveTowardsObject) return;

        curentInteractingTransform = e.interactableAlternate.GetTransform();

        if (e.interactableAlternate.GetTransform().GetComponent<ProjectableObject>()) return;
        if (e.interactableAlternate.GetTransform().GetComponent<ProjectionPlatform>()) return;

        curentInteractingTransform = null;
    }
    private void PlayerInteractAlternate_OnInteractionAlternateEnded(object sender, PlayerInteractAlternate.OnInteractionAlternateEventArgs e)
    {
        curentInteractingTransform = null;
    }
    #endregion
}
