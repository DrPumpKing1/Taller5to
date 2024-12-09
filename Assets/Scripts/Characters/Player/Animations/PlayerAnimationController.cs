using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [Space]
    [SerializeField] private PlayerHorizontalMovement playerHorizontalMovement;
    [SerializeField] private PlayerFall playerFall;
    [SerializeField] private PlayerLand playerLand;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private CheckGround checkGround;

    [Header("Settings")]
    [SerializeField,Range(0.1f,1f)] private float maxTimeFallingWhileGrounded;
    [SerializeField,Range(0.1f,0.5f)] private float forceFallCooldownTime;
    [SerializeField] private bool playingFallAnimation;
    [SerializeField] private bool grounded;

    public float HorizontalSpeed => playerHorizontalMovement.FinalMoveVector.magnitude;

    private const string FALL_ANIMATION_NAME = "Fall";
    private const string HARD_LAND_ANIMATION_NAME = "HardLand";
    private const string NORMAL_LAND_ANIMATION_NAME = "NormalLand";
    private const string MOVEMENT_ANIMATION_NAME = "IdleMove Blend Tree";

    private const string HORIZONTAL_SPEED_FLOAT = "HorizontalSpeed";


    private const string FALL_TRIGGER = "Fall";
    private const string SOFT_LAND_TRIGGER = "SoftLand";
    private const string NORMAL_LAND_TRIGGER = "NormalLand";
    private const string HARD_LAND_TRIGGER = "HardLand";

    private const string INTERACT_TRIGGER = "Interact";

    private const string GROUNDED_BOOL = "Grounded";

    private float fallingWhileGroundedTimer = 0f;
    private float forceFallCooldownTimer = 0f;

    private void OnEnable()
    {
        PlayerFall.OnPlayerFall += PlayerFall_OnPlayerFall;

        PlayerLand.OnPlayerSoftLand += PlayerLand_OnPlayerSoftLand;
        PlayerLand.OnPlayerNormalLand += PlayerLand_OnPlayerNormalLand;
        PlayerLand.OnPlayerHardLand += PlayerLand_OnPlayerHardLand;
    }

    private void OnDisable()
    {
        PlayerFall.OnPlayerFall -= PlayerFall_OnPlayerFall;

        PlayerLand.OnPlayerSoftLand -= PlayerLand_OnPlayerSoftLand;
        PlayerLand.OnPlayerNormalLand -= PlayerLand_OnPlayerNormalLand;
        PlayerLand.OnPlayerHardLand -= PlayerLand_OnPlayerHardLand;
    }

    private void Start()
    {
        ResetFallingWhileGroundedTimer();
        ResetForceFallCooldown();
    }

    private void Update()
    {
        HandleHorizontalSpeedBlend();
        UpdateBooleans();

        HandleForceFallLogic();
    }

    private void LateUpdate()
    {
        //HandleTriggerReset();
    }

    private void HandleHorizontalSpeedBlend()
    {
        animator.SetFloat(HORIZONTAL_SPEED_FLOAT, HorizontalSpeed);
    }

    private void UpdateBooleans()
    {
        animator.SetBool(GROUNDED_BOOL, checkGround.IsGrounded);
    }

    private void HandleTriggerReset()
    {
        if (!checkGround.IsGrounded) return;

        //animator.ResetTrigger(FALL_TRIGGER);
        //animator.ResetTrigger(SOFT_LAND_TRIGGER);
        //animator.ResetTrigger(NORMAL_LAND_TRIGGER);
        //animator.ResetTrigger(HARD_LAND_TRIGGER);
        //ResetLandingTriggers();
    }

    private void ResetLandingTriggers()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(FALL_ANIMATION_NAME)) return; //Hardcoded to prevent SoftLanding in Fall bug -> Not detecting SoftLanding trigger in Fall Animation, sometimes

        animator.ResetTrigger(SOFT_LAND_TRIGGER);
        animator.ResetTrigger(NORMAL_LAND_TRIGGER);
        animator.ResetTrigger(HARD_LAND_TRIGGER);
    }

    private void PlayerFall_OnPlayerFall(object sender, EventArgs e)
    {
        animator.ResetTrigger(SOFT_LAND_TRIGGER);
        animator.ResetTrigger(NORMAL_LAND_TRIGGER);
        animator.ResetTrigger(HARD_LAND_TRIGGER);

        animator.SetTrigger(FALL_TRIGGER);
    }

    private void PlayerLand_OnPlayerSoftLand(object sender, EventArgs e)
    {
        //if (animator.GetCurrentAnimatorStateInfo(0).IsName(MOVEMENT_ANIMATION_NAME)) return;

        animator.ResetTrigger(FALL_TRIGGER);
        animator.ResetTrigger(NORMAL_LAND_TRIGGER);
        animator.ResetTrigger(HARD_LAND_TRIGGER);

        animator.SetTrigger(SOFT_LAND_TRIGGER);
    }

    private void PlayerLand_OnPlayerNormalLand(object sender, EventArgs e)
    {
        //if (animator.GetCurrentAnimatorStateInfo(0).IsName(NORMAL_LAND_ANIMATION_NAME)) return;

        animator.ResetTrigger(FALL_TRIGGER);
        animator.ResetTrigger(SOFT_LAND_TRIGGER);
        animator.ResetTrigger(HARD_LAND_TRIGGER);

        animator.SetTrigger(NORMAL_LAND_TRIGGER);
    }

    private void PlayerLand_OnPlayerHardLand(object sender, EventArgs e)
    {
        //if (animator.GetCurrentAnimatorStateInfo(0).IsName(HARD_LAND_ANIMATION_NAME)) return;

        animator.ResetTrigger(FALL_TRIGGER);
        animator.ResetTrigger(SOFT_LAND_TRIGGER);
        animator.ResetTrigger(NORMAL_LAND_TRIGGER);

        animator.SetTrigger(HARD_LAND_TRIGGER);
    }


    #region ForceFall

    private void HandleForceFallLogic()
    {
        playingFallAnimation = PlayingFallAnimation();
        grounded = PlayerGrounded();

        if(!PlayingFallAnimation() || !PlayerGrounded())
        {
            ResetFallingWhileGroundedTimer();
            ResetForceFallCooldown();
            return;
        }

        if(fallingWhileGroundedTimer < maxTimeFallingWhileGrounded)
        {
            fallingWhileGroundedTimer += Time.deltaTime;
            return;
        }

        HandleForceFall();
    }

    private void HandleForceFall()
    {
        if (ForceFallOnCooldown())
        {
            forceFallCooldownTimer -= Time.deltaTime;
            return;
        }

        SetForceFallOnCooldown();
        ForceFall();
    }

    private void ForceFall()
    {
        animator.ResetTrigger(FALL_TRIGGER);
        animator.ResetTrigger(NORMAL_LAND_TRIGGER);
        animator.ResetTrigger(HARD_LAND_TRIGGER);

        animator.SetTrigger(SOFT_LAND_TRIGGER);

        Debug.Log("Forcing Fall");
    }

    private bool PlayingFallAnimation()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(FALL_ANIMATION_NAME)) return false;
        return true;
    }

    private bool PlayerGrounded() => checkGround.IsGrounded;

    private void ResetFallingWhileGroundedTimer() => fallingWhileGroundedTimer = 0;

    private void ResetForceFallCooldown() => forceFallCooldownTimer = 0;
    private void SetForceFallOnCooldown() => forceFallCooldownTimer = forceFallCooldownTime;
    private bool ForceFallOnCooldown() => forceFallCooldownTimer > 0;

    #endregion
}
