using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogListenerUI : MonoBehaviour
{
    private void OnEnable()
    {
        //UI
        PauseManager.OnGamePaused += PauseManager_OnGamePaused;
        PauseManager.OnGameResumed += PauseManager_OnGameResumed;

        InventoryOpeningManager.OnInventoryOpen += InventoryOpeningManager_OnInventoryOpen;
        InventoryOpeningManager.OnInventoryClose += InventoryOpeningManager_OnInventoryClose;
    }

    private void OnDisable()
    {
        //UI
        PauseManager.OnGamePaused -= PauseManager_OnGamePaused;
        PauseManager.OnGameResumed -= PauseManager_OnGameResumed;

        InventoryOpeningManager.OnInventoryOpen -= InventoryOpeningManager_OnInventoryOpen;
        InventoryOpeningManager.OnInventoryClose -= InventoryOpeningManager_OnInventoryClose;
    }

    private void PauseManager_OnGamePaused(object sender, System.EventArgs e) => GameLogManager.Instance.Log($"UI/GamePaused");

    private void PauseManager_OnGameResumed(object sender, System.EventArgs e) => GameLogManager.Instance.Log($"UI/GameResumed");

    private void InventoryOpeningManager_OnInventoryOpen(object sender, System.EventArgs e) => GameLogManager.Instance.Log($"UI/InventoryOpened");
    private void InventoryOpeningManager_OnInventoryClose(object sender, System.EventArgs e) => GameLogManager.Instance.Log($"UI/InventoryClosed");

}
