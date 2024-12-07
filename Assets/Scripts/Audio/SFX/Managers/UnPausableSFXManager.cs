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

        JournalInfoManager.OnJournalInfoCollected += JournalInfoManager_OnJournalInfoCollected;
        JournalInfoPopUpUI.OnJournalInfoPopUpCloseFromUI += JournalInfoPopUpUI_OnJournalInfoPopUpCloseFromUI;

        AchievementsManager.OnAchievementAchieved += AchievementsManager_OnAchievementAchieved;

        PauseManager.OnGamePaused += PauseManager_OnGamePaused;
        PauseManager.OnGameResumed += PauseManager_OnGameResumed;
    }

    private void OnDisable()
    {
        InventoryOpeningManager.OnInventoryOpen -= InventoryOpeningManager_OnInventoryOpen;
        InventoryOpeningManager.OnInventoryClose -= InventoryOpeningManager_OnInventoryClose;

        JournalOpeningManager.OnJournalOpen -= JournalOpeningManager_OnJournalOpen;
        JournalOpeningManager.OnJournalClose -= JournalOpeningManager_OnJournalClose;

        JournalInfoManager.OnJournalInfoCollected -= JournalInfoManager_OnJournalInfoCollected;
        JournalInfoPopUpUI.OnJournalInfoPopUpCloseFromUI -= JournalInfoPopUpUI_OnJournalInfoPopUpCloseFromUI;

        AchievementsManager.OnAchievementAchieved -= AchievementsManager_OnAchievementAchieved;

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

    #region  Inventory
    private void InventoryOpeningManager_OnInventoryOpen(object sender, System.EventArgs e)
    {
        PlaySound(SFXPoolSO.inventoryOpen);
    }

    private void InventoryOpeningManager_OnInventoryClose(object sender, System.EventArgs e)
    {
        PlaySound(SFXPoolSO.inventoryClose);
    }

    #endregion

    #region Journal

    private void JournalOpeningManager_OnJournalOpen(object sender, System.EventArgs e)
    {
        PlaySound(SFXPoolSO.journalOpen);
    }

    private void JournalOpeningManager_OnJournalClose(object sender, System.EventArgs e)
    {
        PlaySound(SFXPoolSO.journalClose);
    }
    private void JournalInfoManager_OnJournalInfoCollected(object sender, JournalInfoManager.OnJournalInfoEventArgs e)
    {
        PlaySound(SFXPoolSO.journalInfoCollected);
    }

    private void JournalInfoPopUpUI_OnJournalInfoPopUpCloseFromUI(object sender, JournalInfoPopUpUI.OnJournalInfoPopUpEventArgs e)
    {
        PlaySound(SFXPoolSO.journalButtonClick4);
    }

    #endregion

    #region Achievements
    private void AchievementsManager_OnAchievementAchieved(object sender, AchievementsManager.OnAchievementAchievedEventArgs e)
    {
        PlaySound(SFXPoolSO.achievementAchieved);
    }
    #endregion

    #region Pause
    private void PauseManager_OnGamePaused(object sender, System.EventArgs e)
    {
        PlaySound(SFXPoolSO.pauseOpen);
    }
    private void PauseManager_OnGameResumed(object sender, System.EventArgs e)
    {
        PlaySound(SFXPoolSO.pauseClose);
    }
    #endregion

    #region Regular Buttons
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

    #region Journal Buttons
    public void PlaySFXJournalButtonClick1()
    {
        PlaySound(SFXPoolSO.journalButtonClick1);
    }

    public void PlaySFXJournalButtonClick2()
    {
        PlaySound(SFXPoolSO.journalButtonClick2);
    }

    public void PlaySFXJournalButtonClick3()
    {
        PlaySound(SFXPoolSO.journalButtonClick3);
    }

    public void PlaySFXJournalButtonClick4()
    {
        PlaySound(SFXPoolSO.journalButtonClick4);
    }
    #endregion
}
