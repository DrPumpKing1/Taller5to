using System;
using UnityEngine;

public class VirtualMovementInput : MovementInput, IActionHandler
{
    [Header("Components")]
    [SerializeField] private Transform cameraTransform;

    [Header("Movement Actions")]
    private ActionListener horizontalAction;
    private ActionListener verticalAction;

    [Header("Jump Actions")]
    private ActionListener jumpAction;

    [Header("Sprint Actions")]
    private ActionListener sprintDownAction;
    private ActionListener sprintUpAction;
    private ActionListener sprintHoldAction;

    [Header("Crouch Actions")]
    private ActionListener crouchDownAction;
    private ActionListener crouchUpAction;
    private ActionListener crouchHoldAction;

    protected override void Awake()
    {
        base.Awake();
        SetUpActionListener();
    }

    public override bool CanProcessMovementInput() => GameManager.Instance.GameState == GameManager.State.OnGameplay;

    public override Vector2 GetDirectionVectorNormalized()
    {
        if (!CanProcessMovementInput()) return Vector2.zero;

        Vector2 inputVector = new Vector2(horizontalAction.value.f, verticalAction.value.f);
        inputVector.Normalize();
        return inputVector;
    }
    public override Vector2 GetIsometricDirectionVectorNormalized()
    {
        if (!CanProcessMovementInput()) return Vector2.zero;

        Vector2 rawDirection = GetDirectionVectorNormalized();
        Vector3 rawDirectionVector3 = new Vector3(rawDirection.x, 0f, rawDirection.y);
        Vector3 rotatedDirection = Quaternion.AngleAxis(cameraTransform.eulerAngles.y, Vector3.up) * rawDirectionVector3;
        Vector2 isometricDirection = new Vector2(rotatedDirection.x, rotatedDirection.z);

        isometricDirection.Normalize();

        return isometricDirection;
    }

    public override bool GetJump()
    {
        if (!CanProcessMovementInput()) return false;

        bool jumpInput = jumpAction.value.b;
        return jumpInput;
    }

    public override bool GetSprintDown()
    {
        if (!CanProcessMovementInput()) return false;

        bool sprintInput = sprintDownAction.value.b;
        return sprintInput;
    }

    public override bool GetSprintUp()
    {
        if (!CanProcessMovementInput()) return false;

        bool sprintInput = sprintUpAction.value.b;
        return sprintInput;
    }

    public override bool GetSprintHold()
    {
        if (!CanProcessMovementInput()) return false;

        bool sprintInput = sprintHoldAction.value.b;
        return sprintInput;
    }

    public override bool GetCrouchDown()
    {
        if (!CanProcessMovementInput()) return false;

        bool crouchInput = crouchDownAction.value.b;
        return crouchInput;
    }

    public override bool GetCrouchUp()
    {
        if (!CanProcessMovementInput()) return false;

        bool crouchInput = crouchUpAction.value.b;
        return crouchInput;
    }

    public override bool GetCrouchHold()
    {
        if (!CanProcessMovementInput()) return false;

        bool crouchInput = crouchHoldAction.value.b;
        return crouchInput;
    }

    public void SetUpActionListener()
    {
        horizontalAction = gameObject.AddComponent<ActionListener>();
        horizontalAction.SetActionName("Horizontal");

        verticalAction = gameObject.AddComponent<ActionListener>();
        verticalAction.SetActionName("Vertical");

        jumpAction = gameObject.AddComponent<ActionListener>();
        jumpAction.SetActionName("Jump");

        sprintDownAction = gameObject.AddComponent<ActionListener>();
        sprintDownAction.SetActionName("Sprint Down");

        sprintUpAction = gameObject.AddComponent<ActionListener>();
        sprintUpAction.SetActionName("Sprint Up");

        sprintHoldAction = gameObject.AddComponent<ActionListener>();
        sprintHoldAction.SetActionName("Sprint");

        crouchDownAction = gameObject.AddComponent<ActionListener>();
        crouchDownAction.SetActionName("Crouch Down");

        crouchUpAction = gameObject.AddComponent<ActionListener>();
        crouchUpAction.SetActionName("Crouch Up");

        crouchHoldAction = gameObject.AddComponent<ActionListener>();
        crouchHoldAction.SetActionName("Crouch");
    }
}

