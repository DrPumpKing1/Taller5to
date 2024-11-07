using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyzedTeleportationObject : TeleportationObject
{
    [Header("Energyzed Scpecifics")]
    [SerializeField] private Electrode electrode;

    private bool Power => electrode.Power >= Electrode.ACTIVATION_THRESHOLD;

    private float notPoweredTimer = 0f;
    private const float NOT_POWERED_TIME_THRESHOLD = 0.5f;

    private void Update()
    {
        HandlePowered();
    }

    private void HandlePowered()
    {
        if (Power)
        {
            notPoweredTimer = 0f;

            canBeSelected = true;
            isInteractable = true;
        }
        else
        {
            notPoweredTimer += Time.deltaTime;
        }

        if (notPoweredTimer >= NOT_POWERED_TIME_THRESHOLD)
        {
            canBeSelected = false;
            isInteractable = false;
        }
    }
}
