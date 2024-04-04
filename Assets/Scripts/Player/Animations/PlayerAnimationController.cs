using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerMovementHandler playerMovementHandler;
    [SerializeField] private PlayerJump playerJump;
    [SerializeField] private PlayerFall playerFall;
    [SerializeField] private PlayerLand playerLand;
    [SerializeField] private PlayerCrouch playerCrouch;
    [SerializeField] private PlayerInteract playerInteract;

    private Animator animator;

    public float horizontalSpeed => playerMovementHandler.HorizontalMovementMagnitude;

    private const string HORIZONTAL_SPEED_FLOAT = "HorizontalSpeed";

    private const string JUMP_TRIGGER = "Jump";
    private const string FALL_TRIGGER = "Fall";
    private const string LAND_TRIGGER = "Land";
    private const string STAND_DOWN_TRIGGER = "StandDown";
    private const string STAND_UP_TRIGGER = "StandUp";
    private const string INTERACT_TRIGGER = "Interact";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        playerJump.OnPlayerJump += PlayerJump_OnPlayerJump;
        playerFall.OnPlayerFall += PlayerFall_OnPlayerFall;
        playerLand.OnPlayerLand += PlayerLand_OnPlayerLand;
        playerCrouch.OnPlayerStandDown += PlayerCrouch_OnPlayerStandDown;
        playerCrouch.OnPlayerStandUp += PlayerCrouch_OnPlayerStandUp;
    }
    private void OnDisable()
    {
        playerJump.OnPlayerJump -= PlayerJump_OnPlayerJump;
        playerFall.OnPlayerFall -= PlayerFall_OnPlayerFall;
        playerLand.OnPlayerLand -= PlayerLand_OnPlayerLand;
        playerCrouch.OnPlayerStandDown -= PlayerCrouch_OnPlayerStandDown;
        playerCrouch.OnPlayerStandUp -= PlayerCrouch_OnPlayerStandUp;
    }

    private void Update()
    {
        HandleHorizontalSpeedBlend();
    }

    private void HandleHorizontalSpeedBlend()
    {
        animator.SetFloat(HORIZONTAL_SPEED_FLOAT, horizontalSpeed);
    }

    private void PlayerJump_OnPlayerJump(object sender, System.EventArgs e) => animator.SetTrigger(JUMP_TRIGGER);
    private void PlayerFall_OnPlayerFall(object sender, System.EventArgs e) => animator.SetTrigger(FALL_TRIGGER);
    private void PlayerLand_OnPlayerLand(object sender, PlayerLand.OnPlayerLandEventArgs e) => animator.SetTrigger(LAND_TRIGGER);
    private void PlayerCrouch_OnPlayerStandDown(object sender, System.EventArgs e) => animator.SetTrigger(STAND_DOWN_TRIGGER);
    private void PlayerCrouch_OnPlayerStandUp(object sender, System.EventArgs e) => animator.SetTrigger(STAND_UP_TRIGGER);

}
