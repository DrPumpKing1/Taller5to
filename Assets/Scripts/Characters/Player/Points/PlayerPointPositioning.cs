using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPointPositioning : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CapsuleCollider capsulleCollider;

    [Header("Settings")]
    [SerializeField] private Vector3 positionOffset;

    private void Update()
    {
        HandlePositionDueToCharacterControllerCenter();
    }

    private void HandlePositionDueToCharacterControllerCenter() => transform.localPosition = capsulleCollider.center + positionOffset;
}
