using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SymbolsDictionaryData
{
    public SerializableDictionary<int, bool> symbolsCollected;

    public SymbolsDictionaryData()
    {
        symbolsCollected = new SerializableDictionary<int, bool>(); //String -> id; bool -> isCollected
    }
}
