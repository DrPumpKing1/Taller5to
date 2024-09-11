using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewUIInput : UIInput
{
    private PlayerInputActions playerInputActions;

    [Header("Settings")]
    [SerializeField] private float inputCooldown;

    private float inputCooldownTimer;

    protected override void Awake()
    {
        base.Awake();
        InitializePlayerInputActions();
    }

    private void Start()
    {
        ResetInputCooldownTimer();
    }

    private void Update()
    {
        HandleInputCooldown();
    }

    private void InitializePlayerInputActions()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.UI.Enable();
    }

    public override bool CanProcessUIInput()
    {
        if (ScenesManager.Instance.SceneState != ScenesManager.State.Idle) return false;

        if (GameManager.Instance.GameState == GameManager.State.OnLost) return false;

        return true;
    }

    private bool CanProcessInventoryInput()
    {
        if (PauseManager.Instance.GamePaused) return false;
        if (DialogueManager.Instance._ManagerState == DialogueManager.ManagerState.ZeroMovementDialogue) return false;

        return true;
    }

    private bool CanProcessJournalInput()
    {
        if (PauseManager.Instance.GamePaused) return false;
        if (DialogueManager.Instance._ManagerState == DialogueManager.ManagerState.ZeroMovementDialogue) return false;

        return true;
    }

    public override bool GetPauseDown()
    {
        if (!CanProcessUIInput()) return false;
        if (InputOnCooldown()) return false;

        bool UIInput = playerInputActions.UI.Pause.WasPerformedThisFrame();

        return UIInput;
    }

    public override bool GetInventoryDown()
    {
        if (!CanProcessUIInput()) return false;
        if (!CanProcessInventoryInput()) return false;
        if (InputOnCooldown()) return false;

        bool UIInput = playerInputActions.UI.Inventory.WasPerformedThisFrame();

        return UIInput;
    }

    public override bool GetJournalDown()
    {
        if (!CanProcessUIInput()) return false;
        if (!CanProcessJournalInput()) return false;
        if (InputOnCooldown()) return false;

        bool UIInput = playerInputActions.UI.Journal.WasPerformedThisFrame();

        return UIInput;
    }

    public override void UseInput() => MaxInputCooldownTimer();

    private void HandleInputCooldown()
    {
        if (inputCooldownTimer > 0f) inputCooldownTimer -= Time.unscaledDeltaTime;
    }
    private void MaxInputCooldownTimer() => inputCooldownTimer = inputCooldown;
    private void ResetInputCooldownTimer() => inputCooldownTimer = 0f;
    private bool InputOnCooldown() => inputCooldownTimer > 0f;

}
