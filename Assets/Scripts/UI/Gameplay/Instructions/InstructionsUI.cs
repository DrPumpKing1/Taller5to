using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator instructionsUIAnimator;

    [Header("UI Components")]
    [SerializeField] private Transform instructionsContainer;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private void OnEnable()
    {
        InstructionsManager.OnInstructionShow += InstructionsManager_OnInstructionShow;
        InstructionsManager.OnInstructionHide += InstructionsManager_OnInstructionHide;
    }

    private void OnDisable()
    {
        InstructionsManager.OnInstructionShow -= InstructionsManager_OnInstructionShow;
        InstructionsManager.OnInstructionHide -= InstructionsManager_OnInstructionHide;
    }

    private void ShowInstruction(InstructionsManager.UniqueInstruction uniqueInstruction)
    {
        CreateInstruction(uniqueInstruction);

        instructionsUIAnimator.ResetTrigger(HIDE_TRIGGER);
        instructionsUIAnimator.SetTrigger(SHOW_TRIGGER);
    }

    private void HideInstruction()
    {
        instructionsUIAnimator.ResetTrigger(SHOW_TRIGGER);
        instructionsUIAnimator.SetTrigger(HIDE_TRIGGER);
    }

    private void CreateInstruction(InstructionsManager.UniqueInstruction uniqueInstruction)
    {
        Transform instructionsUITransform = Instantiate(uniqueInstruction.instructionUIPrefab, instructionsContainer);
    }

    public void DestroyInstruction()
    {
        foreach (Transform child in instructionsContainer)
        {
            Destroy(child);
        }
    }

    private void InstructionsManager_OnInstructionShow(object sender, InstructionsManager.OnInstructionEventArgs e)
    {
        ShowInstruction(e.uniqueInstruction);
    }

    private void InstructionsManager_OnInstructionHide(object sender, InstructionsManager.OnInstructionEventArgs e)
    {
        HideInstruction();
    }
}
