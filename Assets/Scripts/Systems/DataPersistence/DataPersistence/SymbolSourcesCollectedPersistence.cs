using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolSourcesCollectedPersistence : MonoBehaviour, IDataPersistence<PlayerData>
{
    public void LoadData(PlayerData data)
    {
        SymbolSourcesManager symbolSourcesManager = FindObjectOfType<SymbolSourcesManager>();
        DialectSymbolSource[] dialectSymbolSources = FindObjectsOfType<DialectSymbolSource>();

        foreach (KeyValuePair<int, bool> symbolSourceData in data.symbolSourcesCollected)
        {
            if (symbolSourceData.Value) symbolSourcesManager.AddSourceToInventoryById(symbolSourceData.Key);

            foreach (DialectSymbolSource dialectSymbolSource in dialectSymbolSources)
            {
                if (dialectSymbolSource.DialectSymbolSourceSO.id == symbolSourceData.Key)
                {
                    if (symbolSourceData.Value) dialectSymbolSource.SetIsCollected();
                    break;
                }
            } 
        }    
    }

    public void SaveData(ref PlayerData data)
    {
        SymbolSourcesManager symbolSourcesManager = FindObjectOfType<SymbolSourcesManager>();

        foreach (DialectSymbolSourceSO source in symbolSourcesManager.CompleteSymbolSourcesPool)
        {
            if (data.symbolSourcesCollected.ContainsKey(source.id)) data.symbolSourcesCollected.Remove(source.id);
        }

        foreach (DialectSymbolSourceSO source in symbolSourcesManager.CompleteSymbolSourcesPool)
        {
            bool collected = symbolSourcesManager.CheckIfInventoryContainsSource(source);

            data.symbolSourcesCollected.Add(source.id, collected);
        }
    }
}
