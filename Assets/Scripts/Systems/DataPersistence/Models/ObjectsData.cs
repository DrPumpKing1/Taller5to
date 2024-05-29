using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectsData
{
    public SerializableDictionary<int, bool> learningPlatformsUsed;
    public SerializableDictionary<int, bool> instructionsTriggered;

    public SerializableDictionary<int, bool> inscriptionsRead;

    public ObjectsData()
    {
        learningPlatformsUsed = new SerializableDictionary<int, bool>(); //String -> id; bool -> isLearned
        inscriptionsRead = new SerializableDictionary<int, bool>(); //String -> id; bool -> isTranslated
        instructionsTriggered = new SerializableDictionary<int, bool>(); //String -> id. bool -> hasBeenTriggered
    }
}
