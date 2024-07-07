using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueMonologuesTriggeredPersistence : MonoBehaviour, IDataPersistence<LogData>
{
    public void LoadData(LogData data)
    {
        UniqueMonologueTriggerHandler uniqueMonologueTriggerHandler = FindObjectOfType<UniqueMonologueTriggerHandler>();

        foreach (KeyValuePair<int, bool> uniqueMonologueTriggered in data.uniqueMonologuesTriggered)
        {
            uniqueMonologueTriggerHandler.SetUniqueMonologueTriggered(uniqueMonologueTriggered.Key, uniqueMonologueTriggered.Value);
        }
    }

    public void SaveData(ref LogData data)
    {
        UniqueMonologueTriggerHandler uniqueMonologueTriggerHandler = FindObjectOfType<UniqueMonologueTriggerHandler>();

        for (int i = 0; i < uniqueMonologueTriggerHandler.UniqueMonologueEvents.Count; i++)
        {
            if (data.uniqueMonologuesTriggered.ContainsKey(i)) data.uniqueMonologuesTriggered.Remove(i);
        }

        for (int i = 0; i < uniqueMonologueTriggerHandler.UniqueMonologueEvents.Count; i++)
        {
            data.uniqueMonologuesTriggered.Add(i, uniqueMonologueTriggerHandler.UniqueMonologueEvents[i].triggered);
        }
    }
}

