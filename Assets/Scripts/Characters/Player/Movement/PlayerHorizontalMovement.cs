using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHorizontalMovement : MonoBehaviour
{
    [Header("Enabler")]
    [SerializeField] private bool movementEnabled;

    [Header("Components")]
    [SerializeField] private MovementInput movementInput;
    [Space]
    [SerializeField] private PlayerLand playerLand;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private PlayerInteractAlternate playerInteractAlternate;
    [Space]
    [SerializeField] private CheckGround checkGround;
    [SerializeField] private CheckWall checkWall;

    [Header("Speed Settings")]
    [SerializeField, Range(1.5f,10f)] private float moveSpeed = 2f;
    [Space]
    [SerializeField] private bool flattenSpeedOnSlopes;

    [Header("Smooth Settings")]
    [SerializeField, Range(1f, 100f)] private float smoothInputFactor = 5f;
    [SerializeField, Range(1f, 100f)] private float smoothVelocityFactor = 5f;
    [SerializeField, Range(1f, 100f)] private float smoothDirectionFactor = 5f;

    private Rigidbody _rigidbody;

    public Vector2 DirectionInputVector => movementInput.GetIsometricDirectionVectorNormalized();

    private float desiredSpeed;
    private float smoothCurrentSpeed;

    private Vector2 smoothDirectionInputVector;
    public Vector2 LastNonZeroInput { get; private set; }
    public Vector2 FixedLastNonZeroInput { get; private set; }
    public Vector3 FinalMoveDir { get; private set; }
    public Vector3 SmoothFinalMoveDir { get; private set; }
    public Vector3 FinalMoveVector { get; private set; }
    public bool MovementEnabled { get { return movementEnabled; } }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void FixedUpdate()
    {
        ApplyHorizontalMovement();
    }

    private void HandleMovement()
    {
        if (!movementEnabled) return;

        CalculateDesiredSpeed();
        SmoothSpeed();

        CalculateLastNonZeroDirectionInput();
        FixDirectionVectorDueToWalls();
        SmoothDirectionInputVector();

        CalculateDesiredMovementDirection();
        SmoothDirectionVector();

        CalculateFinalMovement();
    }

    private void CalculateDesiredSpeed()
    {
        desiredSpeed = CanMove() ? moveSpeed : 0f;
    }

    private bool CanMove()
    {
        if (DirectionInputVector == Vector2.zero) return false;
        if (checkWall.HitWall) return false;
        if (playerLand.IsRecoveringFromLanding) return false;
        if (playerInteract.IsInteracting) return false;
        if (playerInteractAlternate.IsInteractingAlternate) return false;

        return true;
    }

    private void SmoothSpeed()
    {
        smoothCurrentSpeed = Mathf.Lerp(smoothCurrentSpeed, desiredSpeed, Time.deltaTime * smoothVelocityFactor);
    }
    
    private void CalculateLastNonZeroDirectionInput() => LastNonZeroInput = DirectionInputVector != Vector2.zero ? DirectionInputVector : LastNonZeroInput;

    private void FixDirectionVectorDueToWalls()
    {
        /*
        if (checkWall.HitDiagonalWall)
        {
            Vector3 wallNormal = checkWall.GetDiagonalWallInfo().normal;

            Vector3 vector3LastNonZeroInput = GeneralMethods.Vector2ToVector3(LastNonZeroInput);
            Vector3 proyection = Vector3.Project(vector3LastNonZeroInput, wallNormal);
            Vector3 perpendicularProyection = vector3LastNonZeroInput - proyection;

            Vector2 vector2PerpendicularProyection = GeneralMethods.Vector3ToVector2(perpendicularProyection);

            FixedLastNonZeroInput = vector2PerpendicularProyection.normalized;

            return;
        }
        */

        FixedLastNonZeroInput = LastNonZeroInput;
    }

    private void SmoothDirectionInputVector() => smoothDirectionInputVector = Vector2.Lerp(smoothDirectionInputVector, FixedLastNonZeroInput, Time.deltaTime * smoothInputFactor);

    private void CalculateDesiredMovementDirection()
    {
        Vector3 moveDirection = GeneralMethods.Vector2ToVector3(smoothDirectionInputVector);
        Vector3 flattenDir = FlattenVectorOnSlopes(moveDirection);

        FinalMoveDir = flattenDir;
    }

    private void SmoothDirectionVector() => SmoothFinalMoveDir = Vector3.Slerp(SmoothFinalMoveDir, FinalMoveDir, Time.deltaTime * smoothDirectionFactor);

    private Vector3 FlattenVectorOnSlopes(Vector3 vectorToFlat)
    {
        if (checkGround.OnSlope && flattenSpeedOnSlopes) vectorToFlat = Vector3.ProjectOnPlane(vectorToFlat, checkGround.SlopeNormal);
        return vectorToFlat;
    }

    private void CalculateFinalMovement()
    {
        Vector3 finalVector = SmoothFinalMoveDir * smoothCurrentSpeed;

        Vector3 roundedFinalVector;
        roundedFinalVector.x = Math.Abs(finalVector.x) < 0.01f ? 0f : finalVector.x;
        roundedFinalVector.z = Math.Abs(finalVector.z) < 0.01f ? 0f : finalVector.z;

        FinalMoveVector = new Vector3(roundedFinalVector.x, 0f, roundedFinalVector.z);
    }

    private void ApplyHorizontalMovement()
    {
        _rigidbody.velocity = new Vector3(FinalMoveVector.x, _rigidbody.velocity.y, FinalMoveVector.z);
    }

    public bool HasMovementInput() => DirectionInputVector != Vector2.zero;
}
