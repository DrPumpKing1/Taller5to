using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCrouch : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private MovementInput movementInput;
    [Space]
    [SerializeField] private PlayerJump playerJump;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private PlayerInteractAlternate playerInteractAlternate;
    [Space]
    [SerializeField] private CheckGround checkGround;
    [SerializeField] private CheckRoof checkRoof;

    [Space, Header("Crouch Settings")]
    [SerializeField, Range(0.2f, 0.9f)] private float crouchPercent = 0.6f;
    [SerializeField] private float crouchTransitionDuration = 1f;
    [SerializeField] private AnimationCurve crouchTransitionCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    private CapsuleCollider capsuleCollider;

    private enum State { NotCrouching, Crouching }
    private State state;

    private bool CrouchInput => movementInput.GetCrouchDown();
    public bool IsCrouching { get; private set; }
    public bool IsCrouchingTransitioning { get; private set; }

    private bool shouldBeCrouching;
    private bool crouchingActive;

    private float normalHeight;
    private float crouchHeight;
    private float stateStartingHeight;
    private float timer = 0f;

    public event EventHandler OnPlayerStandDown;
    public event EventHandler OnPlayerStandUp;

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        InitializeVariables();
    }

    private void Update()
    {
        HandleCrouch();
    }

    private void InitializeVariables()
    {
        normalHeight = capsuleCollider.height;
        crouchHeight = normalHeight * crouchPercent;

        IsCrouching = false;
        shouldBeCrouching = false;
        crouchingActive = false;

        timer = 0f;
    }

    private void SetCrouchState(State state) { this.state = state; }

    public void HandleCrouch()
    {
        switch (state)
        {
            case State.NotCrouching:
                NotCrouchingLogic();
                break;
            case State.Crouching:
                CrouchingLogic();
                break;
        }

        CheckToggleCrouch();
    }

    private void CheckToggleCrouch()
    {
        if (!checkGround.IsGrounded) return;

        if (CrouchInput)
        {
            if (IsCrouching && checkRoof.HitRoof) return;
            if (!playerJump.NotJumping) return;
            if (playerInteract.IsInteracting) return;
            if (playerInteractAlternate.IsInteractingAlternate) return;

            shouldBeCrouching = !shouldBeCrouching;
        }
    }

    private void NotCrouchingLogic()
    {
        if (shouldBeCrouching)
        {
            ResetTimer();
            SetCrouchState(State.Crouching);
        }

        IsCrouching = false;
    }

    private void CrouchingLogic()
    {
        if (shouldBeCrouching)
        {
            if (capsuleCollider.height == crouchHeight)
            {
                IsCrouchingTransitioning = false;
                return;
            }

            if (!crouchingActive)
            {
                crouchingActive = true;
                stateStartingHeight = capsuleCollider.height;
                ResetTimer();

                OnPlayerStandDown?.Invoke(this, EventArgs.Empty);
            }

            if (timer < crouchTransitionDuration)
            {
                timer += Time.deltaTime;
                float percent = timer / crouchTransitionDuration;
                float curveValue = crouchTransitionCurve.Evaluate(percent);

                capsuleCollider.height = Mathf.Lerp(stateStartingHeight, crouchHeight, curveValue);
                IsCrouchingTransitioning = true;
            }
        }
        else
        {
            if (capsuleCollider.height == normalHeight)
            {
                ResetTimer();
                SetCrouchState(State.NotCrouching);
                IsCrouchingTransitioning = false;
                return;
            }

            if (crouchingActive)
            {
                crouchingActive = false;
                stateStartingHeight = capsuleCollider.height;
                ResetTimer();

                OnPlayerStandUp?.Invoke(this, EventArgs.Empty);
            }

            if (timer < crouchTransitionDuration)
            {
                timer += Time.deltaTime;
                float percent = timer / crouchTransitionDuration;
                float curveValue = crouchTransitionCurve.Evaluate(percent);

                capsuleCollider.height = Mathf.Lerp(stateStartingHeight, normalHeight, curveValue);
                IsCrouchingTransitioning = true;
            }
        }

        IsCrouching = true;

        Vector3 newCharacterControllerCenter = new Vector3(capsuleCollider.center.x, capsuleCollider.height / 2, capsuleCollider.center.z);
        capsuleCollider.center = newCharacterControllerCenter;
    }

    private void ResetTimer() => timer = 0f;
}
