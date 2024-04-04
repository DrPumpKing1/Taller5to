using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMethodSub : MonoBehaviour
{
    [SerializeField] private PlayerJump playerJump;
    [SerializeField] private PlayerCrouch playerCrouch;
    [SerializeField] private PlayerLand playerLand;
    [SerializeField] private PlayerFall playerFall;

    private void OnEnable()
    {
        playerJump.OnPlayerJump += PlayerJump_OnPlayerJump;
        playerCrouch.OnPlayerCrouch += PlayerCrouch_OnPlayerCrouch;
        playerCrouch.OnPlayerStandUp += PlayerCrouch_OnPlayerStandUp;
        playerFall.OnPlayerFall += PlayerFall_OnPlayerFall;
        playerLand.OnPlayerLand += PlayerLand_OnPlayerLand;
    }

    private void OnDisable()
    {
        playerJump.OnPlayerJump -= PlayerJump_OnPlayerJump;
        playerCrouch.OnPlayerCrouch -= PlayerCrouch_OnPlayerCrouch;
        playerCrouch.OnPlayerStandUp -= PlayerCrouch_OnPlayerStandUp;
        playerFall.OnPlayerFall -= PlayerFall_OnPlayerFall;
        playerLand.OnPlayerLand -= PlayerLand_OnPlayerLand;

    }

    private void PlayerCrouch_OnPlayerStandUp(object sender, System.EventArgs e)
    {
        Debug.Log("StandUp");
    }

    private void PlayerCrouch_OnPlayerCrouch(object sender, System.EventArgs e)
    {
        Debug.Log("Crouch");
    }

    private void PlayerJump_OnPlayerJump(object sender, System.EventArgs e)
    {
        Debug.Log("Jump");
    }

    private void PlayerFall_OnPlayerFall(object sender, System.EventArgs e)
    {
        Debug.Log("Fall");
    }

    private void PlayerLand_OnPlayerLand(object sender, PlayerLand.OnPlayerLandEventArgs e)
    {
        Debug.Log("Land");
        Debug.Log(e.landHeight);
    }
}
