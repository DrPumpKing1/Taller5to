using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointPersistence : MonoBehaviour, IDataPersistence<PlayerData>
{
    public void LoadData(PlayerData data)
    {
        CheckpointManager checkpointManager = FindObjectOfType<CheckpointManager>();

        if (data.checkpointID == 0) return; //If its 0, it means PlayerData has been initialized as a new(), should avoid SetCheckpointID to use the checkpointID set in the inspector of CheckpointManager

        checkpointManager.SetCheckpointID(data.checkpointID);
    }

    public void SaveData(ref PlayerData data)
    {
        CheckpointManager checkpointManager = FindObjectOfType<CheckpointManager>();

        data.checkpointID = checkpointManager.CurrentCheckpointID;
    }
}
