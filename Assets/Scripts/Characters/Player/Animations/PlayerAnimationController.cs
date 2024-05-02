using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerHorizontalMovement playerHorizontalMovement;
    [SerializeField] private PlayerJump playerJump;
    [SerializeField] private PlayerFall playerFall;
    [SerializeField] private PlayerLand playerLand;
    [SerializeField] private PlayerCrouch playerCrouch;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private CheckGround checkGround;

    private Animator animator;

    public float horizontalSpeed => playerHorizontalMovement.FinalMoveVector.magnitude;

    private const string HORIZONTAL_SPEED_FLOAT = "HorizontalSpeed";

    private const string JUMP_TRIGGER = "Jump";
    private const string FALL_TRIGGER = "Fall";
    private const string SOFT_LAND_TRIGGER = "SoftLand";
    private const string NORMAL_LAND_TRIGGER = "NormalLand";
    private const string HARD_LAND_TRIGGER = "HardLand";
    private const string STAND_DOWN_TRIGGER = "StandDown";
    private const string STAND_UP_TRIGGER = "StandUp";
    private const string INTERACT_TRIGGER = "Interact";

    private const string GROUNDED_BOOL = "Grounded";


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        playerJump.OnPlayerImpulsing += PlayerJump_OnPlayerImpulsing;
        playerFall.OnPlayerFall += PlayerFall_OnPlayerFall;

        playerLand.OnPlayerSoftLand += PlayerLand_OnPlayerSoftLand;
        playerLand.OnPlayerNormalLand += PlayerLand_OnPlayerNormalLand;
        playerLand.OnPlayerHardLand += PlayerLand_OnPlayerHardLand;

        playerCrouch.OnPlayerStandDown += PlayerCrouch_OnPlayerStandDown;
        playerCrouch.OnPlayerStandUp += PlayerCrouch_OnPlayerStandUp;
    }

    private void OnDisable()
    {
        playerJump.OnPlayerImpulsing -= PlayerJump_OnPlayerImpulsing;
        playerFall.OnPlayerFall -= PlayerFall_OnPlayerFall;

        playerLand.OnPlayerSoftLand -= PlayerLand_OnPlayerSoftLand;
        playerLand.OnPlayerNormalLand -= PlayerLand_OnPlayerNormalLand;
        playerLand.OnPlayerHardLand -= PlayerLand_OnPlayerHardLand;

        playerCrouch.OnPlayerStandDown -= PlayerCrouch_OnPlayerStandDown;
        playerCrouch.OnPlayerStandUp -= PlayerCrouch_OnPlayerStandUp;
    }

    private void Update()
    {
        HandleHorizontalSpeedBlend();
        UpdateBooleans();
    }

    private void LateUpdate()
    {
        HandleTriggerReset();
    }

    private void HandleHorizontalSpeedBlend()
    {
        animator.SetFloat(HORIZONTAL_SPEED_FLOAT, horizontalSpeed);
    }

    private void UpdateBooleans()
    {
        animator.SetBool(GROUNDED_BOOL, checkGround.IsGrounded);
    }

    private void HandleTriggerReset()
    {
        if (animator.IsInTransition(0)) return;

        animator.ResetTrigger(JUMP_TRIGGER);
        animator.ResetTrigger(FALL_TRIGGER);
        ResetLandingTriggers();
        //animator.ResetTrigger(STAND_DOWN_TRIGGER);
        //animator.ResetTrigger(STAND_UP_TRIGGER);
    }

    private void ResetLandingTriggers()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Fall")) return; //Hardcoded to prevent SoftLanding in Fall bug -> Not detecting SoftLanding trigger in Fall Animation

        animator.ResetTrigger(SOFT_LAND_TRIGGER);
        animator.ResetTrigger(NORMAL_LAND_TRIGGER);
        animator.ResetTrigger(HARD_LAND_TRIGGER);
    }

    private void PlayerJump_OnPlayerImpulsing(object sender, EventArgs e) => animator.SetTrigger(JUMP_TRIGGER);
    private void PlayerFall_OnPlayerFall(object sender, EventArgs e) => animator.SetTrigger(FALL_TRIGGER);

    private void PlayerLand_OnPlayerSoftLand(object sender, EventArgs e) => animator.SetTrigger(SOFT_LAND_TRIGGER);
    private void PlayerLand_OnPlayerNormalLand(object sender, EventArgs e) => animator.SetTrigger(NORMAL_LAND_TRIGGER);
    private void PlayerLand_OnPlayerHardLand(object sender, EventArgs e) => animator.SetTrigger(HARD_LAND_TRIGGER);

    private void PlayerCrouch_OnPlayerStandDown(object sender, EventArgs e) => animator.SetTrigger(STAND_DOWN_TRIGGER);
    private void PlayerCrouch_OnPlayerStandUp(object sender, EventArgs e) => animator.SetTrigger(STAND_UP_TRIGGER);

}
