using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogDataPersistenceManager : DataPersistenceManager<LogData>
{
    public static LogDataPersistenceManager Instance { get; private set; }

    private void OnEnable()
    {
        CheckpointManager.OnCheckpointReached += CheckpointManager_OnCheckpointReached;
    }
    private void OnDisable()
    {
        CheckpointManager.OnCheckpointReached -= CheckpointManager_OnCheckpointReached;
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one LogDataPersistenceManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    #region CheckpointManager Subscriptions

    private void CheckpointManager_OnCheckpointReached(object sender, CheckpointManager.OnCheckpointReachedEventArgs e)
    {
        SaveGameData();
    }
    #endregion
}

