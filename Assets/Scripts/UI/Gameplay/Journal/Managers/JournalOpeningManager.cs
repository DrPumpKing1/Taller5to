using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JournalOpeningManager : MonoBehaviour
{
    public static JournalOpeningManager Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private UIInput UIInput;

    [Header("Settings")]
    [SerializeField] private string logToSetCanOpen;
    [SerializeField] private bool canOpenJournal;

    public static event EventHandler OnJournalOpen;
    public static event EventHandler OnJournalClose;

    private bool JournalInput => UIInput.GetJournalDown();

    public bool CanOpenJournal => canOpenJournal;
    public bool JournalOpen { get; private set; }

    private void OnEnable()
    {
        JournalUI.OnCloseFromUI += JournalUI_OnCloseFromUI;
        GameLogManager.OnLogAdd += GameLogManager_OnLogAdd;
    }

    private void OnDisable()
    {
        JournalUI.OnCloseFromUI -= JournalUI_OnCloseFromUI;
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
        CheckOpenCloseJournal();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one JournalOpeningManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeVariables()
    {
        JournalOpen = false;
    }
    public void SetCanOpenJournal(bool canOpen) => canOpenJournal = canOpen;
    private void CheckOpenCloseJournal()
    {
        if (!canOpenJournal) return;
        if (!JournalInput) return;

        if (!JournalOpen)
        {
            if (UIManager.Instance.UIActive) return; //UIManager should not have any layer active
            OpenJournal();
            UIInput.UseInput();
        }
        else
        {
            if (UIManager.Instance.GetUILayersCount() == 1) //If count is 1, the active layer is the InventoryUI, this script should not have a refference to the inventoryUI
            CloseJournal();
            UIInput.UseInput();
        }
    }

    private void OpenJournal()
    {
        OnJournalOpen?.Invoke(this, EventArgs.Empty);
        JournalOpen = true;
    }

    private void CloseJournal()
    {
        OnJournalClose?.Invoke(this, EventArgs.Empty);
        JournalOpen = false;
    }


    #region InventoryUI Subscriptions

    private void JournalUI_OnCloseFromUI(object sender, EventArgs e)
    {
        CloseJournal();
    }
    #endregion

    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        if (e.gameplayAction.log != logToSetCanOpen) return;
        SetCanOpenJournal(true);
    }
}
