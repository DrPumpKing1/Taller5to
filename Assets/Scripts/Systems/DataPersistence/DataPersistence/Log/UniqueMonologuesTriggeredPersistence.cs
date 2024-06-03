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

        foreach (UniqueMonologueTriggerHandler.UniqueMonologueEvent uniqueMonologueEvent in uniqueMonologueTriggerHandler.UniqueMonologueEvents)
        {
            if (data.uniqueMonologuesTriggered.ContainsKey(uniqueMonologueEvent.id)) data.uniqueMonologuesTriggered.Remove(uniqueMonologueEvent.id);
        }

        foreach (UniqueMonologueTriggerHandler.UniqueMonologueEvent uniqueMonologueEvent in uniqueMonologueTriggerHandler.UniqueMonologueEvents)
        {
            data.uniqueMonologuesTriggered.Add(uniqueMonologueEvent.id, uniqueMonologueEvent.triggered);
        }
    }
}

