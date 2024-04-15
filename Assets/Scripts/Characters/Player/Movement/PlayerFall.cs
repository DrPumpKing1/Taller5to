using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerFall : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CheckGround checkGround;

    private Rigidbody _rigidbody;

    private bool isFalling;
    private bool previouslyFalling;

    public event EventHandler OnPlayerFall;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
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


    private bool CheckIsFalling() => !checkGround.IsGrounded && _rigidbody.velocity.y < 0f;
}
