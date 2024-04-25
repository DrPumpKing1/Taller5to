using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerFall : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CheckGround checkGround;

    [Header("Fall Settings")]
    [SerializeField, Range(-1f,0f)] private float fallVelocityThreshold;
    [SerializeField, Range(0f,1f)] private float fallDistanceFromGroundThreshold;

    private Rigidbody _rigidbody;

    public bool IsFalling { get; private set; }
    public bool PreviouslyFalling { get; private set; }

    public event EventHandler OnPlayerFall;

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
        HandleFall();
    }

    private void InitializeVariables()
    {
        IsFalling = CheckIsFalling();
        PreviouslyFalling = IsFalling;
    }

    private void HandleFall()
    {
        IsFalling = CheckIsFalling();

        if (!PreviouslyFalling && IsFalling)
        {
            OnPlayerFall?.Invoke(this, EventArgs.Empty);
        }

        PreviouslyFalling = IsFalling;
    }


    private bool CheckIsFalling() 
    {
        if (checkGround.IsGrounded) return false;
        if (checkGround.DistanceFromGround < fallDistanceFromGroundThreshold) return false;
        if (_rigidbody.velocity.y > fallVelocityThreshold) return false;

        return true;
    }
}
