using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewInteractionInput : InteractionInput
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
        playerInputActions.Player.Enable();
    }

    public override bool CanProcessInteractionInput() => true;

    public override bool GetInteractionDown()
    {
        if (!CanProcessInteractionInput()) return false;

        bool interactionInput = playerInputActions.Player.Interact.WasPerformedThisFrame();
        return interactionInput;
    }

    public override bool GetInteractionUp()
    {
        if (!CanProcessInteractionInput()) return false;

        bool interactionInput = playerInputActions.Player.Interact.WasReleasedThisFrame();
        return interactionInput;
    }

    public override bool GetInteractionHold()
    {
        if (!CanProcessInteractionInput()) return false;

        bool interactionInput = playerInputActions.Player.Interact.IsPressed();
        return interactionInput;
    }

    public override bool GetInteractionAlternateDown()
    {
        if (!CanProcessInteractionInput()) return false;

        bool interactionAlternateInput = playerInputActions.Player.InteractAlternate.WasPerformedThisFrame();
        return interactionAlternateInput;
    }

    public override bool GetInteractionAlternateUp()
    {
        if (!CanProcessInteractionInput()) return false;

        bool interactionAlternateInput = playerInputActions.Player.InteractAlternate.WasReleasedThisFrame();
        return interactionAlternateInput;
    }

    public override bool GetInteractionAlternateHold()
    {
        if (!CanProcessInteractionInput()) return false;

        bool interactionAlternateInput = playerInputActions.Player.InteractAlternate.IsPressed();
        return interactionAlternateInput;
    }
}
