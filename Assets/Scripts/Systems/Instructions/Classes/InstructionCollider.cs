using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InstructionCollider : Instruction
{
    private const string PLAYER_TAG = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(PLAYER_TAG)) return;
        if (hasBeenTriggered) return;

        TriggerInstruction();
        hasBeenTriggered = true;
    }
}
