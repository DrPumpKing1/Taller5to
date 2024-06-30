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
        playerInputActions.Interaction.Enable();
    }

    public override bool CanProcessInteractionInput()
    {
        if (GameManager.Instance.GameState != GameManager.State.OnGameplay) return false;

        if (DialogueManager.Instance._ManagerState == DialogueManager.ManagerState.ZeroMovementDialogue) return false;

        return true;
    }

    public override bool GetInteractionDown()
    {
        if (!CanProcessInteractionInput()) return false;

        bool interactionInput = playerInputActions.Interaction.Interact.WasPerformedThisFrame();
        return interactionInput;
    }

    public override bool GetInteractionUp()
    {
        if (!CanProcessInteractionInput()) return false;

        bool interactionInput = playerInputActions.Interaction.Interact.WasReleasedThisFrame();
        return interactionInput;
    }

    public override bool GetInteractionHold()
    {
        if (!CanProcessInteractionInput()) return false;

        bool interactionInput = playerInputActions.Interaction.Interact.IsPressed();
        return interactionInput;
    }

    public override bool GetInteractionAlternateDown()
    {
        if (!CanProcessInteractionInput()) return false;

        bool interactionAlternateInput = playerInputActions.Interaction.InteractAlternate.WasPerformedThisFrame();
        return interactionAlternateInput;
    }

    public override bool GetInteractionAlternateUp()
    {
        if (!CanProcessInteractionInput()) return false;

        bool interactionAlternateInput = playerInputActions.Interaction.InteractAlternate.WasReleasedThisFrame();
        return interactionAlternateInput;
    }

    public override bool GetInteractionAlternateHold()
    {
        if (!CanProcessInteractionInput()) return false;

        bool interactionAlternateInput = playerInputActions.Interaction.InteractAlternate.IsPressed();
        return interactionAlternateInput;
    }
}
