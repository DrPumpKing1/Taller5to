using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LogData
{
    public SerializableDictionary<int, bool> uniqueDialoguesTriggered;
    public SerializableDictionary<int, bool> uniqueMonologuesTriggered;

    public LogData()
    {
        uniqueDialoguesTriggered = new SerializableDictionary<int, bool>(); //String -> indexInList; bool -> isTriggered
        uniqueMonologuesTriggered = new SerializableDictionary<int, bool>(); //String -> indexInList; bool -> isTriggered
    }
}
