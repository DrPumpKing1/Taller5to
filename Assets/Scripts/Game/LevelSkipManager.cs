using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class LevelSkipManager : MonoBehaviour
{
    public static LevelSkipManager Instance { get; private set; }

    [Header("Level1 Settings")]
    [SerializeField] private LevelSettings level1Settings;

    [Header("Level2 Settings")]
    [SerializeField] private LevelSettings level2Settings;

    [Header("Level3 Settings")]
    [SerializeField] private LevelSettings level3Settings;

    [Header("Boss Settings")]
    [SerializeField] private LevelSettings bossSettings;

    private bool skippingLevel;

    [Serializable]
    public class LevelSettings
    {
        public int checkpointID;
        public List<ProjectableObjectSO> projectableObjectsSOs;
        public List<ShieldPieceSO> shieldPiecesSOs;
        public List<int> dialoguesIDsTriggered;
        public List<int> monologuesIDsTriggered;
        public List<int> learningPlatformsUsedIDs;
        public List<int> switchesToggledIDs;
        public int projectionGems;
        public bool canOpenInventory;
        public bool HUDVisible;
        public bool attachToPlayer;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        skippingLevel = false;
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one LevelSkipManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public void SkipLevel(int id)
    {
        if (skippingLevel) return;

        LevelSettings levelSettings;

        switch (id)
        {
            case 1:
                levelSettings = level1Settings;
                break;
            case 2:
                levelSettings = level2Settings;
                break;
            case 3:
                levelSettings = level3Settings;
                break;
            case 4:
                levelSettings = bossSettings;
                break;
            default:
                levelSettings = level1Settings;
                break;
        }

        ReloadWithSettings(levelSettings);
    }

    private void ReloadWithSettings(LevelSettings levelSettings)
    {
        CheckpointManager.Instance.SetCheckpointID(levelSettings.checkpointID);

        ProjectableObjectsLearningManager.Instance.ReplaceProjectableObjectsList(levelSettings.projectableObjectsSOs);
        ShieldPiecesManager.Instance.ReplaceShieldPiecesCollectedList(levelSettings.shieldPiecesSOs);

        SetSwitchesToggled(levelSettings.switchesToggledIDs);
        SetLearningPlatformsUsed(levelSettings.learningPlatformsUsedIDs);

        ProjectionGemsManager.Instance.SetTotalProjectionGems(levelSettings.projectionGems);
        InventoryOpeningManager.Instance.SetCanOpenInventory(levelSettings.canOpenInventory);
        HUDVisibilityHandler.Instance.SetIsVisible(levelSettings.HUDVisible);
        PetPlayerAttachment.Instance.SetAttachToPlayer(levelSettings.attachToPlayer);

        UniqueDialogueTriggerHandler.Instance.SetUniqueDialoguesTriggered(levelSettings.dialoguesIDsTriggered);
        UniqueMonologueTriggerHandler.Instance.SetUniqueMonologuesTriggered(levelSettings.monologuesIDsTriggered);

        //

        PlayerDataPersistenceManager.Instance.SaveGameData();
        PetDataPersistenceManager.Instance.SaveGameData();
        ObjectsDataPersistenceManager.Instance.SaveGameData();
        UIDataPersistenceManager.Instance.SaveGameData();
        LogDataPersistenceManager.Instance.SaveGameData();

        ScenesManager.Instance.FadeReloadCurrentScene();
    }

    private void SetLearningPlatformsUsed(List<int> learningPlatformsUsedIDs)
    {
        LearningPlatform[] learningPlatforms = FindObjectsOfType<LearningPlatform>();

        foreach (LearningPlatform learningPlatform in learningPlatforms)
        {
            foreach (int learningPlatformUsedID in learningPlatformsUsedIDs)
            {
                if (learningPlatform.LearningPlatformSO.id == learningPlatformUsedID)
                {
                    learningPlatform.SetIsLearned(true);
                }
            }
        }
    }

    private void SetSwitchesToggled(List<int> switchesToggledIDs)
    {
        ElectricalSwitch[] electricalSwitches = FindObjectsOfType<ElectricalSwitch>();

        foreach (ElectricalSwitch electricalSwitch in electricalSwitches)
        {
            foreach (int switchToggledID in switchesToggledIDs)
            {
                if (electricalSwitch.ID == switchToggledID)
                {
                    electricalSwitch.SetIsOn(true);
                }
            }
        }
    }
}
