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

    [Header("Debug")]
    [SerializeField] private bool drawRaycasts;

    public bool IsGrounded { get; private set; } = false;
    public bool OnSlope  = false;
    public Vector3 SlopeNormal { get; private set; }

    private void Update()
    {
        IsGrounded = CheckGrounded();
        OnSlope = CheckSlope();
    }

    private bool CheckGrounded() => CheckRaycastGrounded();

    private bool CheckRaycastGrounded()
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

        return onSlope;
    }

    //private bool CheckCharacterControllerGrounded() => capsuleCollider.isGrounded;
}
