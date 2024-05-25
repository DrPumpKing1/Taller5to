using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InstructionCollider : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int instructionsColliderID;
    [SerializeField] private bool hasBeenTriggered;
    [SerializeField,TextArea(3,10)] private string instruction;

    private const string PLAYER_TAG = "Player";

    public void SetHasBeenTriggered() => hasBeenTriggered = true;

    public static event EventHandler<OnInstructionColliderTriggeredEventArgs> OnInstructionColliderTriggered;

    public class OnInstructionColliderTriggeredEventArgs : EventArgs
    {
        public string instruction;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(PLAYER_TAG)) return;
        if (hasBeenTriggered) return;

        OnInstructionColliderTriggered?.Invoke(this, new OnInstructionColliderTriggeredEventArgs { instruction = instruction });
        hasBeenTriggered = true;
    }
}
