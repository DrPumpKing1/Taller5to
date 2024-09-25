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

    [Header("Orbit Positioning Settings")]
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

    [Header("Player Interaction Settings")]
    [SerializeField] private bool moveTowardsObject;
    [SerializeField] private Vector3 offsetFromInteractableObject;

    [Header("Colision Detection")]
    [SerializeField, Range(0.5f, 1.75f)] private float colisionDetectionRadius;
    [SerializeField] private LayerMask collisionLayers;

    [Header("States")]
    [SerializeField] private State state;

    public enum State { Still, FollowingPlayer, OnGuidance }

    public State PositioningState => state;

    private Vector3 desiredDirectionVector;
    private float targetRadius;

    private Vector3 targetDirectionVector;
    private Vector3 desiredPosition;

    private Transform curentInteractingTransform;

    public float currentSmoothPositionSpeedFactor;

    private float timeFollowing;

    private void OnEnable()
    {
        PetPlayerAttachment.OnVyrxAttachToPlayer += PetPlayerAttachment_OnVyrxAttachToPlayer;
        PetPlayerAttachment.OnVyrxUnattachToPlayer += PetPlayerAttachment_OnVyrxUnattachToPlayer;



        PlayerStartPositioning.OnPlayerStartPositioned += PlayerStartPositioning_OnPlayerStartPositioned;

        playerInteract.OnInteractionStarted += PlayerInteract_OnInteractionStarted;
        playerInteract.OnInteractionEnded += PlayerInteract_OnInteractionEnded;

        playerInteractAlternate.OnInteractionAlternateStarted += PlayerInteractAlternate_OnInteractionAlternateStarted;
        playerInteractAlternate.OnInteractionAlternateEnded += PlayerInteractAlternate_OnInteractionAlternateEnded;
    }

    private void OnDisable()
    {
        PetPlayerAttachment.OnVyrxAttachToPlayer -= PetPlayerAttachment_OnVyrxAttachToPlayer;
        PetPlayerAttachment.OnVyrxUnattachToPlayer -= PetPlayerAttachment_OnVyrxUnattachToPlayer;

        PlayerStartPositioning.OnPlayerStartPositioned -= PlayerStartPositioning_OnPlayerStartPositioned;

        playerInteract.OnInteractionStarted -= PlayerInteract_OnInteractionStarted;
        playerInteract.OnInteractionEnded -= PlayerInteract_OnInteractionEnded;

        playerInteractAlternate.OnInteractionAlternateStarted -= PlayerInteractAlternate_OnInteractionAlternateStarted;
        playerInteractAlternate.OnInteractionAlternateEnded -= PlayerInteractAlternate_OnInteractionAlternateEnded;
    }

    private void FixedUpdate()
    {
        HandlePositioningState();
    }

    private void SetPositioningState(State state) => this.state = state;

    private void HandlePositioningState()
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
        ResetTimeFollowing();
    }

    private void FollowingPlayerLogic()
    {
        timeFollowing += Time.fixedDeltaTime;

        HandlePositionSpeedFactor(minSmoothPositionSpeedFactor,maxSmoothPositionSpeedFactor,timeToReachMaxSmoothPositionSpeedFactor);

        if (orbitPoint)
        {
            HandleRegularPositioning();
        }

        if (curentInteractingTransform)
        {
            desiredPosition = curentInteractingTransform.position + offsetFromInteractableObject;
        }

        MoveToDesiredPosition();
    }

    private void OnGuidanceLogic()
    {
        ResetTimeFollowing();
    }

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

        desiredDirectionVector = dirVector;
        targetRadius = radius;
    }
    private Vector3 CalculateLocalPreferredPositionVector(Vector3 worldPreferredPosition, Transform refferenceTransform)
    {
        if (worldPreferredPosition.magnitude == 0f) return refferenceTransform.up;

        Vector3 preferredVector = refferenceTransform.TransformDirection(worldPreferredPosition); //.normalized

        return preferredVector;
    }

    private Vector3 CalculateOrbitPosition(Transform refferenceTransform, Vector3 directionVector, float orbitRadius) => refferenceTransform.position + orbitRadius * directionVector;

    private void CalculateTargetDirectionVector() => targetDirectionVector = Vector3.Slerp(targetDirectionVector, desiredDirectionVector, smoothVectorSpeedFactor * Time.fixedDeltaTime);

    private void CalculateDesiredPosition() => desiredPosition = CalculateOrbitPosition(orbitPoint, targetDirectionVector, targetRadius);

    private void MoveToDesiredPosition() => transform.position = Vector3.Lerp(transform.position, desiredPosition, currentSmoothPositionSpeedFactor * Time.fixedDeltaTime);
    private void MoveInstantlyToDesiredPosition() => transform.position = desiredPosition;

    private void HandlePositionSpeedFactor(float initialFactor, float finalFactor, float timeToReachFinalFactor) => currentSmoothPositionSpeedFactor = Mathf.Lerp(initialFactor, finalFactor, timeFollowing/timeToReachFinalFactor);

    private void ResetTimeFollowing() => timeFollowing = 0f;
    #endregion

    #region StartPosition Methods
    private void StartPositionPet(Vector3 positionVector, float radius)
    {
        if (state != State.FollowingPlayer) return;

        transform.position = orbitPoint.position + startPositionOffsetFromOrbitPoint;
    }
    #endregion



    #region PetPlayerAttachment Subscriptions
    private void PetPlayerAttachment_OnVyrxAttachToPlayer(object sender, EventArgs e)
    {
        SetPositioningState(State.FollowingPlayer);
    }

    private void PetPlayerAttachment_OnVyrxUnattachToPlayer(object sender, EventArgs e)
    {
        SetPositioningState(State.Still);
    }

    #endregion

    #region PlayerStartPositioning Subscriptions
    private void PlayerStartPositioning_OnPlayerStartPositioned(object sender, System.EventArgs e)
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
