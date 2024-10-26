using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CableVFXHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Electrode electrode;
    [SerializeField] private VisualEffect cableVFX;

    [Header("Settings")]
    [SerializeField] private bool enableVFX = true;
    private bool IsPowered => electrode.Power >= Electrode.ACTIVATION_THRESHOLD;

    private const float NOT_POWERED_TIME_THRESHOLD = 0.5f;
    private float notPoweredTimer;
    private bool previousPowered;

    private void Awake()
    {
        TurnVFX(false);
    }

    private void LateUpdate()
    {
        HandlePowered();
    }

    private void HandlePowered()
    {
        if (!IsPowered)
        {
            notPoweredTimer += Time.deltaTime;

            if (notPoweredTimer >= NOT_POWERED_TIME_THRESHOLD && previousPowered)
            {
                TurnVFX(false);
                previousPowered = false;
            }
        }
        else
        {
            if (!previousPowered)
            {
                TurnVFX(true);
            }

            notPoweredTimer = 0;
            previousPowered = true;
        }
    }

    private void TurnVFX(bool on)
    {
        if (on && enableVFX)
        {
            cableVFX.Play();
        }
        else
        {
            cableVFX.Stop();
        }
    }
}
