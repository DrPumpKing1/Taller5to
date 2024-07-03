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

    [Serializable]
    public class LevelSettings
    {
        public int checkpointID;
        public List<ProjectableObjectSO> projectableObjectsSOs;
        public List<ShieldPieceSO> shieldPiecesSOs;
        public List<int> dialoguesIDsTriggered;
        public List<int> monologuesIDsTriggered;
        public int projectionGems;
        public bool canOpenInventory;
        public bool HUDVisible;
        public bool attachToPlayer;
    }

    private void Awake()
    {
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
            Debug.LogWarning("There is more than one LevelSkipManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public void SkipLevel(int id)
    {
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

        ProjectionGemsManager.Instance.SetTotalProjectionGems(levelSettings.projectionGems);
        InventoryOpeningManager.Instance.SetCanOpenInventory(levelSettings.canOpenInventory);
        HUDVisibilityHandler.Instance.SetIsVisible(levelSettings.HUDVisible);
        PetPlayerAttachment.Instance.SetAttachToPlayer(levelSettings.attachToPlayer);

        UniqueDialogueTriggerHandler.Instance.SetUniqueDialoguesTriggered(levelSettings.dialoguesIDsTriggered);
        UniqueMonologueTriggerHandler.Instance.SetUniqueMonologuesTriggered(levelSettings.monologuesIDsTriggered);


        PlayerDataPersistenceManager.Instance.SaveGameData();
        PetDataPersistenceManager.Instance.SaveGameData();
        ObjectsDataPersistenceManager.Instance.SaveGameData();
        UIDataPersistenceManager.Instance.SaveGameData();
        LogDataPersistenceManager.Instance.SaveGameData();

        ScenesManager.Instance.FadeReloadCurrentScene();
    }
}
