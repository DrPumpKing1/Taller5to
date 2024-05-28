using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int checkpointID;
    public SerializableDictionary<int, bool> symbolsCollected;
    public SerializableDictionary<int, bool> symbolSourcesCollected;
    public SerializableDictionary<int, bool> shieldPiecesCollected;
    public SerializableDictionary<int, bool> projectableObjectsLearned;
    public int totalProjectionGems;

    public PlayerData()
    {
        checkpointID = 0;
        symbolsCollected = new SerializableDictionary<int, bool>(); //String -> id; bool -> isCollected
        symbolSourcesCollected = new SerializableDictionary<int, bool>(); //String -> id; bool -> isCollected
        projectableObjectsLearned = new SerializableDictionary<int, bool>(); //String -> id; bool -> isLearned
        shieldPiecesCollected = new SerializableDictionary<int, bool>();//String -> id; bool -> isCollected
        totalProjectionGems = 0;
    }
}
