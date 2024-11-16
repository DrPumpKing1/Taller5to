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

        JournalInfoManager.OnJournalInfoCollected += JournalInfoManager_OnJournalInfoCollected;
        JournalInfoManager.OnJournalInfoChecked += JournalInfoManager_OnJournalInfoChecked;

        WorldSpaceInstructionManager.OnWorldSpaceInstructionAcomplished += WorldSpaceInstructionManager_OnWorldSpaceInstructionAcomplished;
        WorldSpaceInstructionManager.OnWorldSpaceInstructionShow += WorldSpaceInstructionManager_OnWorldSpaceInstructionShow;
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

        JournalInfoManager.OnJournalInfoCollected -= JournalInfoManager_OnJournalInfoCollected;
        JournalInfoManager.OnJournalInfoChecked -= JournalInfoManager_OnJournalInfoChecked;

        WorldSpaceInstructionManager.OnWorldSpaceInstructionAcomplished -= WorldSpaceInstructionManager_OnWorldSpaceInstructionAcomplished;
        WorldSpaceInstructionManager.OnWorldSpaceInstructionShow -= WorldSpaceInstructionManager_OnWorldSpaceInstructionShow;
    }

    private void PauseManager_OnGamePaused(object sender, System.EventArgs e) => GameLogManager.Instance.Log($"UI/GamePaused");
    private void PauseManager_OnGameResumed(object sender, System.EventArgs e) => GameLogManager.Instance.Log($"UI/GameResumed");

    private void InventoryOpeningManager_OnInventoryOpen(object sender, System.EventArgs e) => GameLogManager.Instance.Log($"UI/InventoryOpen");
    private void InventoryOpeningManager_OnInventoryClose(object sender, System.EventArgs e) => GameLogManager.Instance.Log($"UI/InventoryClose");

    private void JournalOpeningManager_OnJournalOpen(object sender, System.EventArgs e) => GameLogManager.Instance.Log($"UI/JournalOpen");
    private void JournalOpeningManager_OnJournalClose(object sender, System.EventArgs e) => GameLogManager.Instance.Log($"UI/JournalClose");
    
    private void JournalInfoManager_OnJournalInfoCollected(object sender, JournalInfoManager.OnJournalInfoEventArgs e)
    {
        GameLogManager.Instance.Log($"Journal/Info/Collected/Any");
        GameLogManager.Instance.Log($"Journal/Info/Collected/{e.journalInfoSO.id}");
    }

    private void JournalInfoManager_OnJournalInfoChecked(object sender, JournalInfoManager.OnJournalInfoEventArgs e)
    {
        GameLogManager.Instance.Log($"Journal/Info/Checked/Any");
        GameLogManager.Instance.Log($"Journal/Info/Checked/{e.journalInfoSO.id}");
    }

    private void WorldSpaceInstructionManager_OnWorldSpaceInstructionShow(object sender, WorldSpaceInstructionManager.OnWorldSpaceInstructionEventArgs e) => GameLogManager.Instance.Log($"UI/WorldSpaceInstruction/Show/{e.id}");
    private void WorldSpaceInstructionManager_OnWorldSpaceInstructionAcomplished(object sender, WorldSpaceInstructionManager.OnWorldSpaceInstructionEventArgs e) => GameLogManager.Instance.Log($"UI/WorldSpaceInstruction/Acomplish/{e.id}");

}
