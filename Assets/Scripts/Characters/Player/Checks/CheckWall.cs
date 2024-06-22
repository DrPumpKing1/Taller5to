using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWall : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private PlayerHorizontalMovement playerHorizontalMovement;

    [Header("General Settings")]
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Wall Detection Settings")]
    [SerializeField, Range(-0.2f, 1f)] private float wallRayLength = 0.1f;
    [SerializeField, Range(0.01f, 1f)] private float wallSphereRadius = 0.1f;
    [Space]
    [SerializeField, Range(0f, 1f)] private List<float> wallDetectionPoints;

    [Header("Diagonal Wall Detection Settings")]
    [SerializeField, Range(0f, 1f)] private float diagonalRayLength = 0.1f;
    [SerializeField, Range(0f, 1f)] private List<float> diagonalWallDetectionPoints;

    [Header("Corner Detection Settings")]
    [SerializeField, Range(0f, 1f)] private float cornerRayLength = 0.1f;
    [SerializeField, Range(0f, 1f)] private List<float> cornerDetectionPoints;

    [Header("Debug")]
    [SerializeField] private bool drawRaycasts;

    private Vector3 MoveDirection => GeneralMethods.Vector2ToVector3(playerHorizontalMovement.LastNonZeroInput);
    private RaycastHit wallInfo;
    private RaycastHit diagonalWallInfo;

    public bool HitWall { get; private set; }
    public bool HitDiagonalWall { get; private set; }
    public bool HitCorner { get; private set; }
    public bool MovementTowardsWall { get; private set; }

    private void FixedUpdate()
    {
        HitWall = CheckIfWall();
        HitDiagonalWall = CheckIfDiagonalWall();
        HitCorner = CheckIfCorner();
        MovementTowardsWall = CheckMovementTowardsWall();
    }

    private bool CheckIfWall()
    {
        foreach(float wallDetectionPoint in wallDetectionPoints)
        {
            if (CheckIfWallAtPoint(transform.position + new Vector3(0f, capsuleCollider.height * wallDetectionPoint, 0f), capsuleCollider.radius + wallRayLength, wallSphereRadius)) return true;
        }

        return false;
    }
    private bool CheckIfDiagonalWall()
    {
        foreach (float diagonalWallDetectionPoint in diagonalWallDetectionPoints)
        {
            if (CheckIfWallAtPoint(transform.position + new Vector3(0f, capsuleCollider.height * diagonalWallDetectionPoint, 0f), capsuleCollider.radius + diagonalRayLength)) return true;
        }

        return false;
    }

    private bool CheckIfCorner()
    {
        foreach (float cornerDetectionPoint in cornerDetectionPoints)
        {
            if (CheckIfCornerAtPoint(transform.position + new Vector3(0f, capsuleCollider.height * cornerDetectionPoint, 0f), capsuleCollider.radius + cornerRayLength)) return true;
        }

        return false;
    }

    private bool CheckIfWallAtPoint(Vector3 origin, float rayLength, float raySphereRadius)
    {
        bool hitWall = false;

        if (MoveDirection != Vector3.zero)
        {
            hitWall = Physics.SphereCast(origin, raySphereRadius, MoveDirection, out RaycastHit info, rayLength, obstacleLayer);

            if (info.collider) wallInfo = info;
        }

        if (drawRaycasts) Debug.DrawRay(origin, MoveDirection * rayLength, Color.blue);

        return hitWall;
    }

    private bool CheckIfWallAtPoint(Vector3 origin, float rayLenght)
    {
        bool hitWall = false;

        if (MoveDirection != Vector3.zero)
        {
            hitWall = Physics.Raycast(origin, MoveDirection, out RaycastHit info, rayLenght, obstacleLayer);

            if (info.collider) diagonalWallInfo = info;
        }

        if (drawRaycasts) Debug.DrawRay(origin, MoveDirection * rayLenght, Color.white);

        return hitWall;
    }

    private bool CheckIfCornerAtPoint(Vector3 origin, float rayLenght)
    {
        bool hitCorner1 = false;
        bool hitCorner2 = false;

        Quaternion rotation1 = Quaternion.Euler(0, 90, 0);
        Quaternion rotation2 = Quaternion.Euler(0, -90, 0);

        if (MoveDirection != Vector3.zero)
        {
            hitCorner1 = Physics.Raycast(origin, rotation1 * MoveDirection , out RaycastHit info1, rayLenght, obstacleLayer);
            hitCorner2 = Physics.Raycast(origin, rotation2 * MoveDirection , out RaycastHit info2, rayLenght, obstacleLayer);
        }

        if (drawRaycasts) Debug.DrawRay(origin, rotation1 * MoveDirection * rayLenght, Color.blue);
        if (drawRaycasts) Debug.DrawRay(origin, rotation2 * MoveDirection * rayLenght, Color.blue);

        return hitCorner1 && hitCorner2;
    }

    public bool CheckMovementTowardsWall() => MoveDirection.normalized == -wallInfo.normal.normalized;
    public RaycastHit GetWallInfo() => wallInfo;
    public RaycastHit GetDiagonalWallInfo() => diagonalWallInfo;
}
