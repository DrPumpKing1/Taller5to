using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectsData
{
    public SerializableDictionary<int, bool> symbolSourcesCollected;
    public SerializableDictionary<int, bool> learningPlatformsUsed;
    public SerializableDictionary<int, bool> inscriptionsTranslated;

    public ObjectsData()
    {
        symbolSourcesCollected = new SerializableDictionary<int, bool>(); //String -> id; bool -> isCollected
        learningPlatformsUsed = new SerializableDictionary<int, bool>(); //String -> id; bool -> isLearned
        inscriptionsTranslated = new SerializableDictionary<int, bool>(); //String -> id; bool -> isTranslated
    }
}
