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

    private CharacterController characterController;
    private bool prevoiuslyGrounded;

    public event EventHandler<OnPlayerLandEventArgs> OnPlayerLand;

    public class OnPlayerLandEventArgs : EventArgs
    {
        public float landHeight;
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void Update()
    {
        HandleLand();
    }

    private void InitializeVariables()
    {
        prevoiuslyGrounded = checkGround.IsGrounded;
    }

    private void HandleLand()
    {
        if(!prevoiuslyGrounded && checkGround.IsGrounded)
        {
            if (HasSurpassedThreshold()) OnPlayerLand?.Invoke(this, new OnPlayerLandEventArgs { landHeight = CalculateLandHeight(characterController.velocity.y, playerGravityController.GetGravity() )});
        }

        prevoiuslyGrounded = checkGround.IsGrounded;
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

    private bool HasSurpassedThreshold() => characterController.velocity.y <= CalculateLandVelocity(landDetectionHeightThreshold, playerGravityController.GetGravity());
}
