using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnPausableSFXManager : SFXManager
{
    private void OnEnable()
    {
        InventoryOpeningManager.OnInventoryOpen += InventoryOpeningManager_OnInventoryOpen;
        InventoryOpeningManager.OnInventoryClose += InventoryOpeningManager_OnInventoryClose;

        PauseManager.OnGamePaused += PauseManager_OnGamePaused;
        PauseManager.OnGameResumed += PauseManager_OnGameResumed;
    }

    private void OnDisable()
    {
        InventoryOpeningManager.OnInventoryOpen += InventoryOpeningManager_OnInventoryOpen;
        InventoryOpeningManager.OnInventoryClose += InventoryOpeningManager_OnInventoryClose;

        PauseManager.OnGamePaused -= PauseManager_OnGamePaused;
        PauseManager.OnGameResumed -= PauseManager_OnGameResumed;
    }

    #region  UI

    private void InventoryOpeningManager_OnInventoryOpen(object sender, System.EventArgs e)
    {
        PlaySound(SFXPoolSO.inventoryOpen);
    }

    private void InventoryOpeningManager_OnInventoryClose(object sender, System.EventArgs e)
    {
        PlaySound(SFXPoolSO.inventoryClosed);
    }

    private void PauseManager_OnGamePaused(object sender, System.EventArgs e)
    {
        PlaySound(SFXPoolSO.pauseOpen);
    }
    private void PauseManager_OnGameResumed(object sender, System.EventArgs e)
    {
        PlaySound(SFXPoolSO.pauseClosed);
    }
    #endregion

}
