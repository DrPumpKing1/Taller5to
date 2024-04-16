using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGravityController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CheckGround checkGround;

    [Header("Gravity Settings")]
    [SerializeField] private float gravityMultiplier = 1f;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private float lowJumpMultiplier;
    [SerializeField] private float stickToGroundForce = 5f;

    public float GravityMultiplier { get { return gravityMultiplier; } }
    public float FallMultiplier { get { return fallMultiplier; } }
    public float LowJumpMultiplier { get { return lowJumpMultiplier; } }

    private Rigidbody _rigidbody;

    private bool previousWasOnSlope;
    private bool isOnSlope;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        isOnSlope = checkGround.OnSlope;
        previousWasOnSlope = isOnSlope;
    }

    private void FixedUpdate()
    {
        //HandleSlopes();
    }

    private void HandleSlopes()
    {
        isOnSlope = checkGround.OnSlope;

        if (isOnSlope && !previousWasOnSlope)
        {
            StickToGround();
        }
        
        if( !isOnSlope && previousWasOnSlope)
        {
            LeaveGround();
        }

        previousWasOnSlope = isOnSlope;
    }

    private void StickToGround()
    {
        _rigidbody.useGravity = false;
        _rigidbody.velocity += (-checkGround.SlopeNormal * stickToGroundForce);
    }

    private void LeaveGround()
    {
        _rigidbody.velocity -= (-checkGround.SlopeNormal * stickToGroundForce);
        _rigidbody.useGravity = true;
    }
}
