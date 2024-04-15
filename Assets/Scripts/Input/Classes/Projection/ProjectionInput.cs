using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectionInput : MonoBehaviour, IProjectionInput
{
    public static ProjectionInput Instance { get; private set; }

    protected virtual void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one ProjectionInput instance");
        }

        Instance = this;
    }

    public abstract bool CanProcessProjectionInput();

    public abstract bool GetNextProjectableObjectDown();
    public abstract bool GetPrevoiusProjectableObjectDown();
}
