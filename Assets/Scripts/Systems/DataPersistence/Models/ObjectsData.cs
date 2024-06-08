using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectsData
{
    public SerializableDictionary<int, bool> learningPlatformsUsed;
    public SerializableDictionary<int, bool> switchesToggled;

    public ObjectsData()
    {
        learningPlatformsUsed = new SerializableDictionary<int, bool>(); //String -> id; bool -> isLearned
        switchesToggled = new SerializableDictionary<int, bool>(); //string -> id; bool -> isOn;
    }
}
