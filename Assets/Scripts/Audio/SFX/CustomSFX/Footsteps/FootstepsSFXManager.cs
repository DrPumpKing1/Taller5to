using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsSFXManager : CustomSFXManager
{
    protected override void OnEnable()
    {
        base.OnEnable();
        PlayerHorizontalMovement.OnPlayerStopMoving += PlayerHorizontalMovement_OnPlayerStopMoving;
        PlayerHorizontalMovement.OnPlayerStartWalking += PlayerHorizontalMovement_OnPlayerStartWalking;
        PlayerHorizontalMovement.OnPlayerStartSprinting += PlayerHorizontalMovement_OnPlayerStartSprinting;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        PlayerHorizontalMovement.OnPlayerStopMoving -= PlayerHorizontalMovement_OnPlayerStopMoving;
        PlayerHorizontalMovement.OnPlayerStartWalking -= PlayerHorizontalMovement_OnPlayerStartWalking;
        PlayerHorizontalMovement.OnPlayerStartSprinting -= PlayerHorizontalMovement_OnPlayerStartSprinting;
    }

    private void PlayerHorizontalMovement_OnPlayerStopMoving(object sender, System.EventArgs e)
    {
        StopAudioSource();
    }
    private void PlayerHorizontalMovement_OnPlayerStartWalking(object sender, System.EventArgs e)
    {
        ReplaceAudioClip(SFXPoolSO.playerWalk);
    }
    private void PlayerHorizontalMovement_OnPlayerStartSprinting(object sender, System.EventArgs e)
    {
        ReplaceAudioClip(SFXPoolSO.playerSprint);
    }
}
