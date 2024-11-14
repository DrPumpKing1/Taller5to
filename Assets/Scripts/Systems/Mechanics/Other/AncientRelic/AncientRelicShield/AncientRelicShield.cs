using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AncientRelicShield : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform collidersTransform;

    [Header("Settings")]
    [SerializeField] private List<ProjectionPlatform> controllingProjectionPlatforms;

    private const int DRAINER_ID = 4;

    private bool previouslyPowered;

    public static event EventHandler OnAncientRelicShieldPowered;
    public static event EventHandler OnAncientRelicShieldDepowered;

    public event EventHandler OnThisAncientRelicShieldPowered;
    public event EventHandler OnThisAncientRelicShieldDepowered;

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
            DisableShield();
            previouslyPowered = true;
        }
        
        if(!AllPlatformsWithDrainer() && previouslyPowered)
        {
            EnableShield();
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
    private void EnableShield()
    {
        collidersTransform.gameObject.SetActive(true);
        OnAncientRelicShieldPowered?.Invoke(this, EventArgs.Empty);
        OnThisAncientRelicShieldPowered?.Invoke(this, EventArgs.Empty);
    }

    private void DisableShield()
    {
        collidersTransform.gameObject.SetActive(false);
        OnAncientRelicShieldDepowered?.Invoke(this, EventArgs.Empty);
        OnThisAncientRelicShieldDepowered.Invoke(this, EventArgs.Empty);
    }
}
