using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetPositioningHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform orbitPoint;

    [Header("Orbit Positioning Settings")]
    [SerializeField, Range(1f, 5f)] private float orbitRadius;
    [SerializeField, Range(1f, 100f)] private float smoothPositionSpeedFactor;
    [SerializeField, Range(1f, 100f)] private float smoothVectorSpeedFactor;
    [SerializeField] List<Vector3> preferredDirectionVectors;

    [Header("Safe Positioning Settings")]
    [SerializeField, Range(1f, 2f)] private float safeOrbitRadius;
    [SerializeField] private Vector3 safePositionVector;

    [Header("Colision Detection")]
    [SerializeField, Range(0.5f, 1.75f)] private float colisionDetectionRadius;
    [SerializeField] private LayerMask collisionLayers;

    private Vector3 desiredDirectionVector;
    private float targetRadius;

    private Vector3 targetDirectionVector;
    private Vector3 desiredPosition;
    private void Update()
    {
        HandlePositioning();
    }

    private void HandlePositioning()
    {
        if (!orbitPoint) return;

        DefineDesiredDirectionVectorAndRadius();
        CalculateTargetDirectionVector();
        CalculateTargetPosition();
        MoveToTargetPosition();
    }

    private void DefineDesiredDirectionVectorAndRadius()
    {
        Vector3 dirVector = safePositionVector; //.normalized
        float radius = safeOrbitRadius;

        foreach (Vector3 directionVector in preferredDirectionVectors)
        {
            Vector3 localDirectionVector = CalculateLocalPreferredPositionVector(directionVector, orbitPoint);
            Vector3 posiblePosition = CalculateOrbitPosition(orbitPoint, localDirectionVector, orbitRadius);

            bool willCollideSomething = Physics.CheckSphere(posiblePosition, colisionDetectionRadius);

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

    private void MoveToTargetPosition() => transform.position = Vector3.Slerp(transform.position, desiredPosition, smoothPositionSpeedFactor * Time.deltaTime);

}
