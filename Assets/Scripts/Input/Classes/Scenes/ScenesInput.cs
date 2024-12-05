using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScenesInput : MonoBehaviour, IScenesInput
{
    public static ScenesInput Instance { get; private set; }

    protected virtual void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one ScenesInput instance");
        }

        Instance = this;
    }

    public abstract bool CanProcessScenesInput();
    public abstract bool GetSkipHold();
}
