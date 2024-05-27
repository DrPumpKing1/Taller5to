using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewUIInput : UIInput
{
    [Header("Components")]
    [SerializeField] private CheckGround checkGround;

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

    public override bool CanProcessUIInput()
    {
        if (GameManager.Instance.GameState == GameManager.State.OnDeath) return false;
        return true;
    }

    private bool CanProcessInventoryInput()
    {
        if (PauseManager.Instance.GamePaused) return false;
        //if (!checkGround.IsGrounded) return false;
        return true;
    }

    public override bool GetPauseDown()
    {
        if (!CanProcessUIInput()) return false;

        bool UIInput = playerInputActions.UI.Pause.WasPerformedThisFrame();
        return UIInput;
    }

    public override bool GetInventoryDown()
    {
        if (!CanProcessUIInput()) return false;
        if (!CanProcessInventoryInput()) return false;

        bool UIInput = playerInputActions.UI.Inventory.WasPerformedThisFrame();
        return UIInput;
    }

}
