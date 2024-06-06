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
        if (GameManager.Instance.GameState == GameManager.State.OnFreeDialogue) return true;
        if (GameManager.Instance.GameState == GameManager.State.OnRestrictedDialogue) return true;
        if (GameManager.Instance.GameState == GameManager.State.OnForcedDialogue) return true;

        return false;
    }

    public override bool GetSkipDown()
    {
        if (!CanProcessDialogueInput()) return false;

        bool dialogueInput = playerInputActions.Dialogues.Skip.WasPerformedThisFrame();
        return dialogueInput;
    }
}
