using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventCollider : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";

    [Header("Settings")]
    [SerializeField] protected bool hasBeenTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(PLAYER_TAG)) return;

        HandleColliderTrigger();
    }

    protected virtual void HandleColliderTrigger()
    {
        if (hasBeenTriggered) return;

        TriggerCollider();
        hasBeenTriggered = true;
    }

    protected abstract void TriggerCollider();
}
