using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWall : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CapsuleCollider capsulleCollider;
    [SerializeField] private PlayerHorizontalMovement playerHorizontalMovement;

    [Header("General Settings")]
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Wall Detection Settings")]
    [SerializeField, Range(0f, 1f)] private float rayLength = 0.1f;
    [SerializeField, Range(0.01f, 1f)] private float raySphereRadius = 0.1f;
    [Space]
    [SerializeField, Range(0f, 1f)] private float lowRayLenght = 0.1f;
    [SerializeField, Range(0.01f, 1f)] private float lowRaySphereRadius = 0.1f;

    [Header("Forward Wall Detection Settings")]
    [SerializeField, Range(0f, 1f)] private float diagonalRayLength = 0.1f;

    [Header("Debug")]
    [SerializeField] private bool drawRaycasts;

    private Vector3 MoveDirection => GeneralMethods.Vector2ToVector3(playerHorizontalMovement.LastNonZeroInput);
    private RaycastHit wallInfo;
    private RaycastHit diagonalWallInfo;
    public bool HitWall { get; private set; }
    public bool HitDiagonalWall { get; private set; }

    private void FixedUpdate()
    {
        HitWall = CheckIfWall();
        HitDiagonalWall = CheckIfDiagonalWall();
    }

    private bool CheckIfWall()
    {
        bool checkWall15PercentBody = CheckIfWallAtPoint(transform.position + capsulleCollider.center + new Vector3(0f,capsulleCollider.height * -0.35f, 0f), lowRayLenght, lowRaySphereRadius);
        bool checkWallHalfBody = CheckIfWallAtPoint(transform.position + capsulleCollider.center,rayLength,raySphereRadius);
        bool checkWall95PercentBody = CheckIfWallAtPoint(transform.position + capsulleCollider.center + new Vector3(0f,capsulleCollider.height * 0.45f,0f), rayLength, raySphereRadius);

        return checkWall15PercentBody || checkWallHalfBody || checkWall95PercentBody;
    }
    private bool CheckIfDiagonalWall()
    {
        bool checkWallHalfBody = CheckIfWallAtPoint(transform.position + capsulleCollider.center, diagonalRayLength);

        return checkWallHalfBody;
    }

    private bool CheckIfWallAtPoint(Vector3 origin, float rayLength, float raySphereRadius)
    {
        bool hitWall = false;

        if (MoveDirection != Vector3.zero)
        {
            hitWall = Physics.SphereCast(origin, raySphereRadius, MoveDirection, out wallInfo, rayLength, obstacleLayer);
        }

        if(drawRaycasts) Debug.DrawRay(origin, MoveDirection * rayLength, Color.blue);

        return hitWall;
    }

    private bool CheckIfWallAtPoint(Vector3 origin, float rayLenght)
    {
        bool hitWall = false;

        if (MoveDirection != Vector3.zero)
        {
            hitWall = Physics.Raycast(origin, MoveDirection, out diagonalWallInfo, rayLenght, obstacleLayer);
        }

        if (drawRaycasts) Debug.DrawRay(origin, MoveDirection * rayLenght, Color.white);

        return hitWall;
    }

    public RaycastHit GetWallInfo() => wallInfo;
    public RaycastHit GetDiagonalWallInfo() => diagonalWallInfo;
}
