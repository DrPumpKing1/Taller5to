using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InstructionsManager : MonoBehaviour
{
    public static InstructionsManager Instance { get; private set; }

    public class UniqueInstruction
    {
        public int id;
        public Instruction instruction;
        public bool hasBeenTriggered;
        public Transform instructionUIPrefab;
    }

    private UniqueInstruction currentInstruction;

    public static event EventHandler<OnInstructionEventArgs> OnInstructionShow;
    public static event EventHandler<OnInstructionEventArgs> OnInstructionHide;

    public class OnInstructionEventArgs : EventArgs
    {
        public UniqueInstruction uniqueInstruction;
    }

    private void OnEnable()
    {
        Instruction.OnInstructionShow += Instruction_OnInstructionShow;
        Instruction.OnInstructionHide += Instruction_OnInstructionHide;       
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one InstructionsManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    #region InstructionSubscriptions
    private void Instruction_OnInstructionShow(object sender, Instruction.OnInstructionEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void Instruction_OnInstructionHide(object sender, Instruction.OnInstructionEventArgs e)
    {
        throw new NotImplementedException();
    }
    #endregion
}
