using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestMethodSub : MonoBehaviour
{
    [SerializeField] private PlayerLand playerLand;
    [SerializeField] private PlayerFall playerFall;

    private void OnEnable()
    {
        playerFall.OnPlayerFall += PlayerFall_OnPlayerFall;
        playerLand.OnPlayerLand += PlayerLand_OnPlayerLand;
    }

    private void OnDisable()
    {
        playerFall.OnPlayerFall -= PlayerFall_OnPlayerFall;
        playerLand.OnPlayerLand -= PlayerLand_OnPlayerLand;

    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SceneManager.LoadScene(0);
        }
    }
    private void PlayerFall_OnPlayerFall(object sender, System.EventArgs e)
    {
        Debug.Log("Fall");
    }

    private void PlayerLand_OnPlayerLand(object sender, PlayerLand.OnPlayerLandEventArgs e)
    {
        if (e.landHeight <= 0) return;

        Debug.Log("Land");
        Debug.Log(e.landHeight);
    }
}
