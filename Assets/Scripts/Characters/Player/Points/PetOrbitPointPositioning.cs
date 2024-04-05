using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetOrbitPointPositioning : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CharacterController characterController;

    [Header("Settings")]
    [SerializeField] private Vector3 positionOffset;

    private void Update()
    {
        HandlePositionDueToCharacterControllerCenter();
    }

    private void HandlePositionDueToCharacterControllerCenter() => transform.localPosition = characterController.center + positionOffset;
}
