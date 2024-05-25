using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ColliderInstruction : Instruction
{
    private const string PLAYER_TAG = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(PLAYER_TAG)) return;

        HandleInstructionTrigger();
    }
}
