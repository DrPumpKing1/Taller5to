using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueDialoguesTriggeredPersistence : MonoBehaviour, IDataPersistence<LogData>
{
    public void LoadData(LogData data)
    {
        UniqueDialogueTriggerHandler uniqueDialogueTriggerHandler = FindObjectOfType<UniqueDialogueTriggerHandler>();

        foreach (KeyValuePair<int, bool> uniqueDialogueTriggered in data.uniqueDialoguesTriggered)
        {
            uniqueDialogueTriggerHandler.SetUniqueDialogueTriggered(uniqueDialogueTriggered.Key, uniqueDialogueTriggered.Value);
        }    
    }

    public void SaveData(ref LogData data)
    {
        UniqueDialogueTriggerHandler uniqueDialogueTriggerHandler = FindObjectOfType<UniqueDialogueTriggerHandler>();

        foreach (UniqueDialogueTriggerHandler.UniqueDialogueEvent uniqueDialogueEvent in uniqueDialogueTriggerHandler.UniqueDialogueEvents)
        {
            if (data.uniqueDialoguesTriggered.ContainsKey(uniqueDialogueEvent.id)) data.uniqueDialoguesTriggered.Remove(uniqueDialogueEvent.id);
        }

        foreach (UniqueDialogueTriggerHandler.UniqueDialogueEvent uniqueDialogueEvent in uniqueDialogueTriggerHandler.UniqueDialogueEvents)
        {
            data.uniqueDialoguesTriggered.Add(uniqueDialogueEvent.id, uniqueDialogueEvent.triggered);
        }
    }
}
