using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionInput : MonoBehaviour, IInteractionInput
{
    public static InteractionInput Instance { get; private set; }

    protected virtual void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one CameraInput instance");
        }

        Instance = this;
    }

    public abstract bool CanProcessInteractionInput();

    public abstract bool GetInteractionDown();

    public abstract bool GetInteractionUp();

    public abstract bool GetInteractionHold();
}
