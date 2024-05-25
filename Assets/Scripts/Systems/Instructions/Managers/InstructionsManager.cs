using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsManager : MonoBehaviour
{
    public static InstructionsManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private Transform intructionUIPrefab;

    private void OnEnable()
    {
        Instruction.OnInstructionTriggered += Instruction_OnInstructionTriggered; ;
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

    private void ShowInstruction(string instruction, int canvasLayer)
    {
        Transform instructionsUITransform = Instantiate(intructionUIPrefab, null);

        InstructionsUI instructionsUI = instructionsUITransform.GetComponentInChildren<InstructionsUI>();

        if (!instructionsUI)
        {
            Debug.LogWarning("There's not a InstructionsUI attached to instantiated prefab");
            return;
        }

        instructionsUI.SetInstructionsText(instruction);
        instructionsUI.SetCanvasLayer(canvasLayer);
    }

    #region InstructionCollider Subscriptions
    private void Instruction_OnInstructionTriggered(object sender, Instruction.OnInstructionTriggeredEventArgs e)
    {
        ShowInstruction(e.instruction, e.canvasSortingLayer);
    }

    #endregion
}
