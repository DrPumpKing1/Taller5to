using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStep : MonoBehaviour
{
    [Header("Enabler")]
    [SerializeField] private bool enableStepOffset;

    [Header("Components")]
    [SerializeField] private PlayerHorizontalMovement playerHorizontalMovement;
    [SerializeField] private CheckGround checkGround;

    [Header("Settings")]
    [SerializeField] private LayerMask stepCheckLayers;
    [SerializeField, Range(0.1f, 0.3f)] private float stepOffset;
    [SerializeField,Range(0.01f,1f)] private float stepCheckDistance;
    [SerializeField,Range(0.01f,0.1f)] private float stepCheckUpDistance;
    [Space]
    [SerializeField, Range(0.1f, 0.2f)] private float raySpacingPosiion;
    [SerializeField, Range(0f, 0.1f)] private float forwardStepDistance;

    private Vector3 MoveDirection => GeneralMethods.Vector2ToVector3(playerHorizontalMovement.LastNonZeroInput);

    private void Update()
    {
        CheckStep();
    }

    private void CheckStep()
    {
        if (!enableStepOffset) return;
        if (!playerHorizontalMovement.HasMovementInput()) return;
        if (checkGround.OnSlope) return;

        Vector3 rayShootInMoveDirOrigin = transform.position + Vector3.up * stepCheckUpDistance;

        if (Physics.Raycast(rayShootInMoveDirOrigin, MoveDirection, out RaycastHit hit, stepCheckDistance, stepCheckLayers))
        {
            Vector3 rayShootDownOrigin = hit.point + MoveDirection * raySpacingPosiion + stepOffset * Vector3.up;

            if(Physics.Raycast(rayShootDownOrigin, Vector3.down, out RaycastHit hitDown, stepOffset, stepCheckLayers))
            {
                if (hit.transform != hitDown.transform) return;

                float stepDistance = hitDown.point.y - transform.position.y;

                if (stepDistance > stepOffset) return;

                Step(stepDistance, MoveDirection);
            }

        }

        Debug.DrawRay(transform.position + Vector3.up * stepCheckUpDistance, MoveDirection * stepCheckDistance, Color.white);
    }

    private void Step(float stepDistance, Vector3 moveDirection)
    {
        transform.position += new Vector3(0f, stepDistance, 0f);
        transform.position += moveDirection * forwardStepDistance;
    }
}
