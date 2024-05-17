using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStartDirection : MonoBehaviour
{
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
        Vector2 direction = CheckpointManager.Instance.GetCurrentCheckpointDirection();
        Vector3 startingFacingDirection = GeneralMethods.Vector2ToVector3(direction);

        if (direction == Vector2.zero)
        {
            if (debug) Debug.LogWarning("Positioning will be ignored due to Vector3.zero position");
            startingFacingDirection = transform.forward;
        }

        transform.localRotation = Quaternion.LookRotation(startingFacingDirection);
        OnPlayerStartDirectioned?.Invoke(this, new OnPlayerStartDirectionedEventArgs { startingFacingDirection = startingFacingDirection });
    }
}
