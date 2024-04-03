using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHorizontalMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private MovementInput movementInput;
    [SerializeField] private PlayerCrouch playerCrouch;
    [SerializeField] private CheckGround checkGround;
    [SerializeField] private CheckWall checkWall;

    [Header("Speed Settings")]
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float sprintSpeed = 3f;
    [SerializeField] private float crouchSpeed = 1f;

    [Header("Smooth Settings")]
    [SerializeField, Range(1f, 100f)] private float smoothDirectionInputFactor = 5f;
    [SerializeField, Range(1f, 100f)] private float smoothVelocityFactor = 5f;
    [SerializeField, Range(1f, 100f)] private float smoothSprintVelocityFactor = 5f;
    [SerializeField, Range(1f, 100f)] private float smoothFinalDirectionSpeed = 5f;

    private CharacterController characterController;
    private Vector2 DirectionInputVector => movementInput.GetIsometricDirectionVectorNormalized();
    private bool SprintInput => movementInput.GetSprintHold();

    private float desiredSpeed;
    private float smoothCurrentSpeed;

    private Vector2 smoothDirectionInputVector;
    public Vector2 LastNonZeroInput { get; private set; }
    public Vector3 FinalMoveDir { get; private set; }
    public Vector3 SmoothFinalMoveDir { get; private set; }

    public void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void HandleHorizontalMovement(ref Vector3 finalMoveVector)
    {
        CalculateDesiredSpeed();
        SmoothSpeed();

        CalculateLastNonZeroDirectionInput();
        SmoothDirectionInputVector();

        CalculateDesiredMovementDirection();
        SmoothDirectionVector();

        CalculateFinalMovement(ref finalMoveVector);
    }

    private void CalculateDesiredSpeed()
    {
        desiredSpeed = SprintInput && CanRun() ? sprintSpeed : walkSpeed;
        desiredSpeed = playerCrouch.IsCrouching ? crouchSpeed : desiredSpeed;
        desiredSpeed = DirectionInputVector == Vector2.zero ? 0f : desiredSpeed;
        desiredSpeed = checkWall.HitWall ? 0f: desiredSpeed;
    }

    private void SmoothSpeed()
    {
        float smoothFactor = IsRunning() && smoothCurrentSpeed > walkSpeed ? smoothSprintVelocityFactor : smoothVelocityFactor;

        smoothCurrentSpeed = Mathf.Lerp(smoothCurrentSpeed, desiredSpeed, Time.deltaTime * smoothFactor);
    }
        
    private bool CanRun() => !checkWall.HitWall && !playerCrouch.IsCrouching;

    private bool IsRunning() => SprintInput && CanRun();

    private void CalculateLastNonZeroDirectionInput() => LastNonZeroInput = DirectionInputVector != Vector2.zero ? DirectionInputVector : LastNonZeroInput;

    private void SmoothDirectionInputVector() => smoothDirectionInputVector = Vector2.Lerp(smoothDirectionInputVector, DirectionInputVector, Time.deltaTime * smoothDirectionInputFactor);

    private void CalculateDesiredMovementDirection()
    {
        Vector3 moveDirection = GeneralMethods.DirectionToVector3(smoothDirectionInputVector);
        Vector3 flattenDir = FlattenVectorOnSlopes(moveDirection);

        FinalMoveDir = flattenDir;
    }

    private void SmoothDirectionVector() => SmoothFinalMoveDir = Vector3.Slerp(SmoothFinalMoveDir, FinalMoveDir, Time.deltaTime * smoothFinalDirectionSpeed);

    private Vector3 FlattenVectorOnSlopes(Vector3 vectorToFlat)
    {
        if (checkGround.OnSlope) vectorToFlat = Vector3.ProjectOnPlane(vectorToFlat, checkGround.SlopeNormal);

        return vectorToFlat;
    }

    private void CalculateFinalMovement(ref Vector3 finalMoveVector)
    {
        Vector3 finalVector = SmoothFinalMoveDir * smoothCurrentSpeed;

        Vector3 roundedFinalVector;
        roundedFinalVector.x = Math.Abs(finalVector.x) < 0.01f ? 0f : finalVector.x;
        roundedFinalVector.z = Math.Abs(finalVector.z) < 0.01f ? 0f : finalVector.z;

        finalMoveVector.x = roundedFinalVector.x;
        finalMoveVector.z = roundedFinalVector.z;
    }

    public bool HasMovementInput() => DirectionInputVector != Vector2.zero;
}
