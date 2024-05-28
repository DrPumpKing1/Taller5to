using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsTriggeredPersistence : MonoBehaviour, IDataPersistence<ObjectsData>
{
    public void LoadData(ObjectsData data)
    {
        Instruction[] instructions = FindObjectsOfType<Instruction>();

        foreach (Instruction instruction in instructions)
        {
            foreach (KeyValuePair<int, bool> instructionTriggered in data.instructionsTriggered)
            {
                if (instruction.ID == instructionTriggered.Key)
                {
                    if (instructionTriggered.Value) instruction.SetHasBeenTriggered();
                    break;
                }
            }
        }
    }

    public void SaveData(ref ObjectsData data)
    {
        Instruction[] instructions = FindObjectsOfType<Instruction>();

        foreach (Instruction instruction in instructions)
        {
            if (data.instructionsTriggered.ContainsKey(instruction.ID)) data.instructionsTriggered.Remove(instruction.ID);
        }

        foreach (Instruction instruction in instructions)
        {
            bool triggered = instruction.HasBeenTriggered;

            data.instructionsTriggered.Add(instruction.ID, triggered);
        }
    }
}
