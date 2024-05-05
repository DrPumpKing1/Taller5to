using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolsDictionaryPersistenceManager : DataPersistenceManager<SymbolsDictionaryData>
{
    public static SymbolsDictionaryPersistenceManager Instance { get; private set; }

    protected override void SetSingleton()
    {
        if (Instance != null)
        {
            Debug.LogError($"There is more than one SymbolsDictionaryPersistenceManager Instance");
        }

        Instance = this;
    }
}
