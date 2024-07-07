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

        for(int i=0; i< uniqueDialogueTriggerHandler.UniqueDialogueEvents.Count; i++)
        {
            if (data.uniqueDialoguesTriggered.ContainsKey(i)) data.uniqueDialoguesTriggered.Remove(i);
        }

        for (int i = 0; i < uniqueDialogueTriggerHandler.UniqueDialogueEvents.Count; i++)
        {
            data.uniqueDialoguesTriggered.Add(i, uniqueDialogueTriggerHandler.UniqueDialogueEvents[i].triggered);
        }
    }
}
