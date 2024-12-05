using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewScenesInput : ScenesInput
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
        playerInputActions.Scenes.Enable();
    }

    public override bool CanProcessScenesInput()
    {
        return true;
    }

    public override bool GetSkipHold()
    {
        if (!CanProcessScenesInput()) return false;

        bool dialogueInput = playerInputActions.Scenes.Skip.IsPressed();
        return dialogueInput;
    }
}
