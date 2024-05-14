using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointCollider : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int checkpointID;

    private const string PLAYER_TAG = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(PLAYER_TAG)) return;
        CheckpointManager.Instance.ReachCheckpoint(checkpointID);
    }
}
