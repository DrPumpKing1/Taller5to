using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerDirectionHandler : MonoBehaviour
{
    public static PlayerDirectionHandler Instance { get; private set; }

    [Header("Enable Rotate To Checkpoint")]
    [SerializeField] private bool enableRotateToCheckpoint;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public static event EventHandler<OnPlayerDirectionedEventArgs> OnPlayerStartDirectioned;
    public static event EventHandler<OnPlayerDirectionedEventArgs> OnPlayerInstantDirectioned;

    public class OnPlayerDirectionedEventArgs : EventArgs
    {
        public Vector3 facingDirection;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        DirectionToCheckpoint();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Debug.LogWarning("There is more than one PlayerDirectionHandler instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void DirectionToCheckpoint()
    {
        Vector3 desiredFacingDirection = transform.forward;

        if (enableRotateToCheckpoint)
        {
            Vector2 checkpointDirection = CheckpointManager.Instance.GetCurrentCheckpointDirection();
            Vector3 checkpointFacingDirection = GeneralMethods.Vector2ToVector3(checkpointDirection);

            if(!(checkpointDirection == Vector2.zero))
            {
                desiredFacingDirection = checkpointFacingDirection;
            }
            else
            {
                if (debug) Debug.LogWarning("Rotation will be ignored due to checkpoint direction being Vector2.zero");
            }

        }

        transform.localRotation = Quaternion.LookRotation(desiredFacingDirection);
        OnPlayerStartDirectioned?.Invoke(this, new OnPlayerDirectionedEventArgs { facingDirection = desiredFacingDirection });
    }

    public void InstantDirectionPlayer(Vector2 desiredDirection)
    {
        Vector3 desiredFacingDirection = GeneralMethods.Vector2ToVector3(desiredDirection);
        transform.localRotation = Quaternion.LookRotation(desiredFacingDirection);
        OnPlayerInstantDirectioned?.Invoke(this, new OnPlayerDirectionedEventArgs { facingDirection = desiredFacingDirection});
    }
}
