using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolSourcesCollectedPersistence : MonoBehaviour, IDataPersistence<PlayerData>
{
    public void LoadData(PlayerData data)
    {
        SymbolSourcesManager symbolSourcesManager = FindObjectOfType<SymbolSourcesManager>();
        SymbolSource[] symbolSources = FindObjectsOfType<SymbolSource>();

        foreach (KeyValuePair<int, bool> symbolSourceData in data.symbolSourcesCollected)
        {
            if (symbolSourceData.Value) symbolSourcesManager.AddSourceToInventoryById(symbolSourceData.Key);

            foreach (SymbolSource symbolSource in symbolSources)
            {
                if (symbolSource.SymbolSourceSO.id == symbolSourceData.Key)
                {
                    if (symbolSourceData.Value) symbolSource.SetIsCollected();
                    break;
                }
            } 
        }    
    }

    public void SaveData(ref PlayerData data)
    {
        SymbolSourcesManager symbolSourcesManager = FindObjectOfType<SymbolSourcesManager>();

        foreach (SymbolSourceSO source in symbolSourcesManager.CompleteSymbolSourcesPool)
        {
            if (data.symbolSourcesCollected.ContainsKey(source.id)) data.symbolSourcesCollected.Remove(source.id);
        }

        foreach (SymbolSourceSO source in symbolSourcesManager.CompleteSymbolSourcesPool)
        {
            bool collected = symbolSourcesManager.CheckIfInventoryContainsSource(source);

            data.symbolSourcesCollected.Add(source.id, collected);
        }
    }
}
