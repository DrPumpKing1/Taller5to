using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventCollider : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected int id;
    [SerializeField] protected bool hasBeenTriggered;

    private const string PLAYER_TAG = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(PLAYER_TAG)) return;

        HandleColliderTrigger();
    }

    protected void HandleColliderTrigger()
    {
        if (hasBeenTriggered) return;

        TriggerCollider();
        hasBeenTriggered = true;
    }

    protected abstract void TriggerCollider();
}