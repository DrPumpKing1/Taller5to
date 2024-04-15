using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckRoof : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CapsuleCollider capsuleCollider;

    [Header("Settings")]
    [SerializeField] private LayerMask roofLayer;
    [SerializeField] private bool useCharacterControllerValues;
    [SerializeField, Range(0f, 1f)] private float rayLength = 0;
    [SerializeField, Range(0.01f, 1f)] private float raySphereRadius;

    [Header("Debug")]
    [SerializeField] private bool drawRaycasts;

    private float characterControllerHeight;
    private RaycastHit roofInfo;
    public bool HitRoof { get; private set; }

    private void Update()
    {
        HitRoof = useCharacterControllerValues? CheckIfRoofCharacterController():CheckIfRoofRegular();
    }

    private void Start()
    {
        InitializeVariabled();
    }
    private void InitializeVariabled()
    {
        characterControllerHeight = capsuleCollider.height;
    }

    private bool CheckIfRoofRegular()
    {
        Vector3 origin = transform.position;

        float finalRayLength = rayLength;

        bool hitRoof = Physics.SphereCast(origin, raySphereRadius, Vector3.up, out roofInfo, finalRayLength, roofLayer);

        if (drawRaycasts) Debug.DrawRay(origin, Vector3.up * (finalRayLength), Color.green);

        return hitRoof;
    }

    private bool CheckIfRoofCharacterController()
    {
        Vector3 origin = transform.position;

        float finalRayLength = characterControllerHeight - capsuleCollider.radius; 

        bool hitRoof = Physics.SphereCast(origin, capsuleCollider.radius, Vector3.up, out roofInfo, finalRayLength, roofLayer);

        if (drawRaycasts) Debug.DrawRay(origin, Vector3.up * (finalRayLength), Color.green);

        return hitRoof;
    }

    public RaycastHit GetHitInfo() => roofInfo;
}
