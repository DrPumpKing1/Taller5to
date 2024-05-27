using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGravityController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CheckGround checkGround;
    [SerializeField] private PlayerHorizontalMovement playerHorizontalMovement;

    [Header("Physic Materials")]
    [SerializeField] private PhysicMaterial frictionMaterial;
    [SerializeField] private PhysicMaterial frictionlessMaterial;

    [Header("Gravity Settings")]
    [SerializeField, Range(0.5f, 2f)] private float gravityMultiplier = 1f;
    [SerializeField, Range(0.5f, 3f)] private float fallMultiplier;
    [SerializeField, Range(0.5f, 3f)] private float lowJumpMultiplier;

    [Header("Stick To Slope Settings")]
    [SerializeField] private bool enableStickToSlopeForce;
    [SerializeField] private float stickToSlopeSpeedThreshold;
    [SerializeField, Range(0f, 30f)] private float stickToSlopeForce = 5f;

    public float HorizontalSpeed => playerHorizontalMovement.FinalMoveVector.magnitude;

    public float GravityMultiplier => gravityMultiplier;
    public float FallMultiplier => fallMultiplier;
    public float LowJumpMultiplier => lowJumpMultiplier;

    private Rigidbody _rigidbody;
    private CapsuleCollider capsulleCollider;

    private bool previousWasOnSlope;
    private bool currentIsOnSlope;

    private Vector3 currentSlopeNormal;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        capsulleCollider = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        currentIsOnSlope = checkGround.OnSlope;
        previousWasOnSlope = currentIsOnSlope;

        currentSlopeNormal = checkGround.SlopeNormal;
    }

    private void FixedUpdate()
    {
        BetterFall();
        HandleSlopes();

    }

    private void BetterFall()
    {
        if (checkGround.IsGrounded) return;

        if (_rigidbody.velocity.y < 0)
        {
            _rigidbody.velocity += Vector3.up * Physics.gravity.y * GravityMultiplier * (FallMultiplier - 1) * Time.fixedDeltaTime;
        }

        else if (_rigidbody.velocity.y > 0)
        {
            _rigidbody.velocity += Vector3.up * Physics.gravity.y * GravityMultiplier * (LowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private void HandleSlopes()
    {
        currentIsOnSlope = checkGround.OnSlope;

        if (currentIsOnSlope && !previousWasOnSlope)
        {
            EnterSlope();
        }

        if (!currentIsOnSlope && previousWasOnSlope)
        {
            LeaveSlope();
        }

        if (currentIsOnSlope)
        {
            StayOnSlope();
        }

        previousWasOnSlope = currentIsOnSlope;
    }

    private void EnterSlope()
    {
        capsulleCollider.material = frictionMaterial;
    }

    private void StayOnSlope()
    {
        if (!enableStickToSlopeForce) return;
        if (HorizontalSpeed < stickToSlopeSpeedThreshold) return;

        _rigidbody.AddForce(stickToSlopeForce * -checkGround.SlopeNormal, ForceMode.Force);
    }

    private void LeaveSlope()
    {
        capsulleCollider.material = frictionlessMaterial;
    }

}
