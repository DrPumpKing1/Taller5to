using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolsDictionaryPersistence : MonoBehaviour, IDataPersistence<PlayerData>
{
    public void LoadData(PlayerData data)
    {
        SymbolsManager symbolsDictionaryManager = FindObjectOfType<SymbolsManager>();

        foreach (KeyValuePair<int,bool> symbolDictionaryData in data.symbolsCollected)
        {
            if (symbolDictionaryData.Value) symbolsDictionaryManager.AddSymbolToDictionaryById(symbolDictionaryData.Key);
        }
    }

    public void SaveData(ref PlayerData data)
    {
        SymbolsManager symbolsDictionaryManager = FindObjectOfType<SymbolsManager>();

        foreach (SymbolSO symbol in symbolsDictionaryManager.CompleteSymbolsPool) //Clear all data in data
        {
            if (data.symbolsCollected.ContainsKey(symbol.id)) data.symbolsCollected.Remove(symbol.id);
        }

        foreach (SymbolSO symbol in symbolsDictionaryManager.CompleteSymbolsPool)
        {
            bool collected = symbolsDictionaryManager.CheckIfDictionaryContainsSymbol(symbol);

            data.symbolsCollected.Add(symbol.id, collected);
        }
    }
}
