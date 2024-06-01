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

    public override bool CanProcessMovementInput()
    {
        if(GameManager.Instance.GameState == GameManager.State.OnGameplay) return true;
        if(GameManager.Instance.GameState == GameManager.State.OnFreeDialogue) return true;
        if (GameManager.Instance.GameState == GameManager.State.OnMonologue) return true;

        return false;
    }

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
}

