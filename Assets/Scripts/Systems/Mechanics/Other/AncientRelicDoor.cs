using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AncientRelicDoor : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform transformToDisable;

    [Header("Settings")]
    [SerializeField] private List<ProjectionPlatform> controllingProjectionPlatforms;

    private const int DRAINER_ID = 4;

    private bool previouslyPowered;

    private void Start()
    {
        InitializeVariables();
    }

    private void Update()
    {
        HandleDrainersInPlatforms();
    }

    private void InitializeVariables()
    {
        previouslyPowered = false;
    }

    private void HandleDrainersInPlatforms()
    {
        if(AllPlatformsWithDrainer() && !previouslyPowered)
        {
            DisableDoor();
            previouslyPowered = true;
        }
        
        if(!AllPlatformsWithDrainer() && previouslyPowered)
        {
            EnableDoor();
            previouslyPowered = false;
        }

    }

    private bool AllPlatformsWithDrainer()
    {
        foreach(ProjectionPlatform projectionPlatform in controllingProjectionPlatforms)
        {
            if (!projectionPlatform.HasObject()) return false;
            if (projectionPlatform.CurrentProjectedObjectSO.id != DRAINER_ID) return false;
        }

        return true;
    }

    private void DisableDoor() => transformToDisable.gameObject.SetActive(false);
    private void EnableDoor() => transformToDisable.gameObject.SetActive(true);
}
