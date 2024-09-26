using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStartPositioning : MonoBehaviour
{
    [Header("Enable Position To Checkpoint")]
    [SerializeField] private bool enablePositionToCheckpoint;

    public static event EventHandler OnPlayerStartPositioned;
    public static event EventHandler OnPlayerStartPositionedFirstUpdate;

    private bool hasStartPositioned;
    private bool hasStartPositionedFirstUpdate;
    private Vector3 desiredStartingPosition;

    public class OnPlayerStartPositionedEventArgs : EventArgs
    {
        public Vector3 playerPosition;
    }

    private void Start()
    {
        InitializeVariables();
        StartPositionPlayer();
    }

    private void Update()
    {
        StartPositionFirstUpdate();
    }

    private void InitializeVariables()
    {
        hasStartPositioned = false;
    }

    private void StartPositionPlayer()
    {
        desiredStartingPosition = transform.position;

        if (enablePositionToCheckpoint)
        {
            desiredStartingPosition = CheckpointManager.Instance.GetCurrentCheckpointPosition();   
        }
        
        transform.position = desiredStartingPosition;
        OnPlayerStartPositioned?.Invoke(this, new OnPlayerStartPositionedEventArgs { playerPosition = desiredStartingPosition });
        hasStartPositioned = true;
    }

    private void StartPositionFirstUpdate() //Trigger Event For PetPlayerPositioning (Should happen after PetPlayerAttachment.OnVyrxAttachToPlayer)
    {
        if (!hasStartPositioned) return;
        if (hasStartPositionedFirstUpdate) return;

        OnPlayerStartPositionedFirstUpdate?.Invoke(this, new OnPlayerStartPositionedEventArgs { playerPosition = desiredStartingPosition });
        hasStartPositionedFirstUpdate = true;
    }
}
