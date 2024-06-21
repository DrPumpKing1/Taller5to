using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerLand : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerFall playerFall;
    [SerializeField] private CheckGround checkGround;
    [SerializeField] private PlayerGravityController playerGravityController;

    [Header("Land Settings")]
    [SerializeField, Range(0f, 1.5f)] private float landDetectionHeightThreshold;
    [Space]
    [SerializeField, Range(-1f, 1.5f)] private float softLandingThreshold;
    [Space]
    [SerializeField, Range(0f, 1.5f)] private float normalLandingThreshold;
    [SerializeField, Range(0f, 0.5f)] private float normalLandRecoveryTime;
    [Space]
    [SerializeField, Range(1.5f, 3f)] private float hardLandingThreshold;
    [SerializeField, Range(0f, 0.5f)] private float hardLandRecoveryTime;

    public bool IsRecoveringFromLanding { get; private set; }

    private enum State {NotLanding, SoftLanding, NormalLanding, HardLanding}
    private State state;

    private Rigidbody _rigidbody;
    private bool previouslyGrounded;
    private float timer = 0f;

    public static event EventHandler OnPlayerSoftLand;
    public static event EventHandler OnPlayerNormalLand;
    public static event EventHandler OnPlayerHardLand;
    public static event EventHandler<OnPlayerLandEventArgs> OnPlayerLand;

    public class OnPlayerLandEventArgs : EventArgs
    {
        public float landHeight;
    }

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
        HandleLandStates();
    }

    private void InitializeVariables()
    {
        previouslyGrounded = checkGround.IsGrounded;
    }

    private void HandleLandStates()
    {
        switch (state)
        {
            case State.NotLanding:
                NotLandingLogic();
                break;
            case State.SoftLanding:
                SoftLandingLogic();
                break;
            case State.NormalLanding:
                NormalLandingLogic();
                break;
            case State.HardLanding:
                HardLandingLogic();
                break;
        }

        previouslyGrounded = checkGround.IsGrounded;
    }

    private void SetLandState(State state) { this.state = state; }

    private void NotLandingLogic()
    {
        IsRecoveringFromLanding = false;

        if (!previouslyGrounded && checkGround.IsGrounded)
        {
            float landHeight = CalculateLandHeight(_rigidbody.velocity.y, Physics.gravity.y * playerGravityController.GravityMultiplier * playerGravityController. FallMultiplier);

            if (HasSurpassedThreshold()) OnPlayerLand?.Invoke(this, new OnPlayerLandEventArgs { landHeight = landHeight});

            ResetTimer();

            if (landHeight >= hardLandingThreshold)
            {
                OnPlayerHardLand?.Invoke(this, EventArgs.Empty);
                SetLandState(State.HardLanding);
            }
            else if(landHeight >= normalLandingThreshold)
            {
                OnPlayerNormalLand?.Invoke(this, EventArgs.Empty);
                SetLandState(State.NormalLanding);
            }
            else if(landHeight >= softLandingThreshold)
            {
                OnPlayerSoftLand?.Invoke(this, EventArgs.Empty);
                SetLandState(State.SoftLanding);
            }
        }
    }

    private void SoftLandingLogic()
    {
        IsRecoveringFromLanding = false;

        ResetTimer();
        SetLandState(State.NotLanding);
    }

    private void NormalLandingLogic()
    {
        IsRecoveringFromLanding = true;

        timer += Time.deltaTime;

        if (timer >= normalLandRecoveryTime)
        {
            ResetTimer();
            SetLandState(State.NotLanding);
        }
    }

    private void HardLandingLogic()
    {
        IsRecoveringFromLanding = true;

        timer += Time.deltaTime;

        if (timer >= hardLandRecoveryTime)
        {
            ResetTimer();
            SetLandState(State.NotLanding);
        }
    }

    private float CalculateLandVelocity(float height, float gravity)
    {
        float landVelocity = - Mathf.Sqrt(2 * height * Mathf.Abs(gravity));
        return landVelocity;
    }

    private float CalculateLandHeight(float velocity, float gravity)
    {
        float landHeight = Mathf.Pow(Mathf.Abs(velocity), 2) / (2 * Mathf.Abs(gravity));
        return landHeight;
    }

    private bool HasSurpassedThreshold() => _rigidbody.velocity.y <= CalculateLandVelocity(landDetectionHeightThreshold, Physics.gravity.y * playerGravityController.GravityMultiplier * playerGravityController.FallMultiplier);
    private void ResetTimer() => timer = 0f;
}
