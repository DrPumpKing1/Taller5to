using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolSourcesCollectedPersistenceManager : DataPersistenceManager<SymbolSourcesData>
{
    public static SymbolSourcesCollectedPersistenceManager Instance { get; private set; }

    protected override void SetSingleton()
    {
        if (Instance != null)
        {
            Debug.LogError($"There is more than one SymbolSourcesCollectedPersistenceManager Instance");
        }

        Instance = this;
    }
}
