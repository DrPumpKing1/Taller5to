using System;
using UnityEngine;

public class NewMovementInput : MovementInput
{
    [Header("Components")]
    [SerializeField] private Transform cameraTransform;

    private PlayerInputActions playerInputActions;

    private Quaternion rotationQuaternion;

    protected override void Awake()
    {
        base.Awake();
        InitializePlayerInputActions();
    }

    private void Start()
    {
        InitializeRotationQuaternion();
    }

    private void InitializePlayerInputActions()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Movement.Enable();
    }

    private void InitializeRotationQuaternion()
    {
        rotationQuaternion = Quaternion.AngleAxis(cameraTransform.eulerAngles.y, Vector3.up);
    }

    public override bool CanProcessMovementInput()
    {
        if (GameManager.Instance.GameState != GameManager.State.OnGameplay) return false;

        if (DialogueManager.Instance._ManagerState == DialogueManager.ManagerState.ZeroMovementDialogue) return false;

        if (ScenesManager.Instance.SceneState == ScenesManager.State.TransitionOut) return false;
        if (ScenesManager.Instance.SceneState == ScenesManager.State.FullBlack) return false;

        return true;
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
        Vector3 rotatedDirection = rotationQuaternion * rawDirectionVector3;
        Vector2 isometricDirection = new Vector2(rotatedDirection.x, rotatedDirection.z);

        isometricDirection.Normalize();

        return isometricDirection;
    }
}

