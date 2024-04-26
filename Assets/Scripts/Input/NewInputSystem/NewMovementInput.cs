using System;
using UnityEngine;

public class NewMovementInput : MovementInput
{
    [Header("Components")]
    [SerializeField] private Transform cameraTransform;

    private PlayerInputActions playerInputActions;

    protected override void Awake()
    {
        base.Awake();
        InitializePlayerInputActions();
    }

    private void InitializePlayerInputActions()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Movement.Enable();
    }

    public override bool CanProcessMovementInput() => GameManager.Instance.GameState == GameManager.State.OnGameplay;

    public override Vector2 GetDirectionVectorNormalized()
    {
        if (!CanProcessMovementInput()) return Vector2.zero;

        Vector2 inputVector = playerInputActions.Movement.Move.ReadValue<Vector2>();
        inputVector.Normalize();
        return inputVector;
    }
    public override Vector2 GetIsometricDirectionVectorNormalized()
    {
        if (!CanProcessMovementInput()) return Vector2.zero;

        Vector2 rawDirection = GetDirectionVectorNormalized();
        Vector3 rawDirectionVector3 = new Vector3(rawDirection.x,0f,rawDirection.y);
        Vector3 rotatedDirection = Quaternion.AngleAxis(cameraTransform.eulerAngles.y, Vector3.up) * rawDirectionVector3;
        Vector2 isometricDirection = new Vector2(rotatedDirection.x, rotatedDirection.z);

        isometricDirection.Normalize();

        return isometricDirection;
    }

    public override bool GetJump()
    {
        if (!CanProcessMovementInput()) return false;

        bool jumpInput = playerInputActions.Movement.Jump.WasPerformedThisFrame();
        return jumpInput;
    }

    public override bool GetSprintDown()
    {
        if (!CanProcessMovementInput()) return false;

        bool sprintInput = playerInputActions.Movement.Sprint.WasPerformedThisFrame();
        return sprintInput;
    }

    public override bool GetSprintUp()
    {
        if (!CanProcessMovementInput()) return false;

        bool sprintInput = playerInputActions.Movement.Sprint.WasReleasedThisFrame();
        return sprintInput;
    }

    public override bool GetSprintHold()
    {
        if (!CanProcessMovementInput()) return false;

        bool sprintInput = playerInputActions.Movement.Sprint.IsPressed();
        return sprintInput;
    }

    public override bool GetCrouchDown()
    {
        if (!CanProcessMovementInput()) return false;

        bool crouchInput = playerInputActions.Movement.Crouch.WasPerformedThisFrame();
        return crouchInput;
    }

    public override bool GetCrouchUp()
    {
        if (!CanProcessMovementInput()) return false;

        bool crouchInput = playerInputActions.Movement.Crouch.WasReleasedThisFrame();
        return crouchInput;
    }

    public override bool GetCrouchHold()
    {
        if (!CanProcessMovementInput()) return false;

        bool crouchInput = playerInputActions.Movement.Crouch.IsPressed();
        return crouchInput;
    }
}

