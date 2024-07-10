using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestMethodSub : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerFall.OnPlayerFall += PlayerFall_OnPlayerFall;
        PlayerLand.OnPlayerLand += PlayerLand_OnPlayerLand;

        PlayerLand.OnPlayerSoftLand += PlayerLand_OnPlayerSoftLand;
        PlayerLand.OnPlayerNormalLand += PlayerLand_OnPlayerNormalLand;
        PlayerLand.OnPlayerHardLand += PlayerLand_OnPlayerHardLand;
    }
    private void OnDisable()
    {
        PlayerFall.OnPlayerFall -= PlayerFall_OnPlayerFall;
        PlayerLand.OnPlayerLand -= PlayerLand_OnPlayerLand;

        PlayerLand.OnPlayerSoftLand -= PlayerLand_OnPlayerSoftLand;
        PlayerLand.OnPlayerNormalLand -= PlayerLand_OnPlayerNormalLand;
        PlayerLand.OnPlayerHardLand -= PlayerLand_OnPlayerHardLand;
    }

    private void PlayerFall_OnPlayerFall(object sender, System.EventArgs e)
    {
        Debug.Log("Fall");
    }

    private void PlayerLand_OnPlayerLand(object sender, PlayerLand.OnPlayerLandEventArgs e)
    {
        if (e.landHeight <= 0.01f) return;

        Debug.Log("Land");
        Debug.Log(e.landHeight);
    }

    private void PlayerLand_OnPlayerSoftLand(object sender, System.EventArgs e)
    {
        Debug.Log("SoftLand");
    }
    private void PlayerLand_OnPlayerNormalLand(object sender, System.EventArgs e)
    {
        Debug.Log("NormalLand");

    }
    private void PlayerLand_OnPlayerHardLand(object sender, System.EventArgs e)
    {
        Debug.Log("HardLand");
    }
}
