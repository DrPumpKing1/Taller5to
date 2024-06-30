using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDialogueInput : DialogueInput
{
    private PlayerInputActions playerInputActions;

    protected override void Awake()
    {
        base.Awake();
        InitializePlayerInputActions();
    }

    private void InitializePlayerInputActions()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Dialogues.Enable();
    }

    public override bool CanProcessDialogueInput()
    {
        if (GameManager.Instance.GameState != GameManager.State.OnGameplay) return false;
        if (DialogueManager.Instance._ManagerState == DialogueManager.ManagerState.NotOnDialogue) return false;
        
        return true;
    }

    public override bool GetSkipDown()
    {
        if (!CanProcessDialogueInput()) return false;

        bool dialogueInput = playerInputActions.Dialogues.Skip.WasPerformedThisFrame();
        return dialogueInput;
    }
}
