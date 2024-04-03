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
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float jumpCooldown = 1f;

    private bool JumpInput => movementInput.GetJump();
    private float jumpCooldownTime = 0f;

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
        HandleJumpCooldown();

        if (!checkGround.IsGrounded) return;
        if (playerCrouch.IsCrouching) return;

        if (JumpInput && !JumpOnCooldown())
        {
            Jump(ref finalMoveVector);
            jumpCooldownTime = jumpCooldown;

            OnPlayerJump?.Invoke(this, EventArgs.Empty);
        }
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
        jumpCooldownTime = jumpCooldownTime > 0f ? jumpCooldownTime-=Time.deltaTime : 0f;
    }

    private bool JumpOnCooldown() => jumpCooldownTime > 0f;
}
