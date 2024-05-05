using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolsDictionaryPersistence : MonoBehaviour, IDataPersistence<SymbolsDictionaryData>
{
    [Header("Components")]
    [SerializeField] private SymbolsDictionaryManager symbolsDictionaryManager;
    //Should be refferenced as loading will happen in Awake
    //and SymbolsDictionaryManager SetSingleton() also happens on Awake

    public void LoadData(SymbolsDictionaryData data)
    {
        foreach(KeyValuePair<int,bool> symbolDictionaryData in data.symbolsCollected)
        {
            if (symbolDictionaryData.Value)
            {
                symbolsDictionaryManager.AddSymbolToDictionaryById(symbolDictionaryData.Key);
            }
        }
    }

    public void SaveData(ref SymbolsDictionaryData data)
    {
        foreach(DialectSymbolSO symbol in symbolsDictionaryManager.CompleteSymbolsPool) //Clear all data in data
        {
            if (data.symbolsCollected.ContainsKey(symbol.id))
            {
                data.symbolsCollected.Remove(symbol.id);
            }
        }

        foreach (DialectSymbolSO symbol in symbolsDictionaryManager.CompleteSymbolsPool)
        {
            bool collected = symbolsDictionaryManager.CheckIfDictionaryContainsSymbol(symbol);

            data.symbolsCollected.Add(symbol.id, collected);
        }
    }
}
