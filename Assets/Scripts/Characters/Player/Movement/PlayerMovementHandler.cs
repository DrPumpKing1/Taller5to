using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerHorizontalMovement playerHorizontalMovement;
    [SerializeField] private PlayerJump playerJump;
    [SerializeField] private PlayerGravityController playerGravityController;

    public float HorizontalMovementMagnitude => GetHorizontalMovementMagnitude();

    private CharacterController characterController;
    private Vector3 finalMoveVector;

    public float test;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        playerHorizontalMovement.HandleHorizontalMovement(ref finalMoveVector);
        playerGravityController.ApplyGravity(ref finalMoveVector); //First apply gravity (Stick to ground force applied if grounded) 
        playerJump.HandleJump(ref finalMoveVector);
        
        ApplyMovement();

        test = HorizontalMovementMagnitude;
    }

    private void ApplyMovement()
    {
        characterController.Move(finalMoveVector * Time.deltaTime);
    }

    private float GetHorizontalMovementMagnitude()
    {
        Vector2 horizontalMovementVector = GeneralMethods.Vector3ToVector2(finalMoveVector);
        return horizontalMovementVector.magnitude;
    }
}
