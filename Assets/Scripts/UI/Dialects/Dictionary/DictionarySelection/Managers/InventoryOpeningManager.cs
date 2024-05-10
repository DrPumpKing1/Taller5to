using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryOpeningManager : MonoBehaviour
{
    public static InventoryOpeningManager Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private UIInput UIInput;

    public static event EventHandler OnInventoryOpen;
    public static event EventHandler OnInventoryClose;

    private bool InventoryInput => UIInput.GetInventoryDown();

    public bool InventoryOpen { get; private set; }

    private void OnEnable()
    {
        InventoryUI.OnCloseFromUI += InventoryUI_OnCloseFromUI;
    }

    private void OnDisable()
    {
        InventoryUI.OnCloseFromUI -= InventoryUI_OnCloseFromUI;
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
    private void CheckOpenCloseInventory()
    {
        if (!InventoryInput) return;

        if (!InventoryOpen)
        {
            if (UIManager.Instance.UIActive) return; //UIManager should not have any layer active
            OpenInventory();
        }
        else    
        {
            if (UIManager.Instance.GetUILayersCount() == 1) //If count is 1, the active layer is the InventoryUI, this script should not have a refference to the inventoryUI
            CloseInventory();
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

    #region DictionarySelectionUI Subscriptions

    private void InventoryUI_OnCloseFromUI(object sender, EventArgs e)
    {
        CloseInventory();
    }
    #endregion
}
