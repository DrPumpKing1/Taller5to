using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] CapsuleCollider capsuleCollider;

    [Header("Check Ground Settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField, Range(-1f, 1f)] private float checkGoundRayLenght = 0.1f;
    [SerializeField, Range(0.01f, 1f)] private float raySphereRadius = 0.1f;

    [Header("Check Slope Settings")]
    [SerializeField, Range(0f, 1f)] private float checkSlopeRayLength = 0.2f;

    [Header("Distance From Ground Settings)")]
    [SerializeField, Range(0f, 1f)] private float checkDistanceGroundRayLenght;
    private Vector3 checkDistanceFromGroundOffset = new Vector3(0f,0.5f,0f);

    [Header("Debug")]
    [SerializeField] private bool drawRaycasts;

    public bool IsGrounded { get; private set; } = false;
    public bool OnSlope { get; private set; } = false;
    public Vector3 SlopeNormal { get; private set; }
    public float DistanceFromGround { get; private set; }

    private void FixedUpdate()
    {
        IsGrounded = CheckGrounded();
        OnSlope = CheckSlope();
        DistanceFromGround = CalculateDistanceFromGround();
    }

    private bool CheckGrounded()
    {
        Vector3 origin = transform.position + capsuleCollider.center;
        float finalRayLength = checkGoundRayLenght + capsuleCollider.center.y;

        bool isGrounded = Physics.SphereCast(origin, raySphereRadius, Vector3.down, out RaycastHit groundInfo, finalRayLength, groundLayer);

        if(drawRaycasts) Debug.DrawRay(origin, Vector3.down * (finalRayLength), Color.red);

        return isGrounded;
    }

    private bool CheckSlope()
    {
        Vector3 origin = transform.position + capsuleCollider.center;
        float finalRayLength = checkSlopeRayLength + capsuleCollider.center.y;

        bool onSlope = Physics.Raycast(origin, Vector3.down, out RaycastHit hitInfo, finalRayLength, groundLayer);
        SlopeNormal = hitInfo.normal;

        if (SlopeNormal == Vector3.up) return false;

        if (drawRaycasts) Debug.DrawRay(origin, Vector3.down * (finalRayLength), Color.cyan);

        return onSlope;
    }

    private float CalculateDistanceFromGround()
    {
        Vector3 origin = transform.position + checkDistanceFromGroundOffset;
        float distance = float.MaxValue;

        bool detectGround = Physics.Raycast(origin, Vector3.down, out RaycastHit hitInfo, checkDistanceGroundRayLenght, groundLayer);
        if(detectGround) distance = (hitInfo.point - transform.position).magnitude;

        return distance;
    }
}
