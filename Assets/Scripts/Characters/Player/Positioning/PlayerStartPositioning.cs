using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStartPositioning : MonoBehaviour
{
    [Header("Enable Position To Checkpoint")]
    [SerializeField] private bool enablePositionToCheckpoint;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public static event EventHandler OnPlayerStartPositioned;

    public class OnPlayerStartPositionedEventArgs : EventArgs
    {
        public Vector3 playerPosition;
    }

    private void Start()
    {
        PositionToCheckpoint();
    }

    private void PositionToCheckpoint()
    {
        Vector3 position = CheckpointManager.Instance.GetCurrentCheckpointPosition();

        if(position == Vector3.zero)
        {
            if (debug) Debug.LogWarning("Positioning will be ignored due to Vector3.zero position");
        }

        if (enablePositionToCheckpoint) transform.position = position;

        OnPlayerStartPositioned?.Invoke(this, new OnPlayerStartPositionedEventArgs { playerPosition = transform.position });
    }
}
