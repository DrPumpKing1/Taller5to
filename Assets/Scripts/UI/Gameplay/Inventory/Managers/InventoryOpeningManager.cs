using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryOpeningManager : MonoBehaviour
{
    public static InventoryOpeningManager Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private UIInput UIInput;

    [Header("Settings")]
    [SerializeField] private string logToSetCanOpen;
    [SerializeField] private bool canOpenInventory;

    public static event EventHandler OnInventoryOpen;
    public static event EventHandler OnInventoryClose;

    private bool InventoryInput => UIInput.GetInventoryDown();

    public bool CanOpenInventory => canOpenInventory;
    public bool InventoryOpen { get; private set; }

    private void OnEnable()
    {
        InventoryUI.OnCloseFromUI += InventoryUI_OnCloseFromUI;
        GameLogManager.OnLogAdd += GameLogManager_OnLogAdd;
    }

    private void OnDisable()
    {
        InventoryUI.OnCloseFromUI -= InventoryUI_OnCloseFromUI;
        GameLogManager.OnLogAdd -= GameLogManager_OnLogAdd;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void Update()
    {
        CheckOpenCloseInventory();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one InventoryOpeningManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeVariables()
    {
        InventoryOpen = false;
    }
    public void SetCanOpenInventory(bool canOpen) => canOpenInventory = canOpen;
    private void CheckOpenCloseInventory()
    {
        if (!canOpenInventory) return;
        if (!InventoryInput) return;

        if (!InventoryOpen)
        {
            if (UIManager.Instance.UIActive) return; //UIManager should not have any layer active
            OpenInventory();
            UIInput.UseInput();
        }
        else    
        {
            if (UIManager.Instance.GetUILayersCount() == 1) //If count is 1, the active layer is the InventoryUI, this script should not have a refference to the inventoryUI
            CloseInventory();
            UIInput.UseInput();
        }
    }

    private void OpenInventory()
    {
        OnInventoryOpen?.Invoke(this, EventArgs.Empty);
        InventoryOpen = true;
    }

    private void CloseInventory()
    {
        OnInventoryClose?.Invoke(this, EventArgs.Empty);
        InventoryOpen = false;
    }


    #region InventoryUI Subscriptions

    private void InventoryUI_OnCloseFromUI(object sender, EventArgs e)
    {
        CloseInventory();
    }
    #endregion

    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        if (e.gameplayAction.log != logToSetCanOpen) return;
        SetCanOpenInventory(true);
    }
}
