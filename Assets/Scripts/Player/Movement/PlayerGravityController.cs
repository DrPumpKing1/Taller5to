using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGravityController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CheckGround checkGround;

    [Space, Header("Gravity Settings")]
    [SerializeField] private float gravityMultiplier = 2.5f;
    [SerializeField] private float stickToGroundForce = 5f;

    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void ApplyGravity(ref Vector3 finalMoveVector)
    {
        if (characterController.isGrounded) 
        {
            finalMoveVector.y = -stickToGroundForce;
        }
        else
        {
            finalMoveVector += Physics.gravity * gravityMultiplier * Time.deltaTime;
        }
    }

    public float GetGravityMultiplier() => gravityMultiplier;
    public float GetGravity() => Physics.gravity.y * gravityMultiplier;
}
