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

        JournalOpeningManager.OnJournalOpen += JournalOpeningManager_OnJournalOpen;
        JournalOpeningManager.OnJournalClose += JournalOpeningManager_OnJournalClose;
    }

    private void OnDisable()
    {
        //UI
        PauseManager.OnGamePaused -= PauseManager_OnGamePaused;
        PauseManager.OnGameResumed -= PauseManager_OnGameResumed;

        InventoryOpeningManager.OnInventoryOpen -= InventoryOpeningManager_OnInventoryOpen;
        InventoryOpeningManager.OnInventoryClose -= InventoryOpeningManager_OnInventoryClose;

        JournalOpeningManager.OnJournalOpen -= JournalOpeningManager_OnJournalOpen;
        JournalOpeningManager.OnJournalClose -= JournalOpeningManager_OnJournalClose;
    }

    private void PauseManager_OnGamePaused(object sender, System.EventArgs e) => GameLogManager.Instance.Log($"UI/GamePaused");
    private void PauseManager_OnGameResumed(object sender, System.EventArgs e) => GameLogManager.Instance.Log($"UI/GameResumed");

    private void InventoryOpeningManager_OnInventoryOpen(object sender, System.EventArgs e) => GameLogManager.Instance.Log($"UI/InventoryOpen");
    private void InventoryOpeningManager_OnInventoryClose(object sender, System.EventArgs e) => GameLogManager.Instance.Log($"UI/InventoryClose");

    private void JournalOpeningManager_OnJournalOpen(object sender, System.EventArgs e) => GameLogManager.Instance.Log($"UI/JournalOpen");
    private void JournalOpeningManager_OnJournalClose(object sender, System.EventArgs e) => GameLogManager.Instance.Log($"UI/JournalClose");
}
