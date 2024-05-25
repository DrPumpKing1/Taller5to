using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VyrxAttachmentCollider : EventCollider
{
    private const string PLAYER_TAG = "Player";

    protected override void TriggerCollider()
    {
        PetPlayerAttachment.Instance.SetAttachToPlayer();
    }
}
