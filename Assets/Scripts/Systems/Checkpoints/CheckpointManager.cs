using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    [Header("Checkpoints Settings")]
    [SerializeField] private int currentCheckpointID;
    [SerializeField] private List<Checkpoint> checkpointList = new List<Checkpoint>();

    [Header("Debug")]
    [SerializeField] private bool debug;
    public int CurrentCheckpointID => currentCheckpointID;

    public static event EventHandler<OnCheckpointReachedEventArgs> OnCheckpointReached;

    [Serializable]
    public class Checkpoint
    {
        public int id;
        public Transform checkpointTransform;
        public Vector2 checkpointDirection;
    }

    public class OnCheckpointReachedEventArgs : EventArgs
    {
        public Checkpoint checkpoint;
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
            Debug.LogWarning("There is more than one CheckpointManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public void SetCheckpointID(int checkpointID) => currentCheckpointID = checkpointID;

    public void ReachCheckpoint(int checkpointID)
    {
        Checkpoint checkpointReached = GetCheckpointByCheckpointID(checkpointID);

        if (checkpointReached == null)
        {
            if (debug) Debug.LogWarning("Checkpoint reach will be ignored due to checkpoint not found");
            return;
        }

        currentCheckpointID = checkpointID;

        OnCheckpointReached?.Invoke(this, new OnCheckpointReachedEventArgs { checkpoint = checkpointReached });
    }

    private Checkpoint GetCheckpointByCheckpointID(int checkpointID)
    {
        foreach(Checkpoint checkpoint in checkpointList)
        {
            if (checkpoint.id == checkpointID) return checkpoint;
        }

        if (debug) Debug.LogWarning($"The checkpoint with id {checkpointID} cannot be found in checkpointList");
        return null;
    }

    public Vector3 GetCurrentCheckpointPosition()
    {
        Checkpoint checkpoint = GetCheckpointByCheckpointID(currentCheckpointID);

        if (checkpoint == null)
        {
            if (debug) Debug.LogWarning("GetCheckpointPosition will be ignored due to checkpoint not found");
            return Vector3.zero;
        }

        return checkpoint.checkpointTransform.position;
    }

    public Vector2 GetCurrentCheckpointDirection()
    {
        Checkpoint checkpoint = GetCheckpointByCheckpointID(currentCheckpointID);

        if (checkpoint == null)
        {
            if (debug) Debug.LogWarning("GetCheckpointDirection will be ignored due to checkpoint not found");
            return Vector2.zero;
        }

        return checkpoint.checkpointDirection;
    }
}
