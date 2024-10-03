using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerPositioningHandler : MonoBehaviour
{
    public static PlayerPositioningHandler Instance { get; private set; }

    [Header("Enable Position To Checkpoint")]
    [SerializeField] private bool enablePositionToCheckpoint;

    public static event EventHandler<OnPlayerPositionedEventArgs> OnPlayerStartPositioned;
    public static event EventHandler<OnPlayerPositionedEventArgs> OnPlayerStartPositionedFirstUpdate;

    public static event EventHandler<OnPlayerPositionedEventArgs> OnPlayerInstantPositioned;

    private bool hasStartPositioned;
    private bool hasStartPositionedFirstUpdate;
    private Vector3 desiredStartingPosition;

    public class OnPlayerPositionedEventArgs : EventArgs
    {
        public Vector3 playerPosition;
    }
    private void Awake()
    {
        SetSingleton();
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

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Debug.LogWarning("There is more than one PlayerPositioningHandler instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
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
        OnPlayerStartPositioned?.Invoke(this, new OnPlayerPositionedEventArgs { playerPosition = desiredStartingPosition });
        hasStartPositioned = true;
    }

    public void InstantPositionPlayer(Vector3 desiredPosition)
    {
        transform.position = desiredPosition;
        OnPlayerInstantPositioned?.Invoke(this,new OnPlayerPositionedEventArgs { playerPosition= desiredPosition });
    }

    private void StartPositionFirstUpdate() //Trigger Event For PetPlayerPositioning (Should happen after PetPlayerAttachment.OnVyrxAttachToPlayer)
    {
        if (!hasStartPositioned) return;
        if (hasStartPositionedFirstUpdate) return;

        OnPlayerStartPositionedFirstUpdate?.Invoke(this, new OnPlayerPositionedEventArgs { playerPosition = desiredStartingPosition });
        hasStartPositionedFirstUpdate = true;
    }
}
