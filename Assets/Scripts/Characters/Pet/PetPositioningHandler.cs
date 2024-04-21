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

    [Header("Orbit Positioning Settings")]
    [SerializeField, Range(1f, 5f)] private float orbitRadius;
    [SerializeField, Range(0.1f, 100f)] private float smoothPositionSpeedFactor;
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

    private Vector3 desiredDirectionVector;
    private float targetRadius;

    private Vector3 targetDirectionVector;
    private Vector3 desiredPosition;

    private bool AttachToPlayer => petPlayerAttachment.AttachToPlayer;

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

    private void Update()
    {
        HandlePositioning();
    }

    private void HandlePositioning()
    {
        if (!AttachToPlayer) return;

        if (orbitPoint)
        {
            HandleRegularPositioning();
        }

        if (curentInteractingTransform)
        {
            desiredPosition = curentInteractingTransform.position + offsetFromInteractableObject;
        }

        MoveToTargetPosition();
    }

    private void HandleRegularPositioning()
    {
        DefineDesiredDirectionVectorAndRadius();
        CalculateTargetDirectionVector();
        CalculateTargetPosition();
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

    private Vector3 CalculateOrbitPosition(Transform refferenceTransform, Vector3 directionVector, float orbitRadius) => refferenceTransform.position + orbitRadius * directionVector;

    private Vector3 CalculateLocalPreferredPositionVector(Vector3 worldPreferredPosition, Transform refferenceTransform)
    {
        if (worldPreferredPosition.magnitude == 0f) return refferenceTransform.up;

        Vector3 preferredVector = refferenceTransform.TransformDirection(worldPreferredPosition); //.normalized

        return preferredVector;
    }

    private void CalculateTargetDirectionVector() => targetDirectionVector = Vector3.Slerp(targetDirectionVector, desiredDirectionVector, smoothVectorSpeedFactor * Time.deltaTime);

    private void CalculateTargetPosition() => desiredPosition = CalculateOrbitPosition(orbitPoint, targetDirectionVector, targetRadius);

    private void MoveToTargetPosition() => transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothPositionSpeedFactor * Time.deltaTime);


    #region PlayerInteractionSubscriptions
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

    #region PlayerInteractionAlternateSubscriptions

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
