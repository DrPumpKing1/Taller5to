using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SymbolSourcesData
{
    public SerializableDictionary<int, bool> symbolSourcesCollected;

    public SymbolSourcesData()
    {
        symbolSourcesCollected = new SerializableDictionary<int, bool>(); //String -> id; bool -> isCollected
    }
}