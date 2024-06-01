using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStartPositioning : MonoBehaviour
{
    [Header("Enable Position To Checkpoint")]
    [SerializeField] private bool enablePositionToCheckpoint;

    public static event EventHandler OnPlayerStartPositioned;

    public class OnPlayerStartPositionedEventArgs : EventArgs
    {
        public Vector3 playerPosition;
    }

    private void Start()
    {
        StartPositionPlayer();
    }

    private void StartPositionPlayer()
    {
        Vector3 desiredPosition = transform.position;

        if (enablePositionToCheckpoint)
        {
            desiredPosition = CheckpointManager.Instance.GetCurrentCheckpointPosition();   
        }
        
        transform.position = desiredPosition;
        OnPlayerStartPositioned?.Invoke(this, new OnPlayerStartPositionedEventArgs { playerPosition = desiredPosition });
    }
}
