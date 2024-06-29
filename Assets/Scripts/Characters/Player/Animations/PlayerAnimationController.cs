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

    public float HorizontalSpeed => playerHorizontalMovement.FinalMoveVector.magnitude;

    private const string FALL_ANIMATION_NAME = "Fall";

    private const string HORIZONTAL_SPEED_FLOAT = "HorizontalSpeed";


    private const string FALL_TRIGGER = "Fall";
    private const string SOFT_LAND_TRIGGER = "SoftLand";
    private const string NORMAL_LAND_TRIGGER = "NormalLand";
    private const string HARD_LAND_TRIGGER = "HardLand";

    private const string INTERACT_TRIGGER = "Interact";

    private const string DEATH_TRIGGER = "Death";

    private const string GROUNDED_BOOL = "Grounded";

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
        animator.SetFloat(HORIZONTAL_SPEED_FLOAT, HorizontalSpeed);
    }

    private void UpdateBooleans()
    {
        animator.SetBool(GROUNDED_BOOL, checkGround.IsGrounded);
    }

    private void HandleTriggerReset()
    {
        if (animator.IsInTransition(0)) return;

        animator.ResetTrigger(FALL_TRIGGER);
        ResetLandingTriggers();
    }

    private void ResetLandingTriggers()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(FALL_ANIMATION_NAME)) return; //Hardcoded to prevent SoftLanding in Fall bug -> Not detecting SoftLanding trigger in Fall Animation, sometimes

        animator.ResetTrigger(SOFT_LAND_TRIGGER);
        animator.ResetTrigger(NORMAL_LAND_TRIGGER);
        animator.ResetTrigger(HARD_LAND_TRIGGER);
    }

    private void PlayerFall_OnPlayerFall(object sender, EventArgs e) => animator.SetTrigger(FALL_TRIGGER);
    private void PlayerLand_OnPlayerSoftLand(object sender, EventArgs e) => animator.SetTrigger(SOFT_LAND_TRIGGER);
    private void PlayerLand_OnPlayerNormalLand(object sender, EventArgs e) => animator.SetTrigger(NORMAL_LAND_TRIGGER);
    private void PlayerLand_OnPlayerHardLand(object sender, EventArgs e) => animator.SetTrigger(HARD_LAND_TRIGGER);
    private void PlayerHealth_OnPlayerDeath(object sender, EventArgs e) => animator.SetTrigger(DEATH_TRIGGER);
}
