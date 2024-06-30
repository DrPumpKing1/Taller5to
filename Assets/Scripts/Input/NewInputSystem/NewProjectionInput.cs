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

    public override bool CanProcessProjectionInput()
    {
        if (GameManager.Instance.GameState != GameManager.State.OnGameplay) return false;

        if (DialogueManager.Instance._ManagerState == DialogueManager.ManagerState.ZeroMovementDialogue) return false;

        return true;
    }

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

    public override bool Get1stProjectableObjectDown()
    {
        if (!CanProcessProjectionInput()) return false;

        bool projectionInput = playerInputActions.Projection._1stProjectableIObject.WasPerformedThisFrame();
        return projectionInput;
    }

    public override bool Get2ndProjectableObjectDown()
    {
        if (!CanProcessProjectionInput()) return false;

        bool projectionInput = playerInputActions.Projection._2ndProjectableObject.WasPerformedThisFrame();
        return projectionInput;
    }

    public override bool Get3rdProjectableObjectDown()
    {
        if (!CanProcessProjectionInput()) return false;

        bool projectionInput = playerInputActions.Projection._3rdProjectableObject.WasPerformedThisFrame();
        return projectionInput;
    }

    public override bool Get4thProjectableObjectDown()
    {
        if (!CanProcessProjectionInput()) return false;

        bool projectionInput = playerInputActions.Projection._4thProjectableObject.WasPerformedThisFrame();
        return projectionInput;
    }

    public override bool Get5thProjectableObjectDown()
    {
        if (!CanProcessProjectionInput()) return false;

        bool projectionInput = playerInputActions.Projection._5thProjectableObject.WasPerformedThisFrame();
        return projectionInput;
    }

    public override bool GetAllProjectableObjectsDematerializationDown()
    {
        if (!CanProcessProjectionInput()) return false;

        bool projectionInput = playerInputActions.Projection.AllProjectableObjectsDematerialization.WasPerformedThisFrame();
        return projectionInput;
    }
}
