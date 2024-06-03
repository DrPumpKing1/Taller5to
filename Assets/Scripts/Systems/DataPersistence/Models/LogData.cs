using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LogData
{
    public SerializableDictionary<int, bool> uniqueDialoguesTriggered;

    public LogData()
    {
        uniqueDialoguesTriggered = new SerializableDictionary<int, bool>(); //String -> id; bool -> isLearned
    }
}
