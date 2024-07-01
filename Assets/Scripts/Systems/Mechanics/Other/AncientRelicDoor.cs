using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AncientRelicDoor : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform transformToDisable;

    [Header("Settings")]
    [SerializeField] private float timeDisabledAfterDepowered;
    [SerializeField] private List<Electrode> controllingElectrodes;

    private bool previouslyPowered;
    public float timer;

    public const float ACTIVATION_THRESHOLD = 20f;

    private void Start()
    {
        InitializeVariables();
    }

    private void Update()
    {
        HandlePowered();
    }

    private void InitializeVariables()
    {
        previouslyPowered = false;
        timer = 0f;
    }

    private void HandlePowered()
    {
        if(AllControllingElectrodesDePowered())
        {
            ResetTimer();

            if (previouslyPowered)
            {
                DisableDoor();
                previouslyPowered = false;
            }

            return;
        }
        
        if(!AllControllingElectrodesDePowered())
        {
            timer += Time.deltaTime;

            if (timer >= timeDisabledAfterDepowered && !previouslyPowered)
            {
                EnableDoor();
                previouslyPowered = true;
            }
        }

    }

    private bool AllControllingElectrodesDePowered()
    {
        foreach(Electrode electrode in controllingElectrodes)
        {
            if (electrode.Power >= ACTIVATION_THRESHOLD) return false;
        }

        return true;
    }

    private void DisableDoor() => transformToDisable.gameObject.SetActive(false);
    private void EnableDoor() => transformToDisable.gameObject.SetActive(true);
    private void ResetTimer() => timer = 0f;
}
