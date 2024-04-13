using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerJump : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private MovementInput movementInput;
    [SerializeField] private CheckGround checkGround;
    [SerializeField] private PlayerCrouch playerCrouch;
    [SerializeField] private PlayerGravityController playerGravityController;

    [Header("Jump Settings")]
    [SerializeField, Range(0f,0.5f)] private float impulseTime = 0.2f;
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float jumpCooldown = 1f;

    private enum State {NotJumping, Impulsing, Jump}
    private State state;

    private bool JumpInput => movementInput.GetJump();
    public bool NotJumping => state == State.NotJumping;
    private float jumpCooldownTime = 0f;
    private float timer = 0f;

    public event EventHandler OnPlayerImpulsing;
    public event EventHandler OnPlayerJump;

    private void Start()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        jumpCooldownTime = 0f;
    }

    public void HandleJump(ref Vector3 finalMoveVector)
    {
        switch (state)
        {
            case State.NotJumping:
                IdleLogic();
                break;
            case State.Impulsing:
                ImpulsingLogic();
                break;
            case State.Jump:
                JumpLogic(ref finalMoveVector);
                break;
        }
    }

    private void SetJumpState(State state) { this.state = state; }

    private void IdleLogic() 
    {
        HandleJumpCooldown();

        if (!checkGround.IsGrounded) return;
        if (playerCrouch.IsCrouching) return;

        if (JumpInput && !JumpOnCooldown())
        {
            ResetTimer();
            SetJumpState(State.Impulsing);

            OnPlayerImpulsing?.Invoke(this, EventArgs.Empty);
        }

    }

    private void ImpulsingLogic()
    {
        timer += Time.deltaTime;

        if(timer>= impulseTime)
        {
            ResetTimer();
            SetJumpState(State.Jump);
        }
    }

    private void JumpLogic(ref Vector3 finalMoveVector)
    {
        OnPlayerJump?.Invoke(this, EventArgs.Empty);

        Jump(ref finalMoveVector);
        jumpCooldownTime = jumpCooldown;

        ResetTimer();
        SetJumpState(State.NotJumping);
    }

    private void Jump(ref Vector3 finalMoveVector)
    {
        finalMoveVector.y = CalculateJumpForce(jumpHeight, Physics.gravity.y * playerGravityController.GetGravityMultiplier());
    }

    private float CalculateJumpForce(float jumpHeight, float gravity)
    {
        float jumpForce = Mathf.Sqrt(2 * Mathf.Abs(gravity) * jumpHeight);
        return jumpForce;
    }

    private void HandleJumpCooldown()
    {
        if (!checkGround.IsGrounded) return;
        jumpCooldownTime = jumpCooldownTime > 0f ? jumpCooldownTime-=Time.deltaTime : 0f;
    }

    private bool JumpOnCooldown() => jumpCooldownTime > 0f;
    private void ResetTimer() => timer = 0f;

}
