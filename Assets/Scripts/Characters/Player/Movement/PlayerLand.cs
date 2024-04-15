using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerLand : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CheckGround checkGround;
    [SerializeField] private PlayerGravityController playerGravityController;

    [Header("Land Settings")]
    [SerializeField, Range(0f, 1.5f)] private float landDetectionHeightThreshold;
    [SerializeField, Range(1.5f, 3f)] private float hardLandingThreshold;

    [SerializeField, Range(0f, 0.5f)] private float landRecoveryTime;
    [SerializeField, Range(0f, 0.5f)] private float hardLandRecoveryTime;

    public bool IsRecoveringFromLanding { get; private set; }

    private enum State {Idle, RegularLanding, HardLanding}
    private State state;

    private Rigidbody _rigidbody;
    private bool prevoiuslyGrounded;
    private float timer = 0f;

    public event EventHandler OnPlayerRegularLand;
    public event EventHandler OnPlayerHardLand;
    public event EventHandler<OnPlayerLandEventArgs> OnPlayerLand;

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
        prevoiuslyGrounded = checkGround.IsGrounded;
    }

    private void HandleLandStates()
    {
        switch (state)
        {
            case State.Idle:
                IdleLogic();
                break;
            case State.RegularLanding:
                RegularLandingLogic();
                break;
            case State.HardLanding:
                HardLandingLogic();
                break;
        }

        prevoiuslyGrounded = checkGround.IsGrounded;
    }

    private void SetLandState(State state) { this.state = state; }

    private void IdleLogic()
    {
        IsRecoveringFromLanding = false;

        if (!prevoiuslyGrounded && checkGround.IsGrounded)
        {
            float landHeight = CalculateLandHeight(_rigidbody.velocity.y, playerGravityController.GetGravity());

            if (HasSurpassedThreshold()) OnPlayerLand?.Invoke(this, new OnPlayerLandEventArgs { landHeight = landHeight});

            ResetTimer();

            if (landHeight >= hardLandingThreshold)
            {
                OnPlayerHardLand?.Invoke(this, EventArgs.Empty);
                SetLandState(State.HardLanding);
            }
            else
            {
                OnPlayerRegularLand?.Invoke(this, EventArgs.Empty);
                SetLandState(State.RegularLanding);
            }
        }
    }

    private void RegularLandingLogic()
    {
        IsRecoveringFromLanding = true;

        timer += Time.deltaTime;

        if (timer >= landRecoveryTime)
        {
            ResetTimer();
            SetLandState(State.Idle);
        }
    }

    private void HardLandingLogic()
    {
        IsRecoveringFromLanding = true;

        timer += Time.deltaTime;

        if (timer >= hardLandRecoveryTime)
        {
            ResetTimer();
            SetLandState(State.Idle);
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

    private bool HasSurpassedThreshold() => _rigidbody.velocity.y <= CalculateLandVelocity(landDetectionHeightThreshold, playerGravityController.GetGravity());
    private void ResetTimer() => timer = 0f;
}
