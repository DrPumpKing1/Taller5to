using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Instruction : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected int id;
    [SerializeField] protected bool hasBeenTriggered;
    [SerializeField, TextArea(3, 10)] protected string instruction;
    [SerializeField] protected int canvasSortingLayer;

    public void SetHasBeenTriggered() => hasBeenTriggered = true;

    public static event EventHandler<OnInstructionTriggeredEventArgs> OnInstructionTriggered;

    public class OnInstructionTriggeredEventArgs : EventArgs
    {
        public string instruction;
        public int canvasSortingLayer;
    }

    public int ID => id;
    public bool HasBeenTriggered => hasBeenTriggered;

    protected void HandleInstructionTrigger()
    {
        if (hasBeenTriggered) return;

        TriggerInstruction();
        hasBeenTriggered = true;
    }

    protected void TriggerInstruction()
    {
        OnInstructionTriggered?.Invoke(this, new OnInstructionTriggeredEventArgs { instruction = instruction, canvasSortingLayer = canvasSortingLayer });
    }
}
