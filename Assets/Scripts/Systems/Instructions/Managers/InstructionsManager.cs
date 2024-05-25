using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsManager : MonoBehaviour
{
    public static InstructionsManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private Transform intructionUIPrefab;

    private void OnEnable()
    {
        InstructionCollider.OnInstructionColliderTriggered += InstructionCollider_OnInstructionColliderTriggered;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one InstructionsManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void ShowInstruction(string instruction)
    {
        Transform instructionsUITransform = Instantiate(intructionUIPrefab, null);

        InstructionsUI instructionsUI = instructionsUITransform.GetComponentInChildren<InstructionsUI>();

        if (!instructionsUI)
        {
            Debug.LogWarning("There's not a InstructionsUI attached to instantiated prefab");
            return;
        }

        instructionsUI.SetInstructionsText(instruction);
    }

    #region InstructionCollider Subscriptions
    private void InstructionCollider_OnInstructionColliderTriggered(object sender, InstructionCollider.OnInstructionColliderTriggeredEventArgs e)
    {
        ShowInstruction(e.instruction);
    }

    #endregion
}
