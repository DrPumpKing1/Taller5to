using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerJump : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private MovementInput movementInput;
    [Space]
    [SerializeField] private PlayerGravityController playerGravityController;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private PlayerInteractAlternate playerInteractAlternate;
    [Space]
    [SerializeField] private CheckGround checkGround;
    [SerializeField] private PlayerCrouch playerCrouch;

    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float jumpHeightError = 0.05f;
    [SerializeField, Range(0f,0.5f)] private float impulseTime = 0.2f;
    [SerializeField] private float jumpCooldown = 1f;

    [Header("Gravity Settings")]
    [SerializeField] private float gravityMultiplier = 1f;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private float lowJumpMultiplier;

    private Rigidbody _rigidbody;
    private enum State {NotJumping, Impulsing, Jump}
    private State state;

    private bool JumpInput => movementInput.GetJump();
    public bool NotJumping => state == State.NotJumping;
    private float jumpCooldownTime = 0f;
    private float timer = 0f;
    private bool shouldJump;

    public float GravityMultiplier { get { return gravityMultiplier; } }
    public float FallMultiplier { get { return fallMultiplier; } }
    public float LowJumpMultiplier { get { return lowJumpMultiplier; } }

    public event EventHandler OnPlayerImpulsing;
    public event EventHandler OnPlayerJump;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void Update()
    {
        CheckShouldJump();
        HandleJumpStates();
    }

    private void FixedUpdate()
    {
        BetterJumpLogic();
    }

    private void InitializeVariables()
    {
        jumpCooldownTime = 0f;
    }

    private void CheckShouldJump()
    {
        if (!checkGround.IsGrounded) return;
        if (playerCrouch.IsCrouching) return;
        if (playerInteract.IsInteracting) return;
        if (playerInteractAlternate.IsInteractingAlternate) return;
        if (jumpCooldownTime > 0) return;
        if (!JumpInput) return;

        shouldJump = true;
    }
    private void SetJumpState(State state) { this.state = state; }
    public void HandleJumpStates()
    {
        switch (state)
        {
            case State.NotJumping:
                NotJumpingLogic();
                break;
            case State.Impulsing:
                ImpulsingLogic();
                break;
            case State.Jump:
                JumpLogic();
                break;
        }
    }

    private void NotJumpingLogic() 
    {
        HandleJumpCooldown();

        if (shouldJump && !JumpOnCooldown())
        {
            ResetTimer();
            SetJumpState(State.Impulsing);
            shouldJump = false;

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

    private void JumpLogic()
    {
        OnPlayerJump?.Invoke(this, EventArgs.Empty);

        Jump();
        jumpCooldownTime = jumpCooldown;

        ResetTimer();
        SetJumpState(State.NotJumping);
    }

    private void Jump()
    {
        float jumpForce = CalculateJumpForce(jumpHeight + jumpHeightError, Physics.gravity.y * gravityMultiplier * lowJumpMultiplier);
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, jumpForce, _rigidbody.velocity.z);
    }

    private float CalculateJumpForce(float jumpHeight, float gravity)
    {
        float jumpForce = Mathf.Sqrt(2 * Mathf.Abs(gravity) * jumpHeight);
        return jumpForce;
    }

    private void HandleJumpCooldown()
    {
        if (!checkGround.IsGrounded) return;
        jumpCooldownTime = jumpCooldownTime > 0f ? jumpCooldownTime -= Time.deltaTime : 0f;
    }

    private void BetterJumpLogic()
    {
        if (checkGround.IsGrounded) return;

        if (_rigidbody.velocity.y < 0)
        {
            _rigidbody.velocity += Vector3.up * Physics.gravity.y * gravityMultiplier * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (_rigidbody.velocity.y > 0 && !shouldJump)
        {
            _rigidbody.velocity += Vector3.up * Physics.gravity.y * gravityMultiplier * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private bool JumpOnCooldown() => jumpCooldownTime > 0f;
    private void ResetTimer() => timer = 0f;

}
