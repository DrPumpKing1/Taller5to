using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DialogueInput : MonoBehaviour, IDialogueInput
{
    public static DialogueInput Instance { get; private set; }

    protected virtual void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one MovementInput instance");
        }

        Instance = this;
    }

    public abstract bool CanProcessDialogueInput();
    public abstract bool GetSkipDown();
}
