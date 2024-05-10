using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewUIInput : UIInput
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
        playerInputActions.UI.Enable();
    }

    public override bool CanProcessUIInput() => true;

    public override bool GetPauseDown()
    {
        if (!CanProcessUIInput()) return false;

        bool UIInput = playerInputActions.UI.Pause.WasPerformedThisFrame();
        return UIInput;
    }

    public override bool GetInventoryDown()
    {
        if (!CanProcessUIInput()) return false;

        bool UIInput = playerInputActions.UI.Dictionary.WasPerformedThisFrame();
        return UIInput;
    }

}
