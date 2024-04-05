using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWall : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private PlayerHorizontalMovement playerHorizontalMovement;

    [Header("Settings")]
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField, Range(0f, 1f)] private float rayObstacleLength = 0.1f;
    [SerializeField, Range(0.01f, 1f)] private float rayObstacleSphereRadius = 0.1f;

    [Header("Debug")]
    [SerializeField] private bool drawRaycasts;

    private Vector3 FacingDirection => playerHorizontalMovement.FinalMoveDir;
    private RaycastHit wallInfo;
    public bool HitWall { get; private set; }

    private void Update()
    {
        HitWall = CheckIfWall();
    }

    private bool CheckIfWall()
    {
        bool checkWallHalfBody = CheckIfWallAtPoint(transform.position + characterController.center);
        bool checkWall95PercentBody = CheckIfWallAtPoint(transform.position + characterController.center + new Vector3(0f,characterController.height * 0.45f,0f));

        return checkWallHalfBody || checkWall95PercentBody;
    }

    private bool CheckIfWallAtPoint(Vector3 origin)
    {
        bool hitWall = false;

        if (FacingDirection != Vector3.zero)
        {
            hitWall = Physics.SphereCast(origin, rayObstacleSphereRadius, FacingDirection, out wallInfo, rayObstacleLength, obstacleLayer);
        }

        if(drawRaycasts) Debug.DrawRay(origin, FacingDirection * rayObstacleLength, Color.blue);

        return hitWall;
    }

    public RaycastHit GetWallInfo() => wallInfo;
}
