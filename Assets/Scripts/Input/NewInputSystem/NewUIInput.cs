using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewUIInput : UIInput
{
    private PlayerInputActions playerInputActions;

    [Header("Settings")]
    [SerializeField] private float inputCooldown;

    public float inputCooldownTimer;

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
        return true;
    }

    private bool CanProcessInventoryInput()
    {
        if (PauseManager.Instance.GamePaused) return false;
        if (GameManager.Instance.GameState == GameManager.State.OnForcedDialogue) return false;

        return true;
    }

    public override bool GetPauseDown()
    {
        if (!CanProcessUIInput()) return false;
        if (InputOnCooldown()) return false;

        bool UIInput = playerInputActions.UI.Pause.WasPerformedThisFrame();

        if (UIInput) MaxInputCooldownTimer();

        return UIInput;
    }

    public override bool GetInventoryDown()
    {
        if (!CanProcessUIInput()) return false;
        if (!CanProcessInventoryInput()) return false;
        if (InputOnCooldown()) return false;

        bool UIInput = playerInputActions.UI.Inventory.WasPerformedThisFrame();

        if (UIInput) MaxInputCooldownTimer();

        return UIInput;
    }

    private void HandleInputCooldown()
    {
        if (inputCooldownTimer > 0f) inputCooldownTimer -= Time.unscaledDeltaTime;
    }

    private void MaxInputCooldownTimer() => inputCooldownTimer = inputCooldown;
    private void ResetInputCooldownTimer() => inputCooldownTimer = 0f;
    private bool InputOnCooldown() => inputCooldownTimer > 0f;

}
