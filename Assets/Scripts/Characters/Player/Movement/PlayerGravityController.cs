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

    [Header("Slope Stick Settings")]
    [SerializeField, Range(0f, 20f)] private float stickToSlopeForce = 5f;

    public float GravityMultiplier { get { return gravityMultiplier; } }
    public float FallMultiplier { get { return fallMultiplier; } }
    public float LowJumpMultiplier { get { return lowJumpMultiplier; } }

    private Rigidbody _rigidbody;
    private CapsuleCollider capsulleCollider;

    private bool previousWasOnSlope;
    private bool currentIsOnSlope;

    private Vector3 previousSlopeNormal;
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
        previousSlopeNormal = currentSlopeNormal;
    }

    private void FixedUpdate()
    {
        HandleSlopes();
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
        if (!playerHorizontalMovement.IsRunning()) return;
        _rigidbody.AddForce(stickToSlopeForce * -checkGround.SlopeNormal, ForceMode.Force);
    }

    private void LeaveSlope()
    {
        capsulleCollider.material = frictionlessMaterial;
    }

}
