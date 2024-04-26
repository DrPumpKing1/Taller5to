using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewProjectionInput : ProjectionInput
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
        playerInputActions.Projection.Enable();
    }

    public override bool CanProcessProjectionInput() => GameManager.Instance.GameState == GameManager.State.OnGameplay;

    public override bool GetNextProjectableObjectDown()
    {
        if (!CanProcessProjectionInput()) return false;

        bool projectionInput = playerInputActions.Projection.NextProjectableObject.WasPerformedThisFrame();
        return projectionInput;
    }

    public override bool GetPreviousProjectableObjectDown()
    {
        if (!CanProcessProjectionInput()) return false;

        bool projectionInput = playerInputActions.Projection.PreviousProjectableObject.WasPerformedThisFrame();
        return projectionInput;
    }
}
