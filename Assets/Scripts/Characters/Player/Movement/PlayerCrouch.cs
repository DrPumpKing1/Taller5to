using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCrouch : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private MovementInput movementInput;
    [SerializeField] private PlayerJump playerJump;
    [SerializeField] private CheckGround checkGround;
    [SerializeField] private CheckRoof checkRoof;

    [Space, Header("Crouch Settings")]
    [SerializeField, Range(0.2f, 0.9f)] private float crouchPercent = 0.6f;
    [SerializeField] private float crouchTransitionDuration = 1f;
    [SerializeField] private AnimationCurve crouchTransitionCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    private CharacterController characterController;

    private enum State { NotCrouching, Crouching }
    private State state;

    private bool CrouchInput => movementInput.GetCrouchDown();
    public bool IsCrouching { get; private set; }
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
        characterController = GetComponent<CharacterController>();
        InitializeVariables();
    }

    private void Update()
    {
        HandleCrouch();
    }

    private void InitializeVariables()
    {
        normalHeight = characterController.height;
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
                IdleLogic();
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

            shouldBeCrouching = !shouldBeCrouching;
        }
    }

    private void IdleLogic()
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
            if (characterController.height == crouchHeight) return;

            if (!crouchingActive)
            {
                crouchingActive = true;
                stateStartingHeight = characterController.height;
                ResetTimer();

                OnPlayerStandDown?.Invoke(this, EventArgs.Empty);
            }

            if (timer < crouchTransitionDuration)
            {
                timer += Time.deltaTime;
                float percent = timer / crouchTransitionDuration;
                float curveValue = crouchTransitionCurve.Evaluate(percent);

                characterController.height = Mathf.Lerp(stateStartingHeight, crouchHeight, curveValue);
            }
        }
        else
        {
            if (characterController.height == normalHeight)
            {
                ResetTimer();
                SetCrouchState(State.NotCrouching);
                return;
            }

            if (crouchingActive)
            {
                crouchingActive = false;
                stateStartingHeight = characterController.height;
                ResetTimer();

                OnPlayerStandUp?.Invoke(this, EventArgs.Empty);
            }

            if (timer < crouchTransitionDuration)
            {
                timer += Time.deltaTime;
                float percent = timer / crouchTransitionDuration;
                float curveValue = crouchTransitionCurve.Evaluate(percent);

                characterController.height = Mathf.Lerp(stateStartingHeight, normalHeight, curveValue);
            }
        }

        IsCrouching = true;

        Vector3 newCharacterControllerCenter = new Vector3(characterController.center.x, characterController.height / 2, characterController.center.z);
        characterController.center = newCharacterControllerCenter;
    }

    private void ResetTimer() => timer = 0f;
}
