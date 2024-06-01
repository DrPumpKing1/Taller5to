using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStartDirection : MonoBehaviour
{
    [Header("Enable Rotate To Checkpoint")]
    [SerializeField] private bool enableRotateToCheckpoint;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public event EventHandler<OnPlayerStartDirectionedEventArgs> OnPlayerStartDirectioned;

    public class OnPlayerStartDirectionedEventArgs : EventArgs
    {
        public Vector3 startingFacingDirection;
    }

    private void Start()
    {
        DirectionToCheckpoint();
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
        OnPlayerStartDirectioned?.Invoke(this, new OnPlayerStartDirectionedEventArgs { startingFacingDirection = desiredFacingDirection });
    }
}
