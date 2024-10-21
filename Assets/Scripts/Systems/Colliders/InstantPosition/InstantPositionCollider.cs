using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantPositionCollider : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform instantPositionPlace;

    private const string PLAYER_TAG = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(PLAYER_TAG)) return;

        InstantPositionPlayer();
    }

    private void InstantPositionPlayer() => PlayerPositioningHandler.Instance.InstantPositionPlayer(instantPositionPlace.position);
}
