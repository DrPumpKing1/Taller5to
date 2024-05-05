using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public SerializableDictionary<int, bool> symbolsCollected;
    public SerializableDictionary<int, bool> projectableObjectsLearned;
    public SerializableDictionary<int, bool> symbolSourcesCollected;
    public SerializableDictionary<int, bool> learningPlatformsUsed;
    public SerializableDictionary<int, bool> inscriptionsTranslated;

    public GameData()
    {
        symbolsCollected = new SerializableDictionary<int, bool>(); //String -> id; bool -> isCollected
        projectableObjectsLearned = new SerializableDictionary<int, bool>(); //String -> id; bool -> isLearned
        symbolSourcesCollected = new SerializableDictionary<int, bool>(); //String -> id; bool -> isCollected
        learningPlatformsUsed = new SerializableDictionary<int, bool>(); //String -> id; bool -> isLearned
        inscriptionsTranslated = new SerializableDictionary<int, bool>(); //String -> id; bool -> isTranslated
    }
}
