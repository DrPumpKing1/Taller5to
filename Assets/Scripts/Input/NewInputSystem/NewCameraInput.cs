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
        if (GameManager.Instance.GameState == GameManager.State.OnUI) return false;

        if (ScenesManager.Instance.SceneState == ScenesManager.State.TransitionOut) return false;
        if (ScenesManager.Instance.SceneState == ScenesManager.State.FullBlack) return false;

        return true;
    }

    public override float GetScroll()
    {
        if (!CanProcessCameraInput()) return 0f;

        float scrollValue = playerInputActions.Camera.Scroll.ReadValue<float>();
        return scrollValue;
    }
}
