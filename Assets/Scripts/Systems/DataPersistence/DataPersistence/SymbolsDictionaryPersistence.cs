using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolsDictionaryPersistence : MonoBehaviour, IDataPersistence<PlayerData>
{
    public void LoadData(PlayerData data)
    {
        SymbolsDictionaryManager symbolsDictionaryManager = FindObjectOfType<SymbolsDictionaryManager>();

        foreach (KeyValuePair<int,bool> symbolDictionaryData in data.symbolsCollected)
        {
            if (symbolDictionaryData.Value) symbolsDictionaryManager.AddSymbolToDictionaryById(symbolDictionaryData.Key);
        }
    }

    public void SaveData(ref PlayerData data)
    {
        SymbolsDictionaryManager symbolsDictionaryManager = FindObjectOfType<SymbolsDictionaryManager>();

        foreach (DialectSymbolSO symbol in symbolsDictionaryManager.CompleteSymbolsPool) //Clear all data in data
        {
            if (data.symbolsCollected.ContainsKey(symbol.id)) data.symbolsCollected.Remove(symbol.id);
        }

        foreach (DialectSymbolSO symbol in symbolsDictionaryManager.CompleteSymbolsPool)
        {
            bool collected = symbolsDictionaryManager.CheckIfDictionaryContainsSymbol(symbol);

            data.symbolsCollected.Add(symbol.id, collected);
        }
    }
}
