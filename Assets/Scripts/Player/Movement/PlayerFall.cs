using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerFall : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CheckGround checkGround;
    [SerializeField] private PlayerGravityController playerGravityController;

    private CharacterController characterController;

    private bool isFalling;
    private bool previouslyFalling;

    public event EventHandler OnPlayerFall;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void Update()
    {
        HandleFall();
    }

    private void InitializeVariables()
    {
        isFalling = CheckIsFalling();
        previouslyFalling = isFalling;
    }

    private void HandleFall()
    {
        isFalling = CheckIsFalling();

        if (!previouslyFalling && isFalling)
        {
            OnPlayerFall?.Invoke(this, EventArgs.Empty);
        }

        previouslyFalling = isFalling;
    }


    private bool CheckIsFalling() => !checkGround.IsGrounded && characterController.velocity.y < 0f;
}
