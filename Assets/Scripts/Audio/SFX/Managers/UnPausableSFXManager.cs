using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnPausableSFXManager : SFXManager
{
    public static UnPausableSFXManager Instance { get; private set; }
    private void OnEnable()
    {
        InventoryOpeningManager.OnInventoryOpen += InventoryOpeningManager_OnInventoryOpen;
        InventoryOpeningManager.OnInventoryClose += InventoryOpeningManager_OnInventoryClose;

        JournalOpeningManager.OnJournalOpen += JournalOpeningManager_OnJournalOpen;
        JournalOpeningManager.OnJournalClose += JournalOpeningManager_OnJournalClose;

        PauseManager.OnGamePaused += PauseManager_OnGamePaused;
        PauseManager.OnGameResumed += PauseManager_OnGameResumed;
    }

    private void OnDisable()
    {
        InventoryOpeningManager.OnInventoryOpen -= InventoryOpeningManager_OnInventoryOpen;
        InventoryOpeningManager.OnInventoryClose -= InventoryOpeningManager_OnInventoryClose;

        JournalOpeningManager.OnJournalOpen -= JournalOpeningManager_OnJournalOpen;
        JournalOpeningManager.OnJournalClose -= JournalOpeningManager_OnJournalClose;

        PauseManager.OnGamePaused -= PauseManager_OnGamePaused;
        PauseManager.OnGameResumed -= PauseManager_OnGameResumed;
    }

    protected override void Awake()
    {
        base.Awake();
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Debug.LogWarning("There is more than one UnPausableSFXManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    #region  UI
    private void InventoryOpeningManager_OnInventoryOpen(object sender, System.EventArgs e)
    {
        PlaySound(SFXPoolSO.inventoryOpen);
    }

    private void InventoryOpeningManager_OnInventoryClose(object sender, System.EventArgs e)
    {
        PlaySound(SFXPoolSO.inventoryClose);
    }

    private void JournalOpeningManager_OnJournalOpen(object sender, System.EventArgs e)
    {
        PlaySound(SFXPoolSO.journalOpen);
    }

    private void JournalOpeningManager_OnJournalClose(object sender, System.EventArgs e)
    {
        PlaySound(SFXPoolSO.journalClose);
    }

    private void PauseManager_OnGamePaused(object sender, System.EventArgs e)
    {
        PlaySound(SFXPoolSO.pauseOpen);
    }
    private void PauseManager_OnGameResumed(object sender, System.EventArgs e)
    {
        PlaySound(SFXPoolSO.pauseClose);
    }
    #endregion

    #region Buttons
    public void PlaySFXButtonClick1()
    {
        PlaySound(SFXPoolSO.buttonClick1);
    }

    public void PlaySFXButtonClick2()
    {
        PlaySound(SFXPoolSO.buttonClick2);
    }

    public void PlaySFXButtonClick3()
    {
        PlaySound(SFXPoolSO.buttonClick3);
    }
    #endregion
}
