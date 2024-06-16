using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InstructionsManager : MonoBehaviour
{
    public static InstructionsManager Instance { get; private set; }

    [Header("Unique Instructions")]
    [SerializeField] private List<UniqueInstruction> uniqueInstructions = new List<UniqueInstruction>();

    [Header("Debug")]
    [SerializeField] private bool debug;

    [Serializable]
    public class UniqueInstruction
    {
        public int id;
        public Instruction instruction;
        public bool hasBeenShown;
        public Transform instructionUIPrefab;
    }

    private UniqueInstruction currentInstruction;

    public static event EventHandler<OnInstructionEventArgs> OnInstructionShow;
    public static event EventHandler<OnInstructionEventArgs> OnInstructionReplace;
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

    private void OnDisable()
    {
        Instruction.OnInstructionShow -= Instruction_OnInstructionShow;
        Instruction.OnInstructionHide -= Instruction_OnInstructionHide;
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

    private void CheckInstructionToShow(Instruction instruction)
    {
        if (currentInstruction != null)
        {
            ReplaceInstruction(instruction);
            return;
        }

        ShowInstruction(instruction);
    }

    private void ShowInstruction(Instruction instruction)
    {
        foreach (UniqueInstruction uniqueInstruction in uniqueInstructions)
        {
            if (uniqueInstruction.instruction != instruction) continue;

            OnInstructionShow?.Invoke(this, new OnInstructionEventArgs { uniqueInstruction = uniqueInstruction });
            currentInstruction = uniqueInstruction;
            uniqueInstruction.hasBeenShown = true;
            return;
        }

        if (debug) Debug.Log("No instruction in UniqueInstructionsList matches instruction");
    }

    private void ReplaceInstruction(Instruction instruction)
    {
        foreach (UniqueInstruction uniqueInstruction in uniqueInstructions)
        {
            if (uniqueInstruction.instruction != instruction) continue;

            OnInstructionReplace?.Invoke(this, new OnInstructionEventArgs { uniqueInstruction = uniqueInstruction });
            currentInstruction = uniqueInstruction;
            uniqueInstruction.hasBeenShown = true;
            return;
        }

        if (debug) Debug.Log("No instruction in UniqueInstructionsList matches instruction");
    }

    private void CheckInstructionToHide(Instruction instruction)
    {
        foreach (UniqueInstruction uniqueInstruction in uniqueInstructions)
        {
            if (uniqueInstruction.instruction != instruction) continue;

            if (uniqueInstruction != currentInstruction)
            {
                if (debug) Debug.Log("Instruction to hide is not current instruction");
                return;
            }

            OnInstructionHide?.Invoke(this, new OnInstructionEventArgs { uniqueInstruction = uniqueInstruction });
            return;
        }

        if (debug) Debug.Log("No instruction in UniqueInstructionsList matches instruction");
    }

    #region InstructionSubscriptions
    private void Instruction_OnInstructionShow(object sender, Instruction.OnInstructionEventArgs e)
    {
        CheckInstructionToShow(e.instruction);
    }

    private void Instruction_OnInstructionHide(object sender, Instruction.OnInstructionEventArgs e)
    {
        CheckInstructionToHide(e.instruction);
    }
    #endregion
}
