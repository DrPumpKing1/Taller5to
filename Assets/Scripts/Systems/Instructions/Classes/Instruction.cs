using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public abstract class Instruction : MonoBehaviour
{
    [Header("Booleans")]
    [SerializeField] protected bool hasBeenAcomplished;
    [SerializeField] protected bool isShowing;

    public static event EventHandler<OnInstructionEventArgs> OnInstructionShow;
    public static event EventHandler<OnInstructionEventArgs> OnInstructionHide;

    public class OnInstructionEventArgs
    {
        public Instruction instruction;
    }

    private void Start()
    {
        InitializeVariables();
    }

    protected virtual void Update()
    {
        CheckShouldHide();
    }

    private void InitializeVariables()
    {
        hasBeenAcomplished = false;
        isShowing = false;
    }

    protected void ShowInstruction()
    {
        OnInstructionShow?.Invoke(this, new OnInstructionEventArgs { instruction = this });
        isShowing = true;
    }
    protected void HideInstruction()
    {
        OnInstructionHide?.Invoke(this, new OnInstructionEventArgs { instruction = this });
        isShowing = false;
    }

    protected void CheckShouldShow()
    {
        if (hasBeenAcomplished) return;
        if (isShowing) return;

        ShowInstruction();
    }

    protected void CheckShouldHide()
    {
        if (!isShowing) return;
        if (!hasBeenAcomplished) return;

        HideInstruction();
    }
}
