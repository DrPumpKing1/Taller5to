using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHorizontalMovement : MonoBehaviour
{
    public static PlayerHorizontalMovement Instance { get; private set; }

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
    [SerializeField, Range(1.5f,10f)] private float sprintSpeed = 2f;
    [SerializeField, Range(1.5f,10f)] private float walkSpeed = 2f;
    [Space]
    [SerializeField] private bool flattenSpeedOnSlopes;
    [SerializeField, Range(0f,10f)] private float flattenSpeedThreshold;

    [Header("Smooth Settings")]
    [SerializeField, Range(1f, 100f)] private float smoothInputFactor = 5f;
    [SerializeField, Range(1f, 100f)] private float smoothVelocityFactor = 5f;
    [SerializeField, Range(1f, 100f)] private float smoothDirectionFactor = 5f;

    [Header("State")]
    [SerializeField] private State state;
    private enum State {NotMoving, Walking, Sprinting}

    private Rigidbody _rigidbody;

    public Vector2 DirectionInputVector => movementInput.GetIsometricDirectionVectorNormalized();

    private float desiredSpeed;
    private float smoothCurrentSpeed;

    private bool movementTowardsWall;

    private Vector2 smoothDirectionInputVector;
    public Vector2 LastNonZeroInput { get; private set; }
    public Vector2 FixedLastNonZeroInput { get; private set; }
    public Vector3 FinalMoveDir { get; private set; }
    public Vector3 SmoothFinalMoveDir { get; private set; }
    public Vector3 FinalMoveVector { get; private set; }
    public bool MovementEnabled => movementEnabled;

    public static event EventHandler OnPlayerStopMoving;
    public static event EventHandler OnPlayerStartSprinting;
    public static event EventHandler OnPlayerStartWalking;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        SetSingleton();
    }

    private void Start()
    {
        SetMovementState(State.NotMoving);
    }

    private void Update()
    {
        HandleMovement();
        HandleMovementState();
    }

    private void FixedUpdate()
    {
        ApplyHorizontalMovement();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one PlayerHorizontalMovement instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
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
        float moveSpeed = RestrictedMovement() ? walkSpeed : sprintSpeed;
        desiredSpeed = CanMove() ? moveSpeed : 0f;
    }

    private bool CanMove()
    {
        if (DirectionInputVector == Vector2.zero) return false;
        if (checkWall.HitCorner) return false;
        if (movementTowardsWall) return false;
        if (playerLand.IsRecoveringFromLanding) return false;
        if (playerInteract.IsInteracting) return false;
        if (playerInteractAlternate.IsInteractingAlternate) return false;

        return true;
    }

    private bool RestrictedMovement() => GameManager.Instance.GameState == GameManager.State.OnRestrictedDialogue;

    private void SmoothSpeed()
    {
        smoothCurrentSpeed = Mathf.Lerp(smoothCurrentSpeed, desiredSpeed, Time.deltaTime * smoothVelocityFactor);
    }
    
    private void CalculateLastNonZeroDirectionInput() => LastNonZeroInput = DirectionInputVector != Vector2.zero ? DirectionInputVector : LastNonZeroInput;

    private void FixDirectionVectorDueToWalls()
    {        
        if (checkWall.HitDiagonalWall)
        {
            Vector3 wallNormal = checkWall.GetDiagonalWallInfo().normal;

            Vector3 vector3LastNonZeroInput = GeneralMethods.Vector2ToVector3(LastNonZeroInput);
            Vector3 projection = Vector3.Project(vector3LastNonZeroInput, wallNormal);

            if (vector3LastNonZeroInput.normalized == -wallNormal.normalized)
            {
                movementTowardsWall = true;
            }
            else
            {
                movementTowardsWall = false;
            }

            Vector3 perpendicularProyection = vector3LastNonZeroInput - projection;

            Vector2 vector2PerpendicularProyection = GeneralMethods.Vector3ToVector2(perpendicularProyection);

            FixedLastNonZeroInput = vector2PerpendicularProyection.normalized;

            return;
        }
        
        FixedLastNonZeroInput = LastNonZeroInput;
        movementTowardsWall = false;
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
        if (!checkGround.OnSlope) return vectorToFlat;
        if (!flattenSpeedOnSlopes) return vectorToFlat;
        if (FinalMoveVector.magnitude < flattenSpeedThreshold) return vectorToFlat;

        vectorToFlat = Vector3.ProjectOnPlane(vectorToFlat, checkGround.SlopeNormal);
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

    private void HandleMovementState()
    {
        switch (state)
        {
            case State.NotMoving:
                NotMovingLogic();
                break;
            case State.Walking:
                WalkingLogic();
                break;
            case State.Sprinting:
                SprintingLogic();
                break;
        }
    }

    private void NotMovingLogic()
    {
        if (!checkGround.IsGrounded) return;

        if(desiredSpeed == walkSpeed)
        {
            SetMovementState(State.Walking);
            OnPlayerStartWalking?.Invoke(this, EventArgs.Empty);
            return;
        }

        if (desiredSpeed == sprintSpeed)
        {
            SetMovementState(State.Sprinting);
            OnPlayerStartSprinting?.Invoke(this, EventArgs.Empty);
            return;
        }
    }

    private void WalkingLogic()
    {
        if (!checkGround.IsGrounded || desiredSpeed == 0f)
        {
            SetMovementState(State.NotMoving);
            OnPlayerStopMoving?.Invoke(this, EventArgs.Empty);
            return;
        }

        if (desiredSpeed == sprintSpeed)
        {
            SetMovementState(State.Sprinting);
            OnPlayerStartSprinting?.Invoke(this, EventArgs.Empty);
            return;
        }
    }

    private void SprintingLogic()
    {
        if (!checkGround.IsGrounded || desiredSpeed == 0f)
        {
            SetMovementState(State.NotMoving);
            OnPlayerStopMoving?.Invoke(this, EventArgs.Empty);
            return;
        }

        if (desiredSpeed == walkSpeed)
        {
            SetMovementState(State.Walking);
            OnPlayerStartWalking?.Invoke(this, EventArgs.Empty);
            return;
        }
    }

    private void SetMovementState(State state) => this.state = state;
}
