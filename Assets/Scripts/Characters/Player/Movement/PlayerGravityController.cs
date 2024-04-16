using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGravityController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CheckGround checkGround;

    [Header("Physic Materials")]
    [SerializeField] private PhysicMaterial frictionMaterial;
    [SerializeField] private PhysicMaterial frictionlessMaterial;

    [Header("Gravity Settings")]
    [SerializeField, Range(0.5f,2f)] private float gravityMultiplier = 1f;
    [SerializeField, Range(0.5f,3f)] private float fallMultiplier;
    [SerializeField, Range(0.5f,3f)] private float lowJumpMultiplier;

    [Header("Slope Stick Settings")]
    [SerializeField, Range(10f,100f)] private float stickToSlopeForce = 5f;
    [SerializeField, Range(1f,50f)] private int stickToSlopeFrames = 30;

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
        HandleSlopeChange();
    }

    private void HandleSlopes()
    {
        currentIsOnSlope = checkGround.OnSlope;

        if (currentIsOnSlope && !previousWasOnSlope)
        {
            StickToSlope();
        }

        if ( !currentIsOnSlope && previousWasOnSlope)
        {
            LeaveSlope();
        }

        previousWasOnSlope = currentIsOnSlope;
    }

    private void HandleSlopeChange()
    {
        currentSlopeNormal = checkGround.SlopeNormal;

        if (currentSlopeNormal != previousSlopeNormal)
        {
            if (!checkGround.OnSlope) return;
            StartCoroutine(StayOnSlopeCoroutine());
        }

        previousSlopeNormal = currentSlopeNormal;
    }

    private void StickToSlope()
    {
        capsulleCollider.material = frictionMaterial;       
    }

    private void StayOnSlope()
    {
        _rigidbody.AddForce(-checkGround.SlopeNormal * stickToSlopeForce, ForceMode.Force);
    }

    private void LeaveSlope()
    {
        capsulleCollider.material = frictionlessMaterial;
    }

    private IEnumerator StayOnSlopeCoroutine()
    {
        yield return new WaitForEndOfFrame();

        int frames = 0;

        while(frames < stickToSlopeFrames)
        {
            StayOnSlope();
            frames++;

            yield return null;
        }
    }

}
