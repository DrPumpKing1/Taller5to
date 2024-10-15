using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WorldSpaceInstructionManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private List<WorldSpaceInstruction> worldSpaceInstructions;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private List<WorldSpaceInstruction> currentShownWorldSpaceInstructions = new List<WorldSpaceInstruction>();

    [Serializable]
    public class WorldSpaceInstruction
    {
        public int id;
        public string logToShow;
        public string logToAcomplish;
        public Transform instantiationPosition;
        public Transform prefabTransform;
        [HideInInspector] public Transform instantiatedTransform;
    }


    private void OnEnable()
    {
        GameLogManager.OnLogAdd += GameLogManager_OnLogAdd;
    }

    private void OnDisable()
    {
        GameLogManager.OnLogAdd -= GameLogManager_OnLogAdd;
    }

    private void CheckShowInstruction(string log)
    {
        foreach(WorldSpaceInstruction worldSpaceInstruction in worldSpaceInstructions)
        {
            if (worldSpaceInstruction.logToShow == log)
            {
                InstantiateWorldSpaceInstruction(worldSpaceInstruction);
                currentShownWorldSpaceInstructions.Add(worldSpaceInstruction);
            }
        }
    }

    private void CheckHideInstruction(string log)
    {
        List<WorldSpaceInstruction> instructionsToRemove = new List<WorldSpaceInstruction>();

        foreach (WorldSpaceInstruction worldSpaceInstruction in currentShownWorldSpaceInstructions)
        {
            if (worldSpaceInstruction.logToAcomplish == log)
            {
                AcomplishInstruction(worldSpaceInstruction);
                instructionsToRemove.Add(worldSpaceInstruction);
            }
        }

        foreach(WorldSpaceInstruction instructionToRemove in instructionsToRemove)
        {
            currentShownWorldSpaceInstructions.Remove(instructionToRemove);
        }
    }

    private void InstantiateWorldSpaceInstruction(WorldSpaceInstruction worldSpaceInstruction)
    {
        Transform worldSpaceInstructionTransform = Instantiate(worldSpaceInstruction.prefabTransform, worldSpaceInstruction.instantiationPosition.position, worldSpaceInstruction.instantiationPosition.rotation);
        worldSpaceInstruction.instantiatedTransform = worldSpaceInstructionTransform;
    }

    private void AcomplishInstruction(WorldSpaceInstruction worldSpaceInstruction)
    {
        WorldSpaceInstructionUI worldSpaceInstructionUI = worldSpaceInstruction.instantiatedTransform.GetComponentInChildren<WorldSpaceInstructionUI>();

        if (!worldSpaceInstructionUI)
        {
            if (debug) Debug.Log("Acomplished instruction prefab does not contain a WorldSpaceInstructionUI component");
            return;
        }

        worldSpaceInstructionUI.AcomplishInstruction();
    }

    #region GameLogManager Subscriptions
    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        CheckShowInstruction(e.gameplayAction.log);
        CheckHideInstruction(e.gameplayAction.log);
    }
    #endregion
}
