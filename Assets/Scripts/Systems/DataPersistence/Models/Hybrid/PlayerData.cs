using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public SerializableDictionary<int, bool> symbolsCollected;
    public SerializableDictionary<int, bool> projectableObjectsLearned;
    public int totalProjectionGems;

    public PlayerData()
    {
        symbolsCollected = new SerializableDictionary<int, bool>(); //String -> id; bool -> isCollected
        projectableObjectsLearned = new SerializableDictionary<int, bool>(); //String -> id; bool -> isLearned
        totalProjectionGems = 0;
    }
}
