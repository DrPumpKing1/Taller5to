using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionCollidersPersistence : MonoBehaviour, IDataPersistence<ObjectsData>
{
    public void LoadData(ObjectsData data)
    {
        InstructionCollider[] instructionColliders = FindObjectsOfType<InstructionCollider>();

        foreach (InstructionCollider instructionCollider in instructionColliders)
        {
            foreach (KeyValuePair<int, bool> instructionColliderTriggered in data.instructionCollidersTriggered)
            {
                if (instructionCollider.ID == instructionColliderTriggered.Key)
                {
                    if (instructionColliderTriggered.Value) instructionCollider.SetHasBeenTriggered();
                    break;
                }
            }
        }
    }

    public void SaveData(ref ObjectsData data)
    {
        InstructionCollider[] instructionColliders = FindObjectsOfType<InstructionCollider>();

        foreach (InstructionCollider instructionCollider in instructionColliders)
        {
            if (data.instructionCollidersTriggered.ContainsKey(instructionCollider.ID)) data.instructionCollidersTriggered.Remove(instructionCollider.ID);
        }

        foreach (InstructionCollider instructionCollider in instructionColliders)
        {
            bool triggered = instructionCollider.HasBeenTriggered;

            data.instructionCollidersTriggered.Add(instructionCollider.ID, triggered);
        }
    }
}
