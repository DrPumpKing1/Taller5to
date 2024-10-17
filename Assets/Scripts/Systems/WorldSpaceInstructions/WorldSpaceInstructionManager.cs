using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static WorldSpaceInstructionManager;

public class WorldSpaceInstructionManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private List<WorldSpaceInstruction> worldSpaceInstructions;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private List<WorldSpaceInstruction> currentShownWorldSpaceInstructions = new List<WorldSpaceInstruction>();

    public static event EventHandler<OnWorldSpaceInstructionEventArgs> OnWorldSpaceInstructionShow;
    public static event EventHandler<OnWorldSpaceInstructionEventArgs> OnWorldSpaceInstructionAcomplished;

    public class OnWorldSpaceInstructionEventArgs : EventArgs
    {
        public int id;
    }

    [Serializable]
    public class WorldSpaceInstruction
    {
        public int id;
        public string logToShow;
        public string logToAcomplish;
        public Transform instantiationPosition;
        public Transform prefabTransform;
        [HideInInspector] public Transform instantiatedTransform;
        [HideInInspector] public bool hasShown = false;
        [HideInInspector] public bool hasBeenAcomplished = false;
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
            if (worldSpaceInstruction.logToShow == log && !worldSpaceInstruction.hasShown)
            {
                InstantiateWorldSpaceInstruction(worldSpaceInstruction);
                currentShownWorldSpaceInstructions.Add(worldSpaceInstruction);

                OnWorldSpaceInstructionShow?.Invoke(this, new OnWorldSpaceInstructionEventArgs { id = worldSpaceInstruction.id });
            }
        }
    }

    private void CheckHideInstruction(string log)
    {
        List<WorldSpaceInstruction> instructionsToRemove = new List<WorldSpaceInstruction>();

        foreach (WorldSpaceInstruction worldSpaceInstruction in currentShownWorldSpaceInstructions)
        {
            if (worldSpaceInstruction.logToAcomplish == log && !worldSpaceInstruction.hasBeenAcomplished && worldSpaceInstruction.hasShown)
            {
                AcomplishInstruction(worldSpaceInstruction);
                instructionsToRemove.Add(worldSpaceInstruction);
            }
        }

        foreach(WorldSpaceInstruction instructionToRemove in instructionsToRemove)
        {
            currentShownWorldSpaceInstructions.Remove(instructionToRemove);
            OnWorldSpaceInstructionAcomplished?.Invoke(this, new OnWorldSpaceInstructionEventArgs { id = instructionToRemove.id });
        }
    }

    private void InstantiateWorldSpaceInstruction(WorldSpaceInstruction worldSpaceInstruction)
    {
        Transform worldSpaceInstructionTransform = Instantiate(worldSpaceInstruction.prefabTransform, worldSpaceInstruction.instantiationPosition.position, worldSpaceInstruction.instantiationPosition.rotation);
        worldSpaceInstruction.instantiatedTransform = worldSpaceInstructionTransform;
        worldSpaceInstruction.hasShown = true;
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
        worldSpaceInstruction.hasBeenAcomplished = true;
    }

    #region GameLogManager Subscriptions
    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        CheckShowInstruction(e.gameplayAction.log);
        CheckHideInstruction(e.gameplayAction.log);
    }
    #endregion
}
