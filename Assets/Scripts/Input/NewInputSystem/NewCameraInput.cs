using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCameraInput : CameraInput
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
        playerInputActions.Camera.Enable();
    }

    public override bool CanProcessCameraInput()
    {
        if (GameManager.Instance.GameState == GameManager.State.OnGameplay) return true;
        if (GameManager.Instance.GameState == GameManager.State.OnMonologue) return true;
        if (GameManager.Instance.GameState == GameManager.State.OnFreeDialogue) return true;
        if (GameManager.Instance.GameState == GameManager.State.OnRestrictedDialogue) return true;
        if (GameManager.Instance.GameState == GameManager.State.OnForcedDialogue) return true;

        return false;
    }

    public override float GetScroll()
    {
        if (!CanProcessCameraInput()) return 0f;

        float scrollValue = playerInputActions.Camera.Scroll.ReadValue<float>();
        return scrollValue;
    }
}
