using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolsDictionaryPersistenceUnified : MonoBehaviour, IDataPersistence<GameData>
{
    public void LoadData(GameData data)
    {
        SymbolsDictionaryManager symbolsDictionaryManager = FindObjectOfType<SymbolsDictionaryManager>();

        foreach (KeyValuePair<int, bool> symbolDictionaryData in data.symbolsCollected)
        {
            if (symbolDictionaryData.Value) symbolsDictionaryManager.AddSymbolToDictionaryById(symbolDictionaryData.Key);
        }
    }

    public void SaveData(ref GameData data)
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
